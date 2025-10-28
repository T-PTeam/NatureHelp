import { Component } from "@angular/core";
import { TranslateModule } from "@ngx-translate/core";
import { MatIconModule } from "@angular/material/icon";

@Component({
  selector: "app-contacts",
  standalone: true,
  imports: [TranslateModule, MatIconModule],
  templateUrl: "./contacts.component.html",
  styleUrls: ["./contacts.component.css"],
})
export class ContactsComponent {
  constructor() {}
}
