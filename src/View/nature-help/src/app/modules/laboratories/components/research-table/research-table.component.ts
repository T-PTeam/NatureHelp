import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { withLatestFrom } from "rxjs";

import { sortIndicator, toggleSort } from "@/shared/helpers/table-sort.helper";
import { ITableSort } from "@/shared/models/ITableSort";
import { ResearchesAPIService } from "../../services/researches-api.service";

@Component({
  selector: "app-research-table",
  templateUrl: "./research-table.component.html",
  styleUrls: ["../../../../shared/styles/table-list.component.css", "./research-table.component.css"],
  standalone: false,
})
export class ResearchTableComponent implements OnInit {
  public scrollCheckDisabled: boolean = false;
  public sort: ITableSort | null = null;
  public sortIndicator = sortIndicator;

  private listScrollCount = 0;

  constructor(
    public researchesAPIService: ResearchesAPIService,
    private router: Router,
  ) {}

  ngOnInit() {}

  switchTable() {
    this.router.navigateByUrl("labs");
  }

  onSort(field: string) {
    this.sort = toggleSort(this.sort, field);
    this.listScrollCount = 0;
    this.researchesAPIService.loadResearches(0, this.sort);
  }

  onScroll() {
    this.listScrollCount++;
    this.researchesAPIService.loadResearches(this.listScrollCount, this.sort);

    this.researchesAPIService.researches$
      .pipe(withLatestFrom(this.researchesAPIService.totalCount$))
      .subscribe(([researches, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= researches.length;
      });
  }
}
