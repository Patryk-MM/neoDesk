import {SimpleUserDTO} from "./user.interface";

export interface Comment {
  id: number;
  content: string;
  createdAt: string;
  ticketId: number;
  user: SimpleUserDTO;
}
