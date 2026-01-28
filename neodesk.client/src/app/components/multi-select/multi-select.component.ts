import {Component, Input, Output, EventEmitter} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";
import {Ticket} from "../../models/ticket.interface";
import {TicketStatus} from "../../models/ticket.enums";

@Component({
  selector: 'app-multi-select',
  standalone: true,
  template: `
    <div class="relative inline-block w-48">

      <div
        (click)="toggleOpen()"
        class="select select-bordered w-full flex items-center justify-between cursor-pointer">
        <span>Selected Option</span>
      </div>

      <div *ngIf="isOpen"
           class="absolute top-full left-0 z-50 mt-1 w-full rounded-md border border-base-300 bg-base-100 p-1 shadow-lg max-h-60 overflow-auto">
        <div class="flex p-1" *ngFor="let option of options">
          <input type="checkbox" (change)="onCheckChange($event, option.value)">
          <label for="test" class="pl-2">{{option.label}}</label>
        </div>
      </div>
    </div>

  `,
  imports: [
    NgIf,
    NgForOf
  ]
})
export class MultiSelectComponent<T> {
  @Input() options: {value: T, label: string}[] = [];
  @Output() selectedChange: EventEmitter<T[]> = new EventEmitter();

  protected selected: T[] = [];

  isOpen = false;


  toggleOpen() {
    this.isOpen = !this.isOpen;
  }

  onCheckChange(event: Event, value: T): void {
    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) {
      this.selected.push(value);
    } else {
      this.selected = this.selected.filter(t => t !== value)
    }

    this.selectedChange.emit(this.selected);
    console.log(this.selected);
  }
}
