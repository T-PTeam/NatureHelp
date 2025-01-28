import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-toggle-switch',
  templateUrl: './toggle-switch.component.html',
  styleUrls: ['./toggle-switch.component.css']
})
export class ToggleSwitchComponent {
  @Input() isChecked: boolean = true; 
  @Output() toggle = new EventEmitter<boolean>();

  onToggle(): void {
    this.isChecked = !this.isChecked;
    this.toggle.emit(this.isChecked);
  }
}