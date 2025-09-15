import { Component, OnInit } from "@angular/core";
import { UserAPIService } from "./shared/services/user-api.service";

@Component({
  selector: "n-root",
  templateUrl: "./app.component.html",
  styleUrl: "./app.component.css",
  standalone: false,
})
export class AppComponent implements OnInit {
  constructor(private userService: UserAPIService) {}

  ngOnInit() {
    this.userService.initializeAuth().subscribe();
  }
}
