import { Component, OnInit } from '@angular/core';
import { LabsAPIService } from '../../services/labs-api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'nat-labs-table',
  templateUrl: './labs-table.component.html',
  styleUrls: ['./labs-table.component.css'],
  standalone: false,
})
export class LabsTableComponent implements OnInit {
  public search: string = "";

  constructor(
    public labsAPI: LabsAPIService,
    private router: Router,
  ) {
   }

  ngOnInit (): void {
  }

  public navigateToDetail(id?: string){
    if (id) {
      this.router.navigate([`/labs/${id}`]);
    } else {
      this.router.navigate([`labs/add`]);
    }
  }
}
