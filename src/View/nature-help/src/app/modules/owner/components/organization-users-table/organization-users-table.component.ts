import { ReportAPIService } from "@/shared/services/report-api.service";
import { UserAPIService } from "@/shared/services/user-api.service";
import { Component } from "@angular/core";
import { MatSnackBar } from "@angular/material/snack-bar";
import { withLatestFrom } from "rxjs";

@Component({
  selector: "nat-organization-users-table",
  templateUrl: "./organization-users-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./organization-users-table.component.css" ],
  standalone: false,
})
export class OrganizationUsersTableComponent {
  public search: string = "";
  public scrollCheckDisabled: boolean = false;
  public isShowingAllOrgUsers: boolean = true;

  private listScrollCount = 0;
  private changedUsersRoles = new Map<string, number>();

  constructor(
    public usersAPIService: UserAPIService,
    private reportAPIService: ReportAPIService,
    private notify: MatSnackBar,
  ) {}

  downloadExcel() {
    this.reportAPIService.downloadOrgUsersExcelListFile();
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

  switchOrganizationUsers() {
    this.isShowingAllOrgUsers = !this.isShowingAllOrgUsers;

    if (!this.isShowingAllOrgUsers) {
      this.usersAPIService.loadNotLoginEverOrganizationUsers();
    } else {
      this.listScrollCount = 0;
      this.usersAPIService.loadOrganizationUsers(this.listScrollCount);
    }
  }
}
