import { Component, Input, Output, EventEmitter, ElementRef, HostListener } from '@angular/core';
import { NgForOf, NgIf } from "@angular/common";

@Component({
  selector: 'app-multi-select',
  standalone: true,
  template: `
    <div class="relative inline-block w-48">

      <div
        (click)="toggleOpen()"
        class="select select-bordered w-full flex items-center justify-between cursor-pointer">
        <span class="text-gray-400" *ngIf="selected.length === 0">{{placeholder}}</span>
        <span class="truncate mr-2" *ngIf="selected.length > 0">{{displayLabel}}</span>
      </div>

      <div *ngIf="isOpen"
           class="absolute top-full left-0 z-50 mt-1 w-full rounded-md border border-base-300 bg-base-100 p-1 shadow-lg max-h-60 overflow-auto">
        <div class="flex p-1" *ngFor="let option of options; let i = index">
          <input type="checkbox"
                 [id]="'opt-' + i"
                 [checked]="selected.includes(option.value)"
                 (change)="onCheckChange($event, option.value)">
          <label [for]="'opt-' + i" class="pl-2 cursor-pointer">{{option.label}}</label>
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
  @Input() placeholder: string = 'Select...';
  @Output() selectedChange: EventEmitter<T[]> = new EventEmitter();

  protected selected: T[] = [];
  isOpen = false;

  constructor(private eRef: ElementRef) {}

  @HostListener('document:click', ['$event'])
  clickout(event: Event) {
    if (!this.eRef.nativeElement.contains(event.target)) {
      this.isOpen = false;
    }
  }

  toggleOpen() {
    this.isOpen = !this.isOpen;
  }

  get displayLabel(): string {
    return this.selected
      .map(val => this.options.find(opt => opt.value === val)?.label)
      .filter(label => !!label)
      .join(', ');
  }

  onCheckChange(event: Event, value: T): void {
    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) {
      this.selected.push(value);
    } else {
      this.selected = this.selected.filter(t => t !== value);
    }

    this.selectedChange.emit(this.selected);
  }
}
