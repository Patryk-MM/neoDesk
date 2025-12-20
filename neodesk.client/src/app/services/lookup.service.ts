import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {SimpleUserDTO} from "../models/user.interface";

@Injectable({
  providedIn: 'root'
})

export class LookupService {
  private apiUrl = '/api/lookup';

  constructor(private http: HttpClient) {}

  getTechnicians(): Observable<SimpleUserDTO[]>{
    return this.http.get<SimpleUserDTO[]>(`${this.apiUrl}/get_technicians`);
  }
}
