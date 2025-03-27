import { Component, OnInit } from '@angular/core';
import { LabsAPIService } from '../../services/labs-api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-research-table',
  templateUrl: './research-table.component.html',
  styleUrls: ['./research-table.component.css'],
  standalone: false,
})
export class ResearchTableComponent implements OnInit {
  public search: string = "";

  constructor(public labsAPI: LabsAPIService,
    private router: Router
  ) {}

  ngOnInit() {
  }

  switchTable(){
    this.router.navigateByUrl('labs');
  }
}