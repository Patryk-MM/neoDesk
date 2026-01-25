import { Pipe, PipeTransform } from '@angular/core';
import {TicketStatus} from "../models/ticket.enums";
import {TICKET_STATUS_LABELS} from "../models/ticket.constants";

@Pipe({
  name: 'statusLabel'
})
export class StatusLabelPipe implements PipeTransform {

  transform(value: TicketStatus): string {
    return TICKET_STATUS_LABELS[value] || 'Nieznany';
  }

}
