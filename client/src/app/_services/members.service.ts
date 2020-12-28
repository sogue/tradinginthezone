import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { TradeLog } from '../_models/trade-log';
import { TradeLogDetail } from '../_models/trade-log-detail';
import { Trader } from '../_models/trader';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl = environment.apiURL;
  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  getMemberTrades(userName: string) {
    return this.http.get<Trader>(this.baseUrl + 'users/profiles/' + userName);
  }

  sendTradeLogFile(formData: FormData) {
    return this.http.post<TradeLog[]>(this.baseUrl + 'upload', formData);
  }
}
