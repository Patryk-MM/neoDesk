import {TicketCategory, TicketStatus} from "./ticket.enums";

export enum SortOptions {
  Id = 'Id',
  Title = 'Title',
  CreatedAt = 'CreatedAt',
  Status = 'Status',
  Category = 'Category'
}

export enum SortDirection {
  Asc = 'Asc',
  Desc = 'Desc',
}

export interface TicketFilterParams {
  sortBy?: SortOptions;
  sortDir?: SortDirection;

  pageIndex?: number;
  pageSize: number;

  searchTerm?: string;
  statuses?: TicketStatus[];
  categories?: TicketCategory[];
}
