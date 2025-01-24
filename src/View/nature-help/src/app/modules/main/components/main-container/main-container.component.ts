import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'n-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.css'],
  standalone: false,
})
export class MainContainerComponent {
  showWaterTable: boolean = true;

  constructor(private router:Router) {

   }

  toggleTable(): void {
    this.showWaterTable = !this.showWaterTable;
    if(this.showWaterTable)
    {
      this.router.navigate(['water']);
    }
    else{
      this.router.navigate(['soil']);
    }
  }

}
