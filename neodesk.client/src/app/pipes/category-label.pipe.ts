import {Pipe, PipeTransform} from '@angular/core';
import {categoryOptions, TicketCategory} from "../models/ticket.enums";
import {TICKET_CATEGORY_LABELS} from "../models/ticket.constants";

@Pipe({
  name: 'categoryLabel'
})
export class CategoryLabelPipe implements PipeTransform {

  transform(value: TicketCategory): string {
   return TICKET_CATEGORY_LABELS[value] || 'Nieznana';
  }
}
