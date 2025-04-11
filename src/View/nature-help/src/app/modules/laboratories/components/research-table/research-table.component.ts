import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ResearchesAPIService } from "../../services/researches-api.service";
import { withLatestFrom } from "rxjs";

@Component({
  selector: "app-research-table",
  templateUrl: "./research-table.component.html",
  styleUrls: ["./research-table.component.css", "../../../../shared/styles/table-list.component.css"],
  standalone: false,
})
export class ResearchTableComponent implements OnInit {
  public search: string = "";
  public scrollCheckDisabled: boolean = false;

  private listScrollCount = 0;

  constructor(
    public researchesAPIService: ResearchesAPIService,
    private router: Router,
  ) {}

  ngOnInit() {}

  switchTable() {
    this.router.navigateByUrl("labs");
  }

  onScroll() {
    this.listScrollCount++;
    this.researchesAPIService.loadResearches(this.listScrollCount);

    this.researchesAPIService.researches$
      .pipe(withLatestFrom(this.researchesAPIService.totalCount$))
      .subscribe(([researches, totalCount]) => {
        this.scrollCheckDisabled = totalCount <= researches.length;
      });
  }
}
