import { ReportAPIService } from "@/shared/services/report-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { Component, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MatDialog } from "@angular/material/dialog";
import { withLatestFrom, combineLatest, Observable } from "rxjs";
import { map, startWith } from "rxjs/operators";
import { IUser } from "@/models/IUser";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AddOrganizationUsersComponent } from "@/shared/components/dialogs/add-organization-users/add-organization-users.component";
import { EAuthType, ERole } from "@/models/enums";

@Component({
  selector: "nat-organization-users-table",
  templateUrl: "./organization-users-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./organization-users-table.component.css"],
  standalone: false,
})
export class OrganizationUsersTableComponent implements OnInit {
  public scrollCheckDisabled: boolean = false;
  public combinedUsers: Observable<(IUser & { isActive: boolean })[]>;
  public filterForm: FormGroup;
  public changedUsersRoles = new Map<string, number>();
  public ERole = ERole;
  public roleOptions = [
    { value: ERole.Owner, label: "Owner" },
    { value: ERole.Manager, label: "Manager" },
    { value: ERole.Supervisor, label: "Supervisor" },
    { value: ERole.Researcher, label: "Researcher" },
  ];

  private listScrollCount = 0;

  constructor(
    public usersAPIService: UserAPIService,
    private reportAPIService: ReportAPIService,
    private notify: MatSnackBar,
    private fb: FormBuilder,
    private dialog: MatDialog,
  ) {
    this.filterForm = this.fb.group({
      search: [""],
      roleFilter: [""],
      statusFilter: [""],
    });
    this.combinedUsers = combineLatest([
      this.usersAPIService.$organizationUsers,
      this.usersAPIService.$notLoginEverOrganizationUsers,
      this.filterForm.valueChanges.pipe(startWith(this.filterForm.value)),
    ]).pipe(
      map(([orgUsers, notLoginEverUsers, filters]) => {
        const activeUsers = orgUsers.map((user) => ({ ...user, isActive: true }));
        const inactiveUsers = notLoginEverUsers.map((user) => ({ ...user, isActive: false }));
        let combined = [...activeUsers, ...inactiveUsers];

        if (filters.search) {
          const searchLower = filters.search.toLowerCase();
          combined = combined.filter(
            (user) =>
              user.firstName?.toLowerCase().includes(searchLower) ||
              user.lastName?.toLowerCase().includes(searchLower) ||
              user.email?.toLowerCase().includes(searchLower),
          );
        }

        if (filters.roleFilter) {
          const roleValue = Number(filters.roleFilter);
          combined = combined.filter((user) => user.role === roleValue);
        }

        if (filters.statusFilter) {
          const isActive = filters.statusFilter === "active";
          combined = combined.filter((user) => user.isActive === isActive);
        }

        return combined;
      }),
    );
  }

  ngOnInit(): void {
    this.usersAPIService.loadOrganizationUsers(this.listScrollCount);
    this.usersAPIService.loadNotLoginEverOrganizationUsers();
  }

  downloadExcel() {
    this.reportAPIService.downloadOrgUsersExcelListFile();
  }

  addUserToOrganization(isMultiple: boolean): void {
    if (isMultiple) {
      const dialogRef = this.dialog.open(AddOrganizationUsersComponent, {
        width: "fit-content",
        height: "fit-content",
        maxHeight: "80vh",
        maxWidth: "80vw",
        minHeight: "300px",
        minWidth: "400px",
        data: {
          isAddingOneUser: false,
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.usersAPIService.addOrganizationUsers(EAuthType.AddMultipleToOrganization, result.users).subscribe({
            error: (err) => {
              this.notify.open("Add multiple users to Your organization failed...", "Close", {
                duration: 2000,
              });
              return err;
            },
          });
        }
      });
    } else {
      const dialogRef = this.dialog.open(AddOrganizationUsersComponent, {
        width: "50vw",
        maxWidth: "90vw",
        data: {
          isAddingOneUser: true,
        },
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result) {
          this.usersAPIService.addOrganizationUser(EAuthType.AddOneToOrganization, result.users[0]).subscribe({
            error: (err) => {
              this.notify.open("Add new user to Your organization failed...", "Close", {
                duration: 2000,
              });
              return err;
            },
          });
        }
      });
    }
  }

  userRoleChanged(userId: string, role: number) {
    this.changedUsersRoles.set(userId, role);
  }

  saveChangedRoles() {
    if (this.changedUsersRoles.size) this.usersAPIService.changeUsersRoles(this.changedUsersRoles);
    else this.notify.open("Nothing to update...", "Close", { duration: 2000 });
  }

  onScroll() {
    this.listScrollCount++;
    this.usersAPIService.loadOrganizationUsers(this.listScrollCount);

    this.usersAPIService.$organizationUsers
      .pipe(withLatestFrom(this.usersAPIService.$totalCount))
      .subscribe(([users, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= users.length;
      });
  }
}
