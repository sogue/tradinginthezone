import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { TradeLogDetail } from '../_models/trade-log-detail';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-trades',
  templateUrl: './trades.component.html',
  styleUrls: ['./trades.component.scss']
})
export class TradesComponent implements OnInit {
  user: User;
  results: TradeLogDetail[];
  searchText: string;
  constructor(public accountService: AccountService, public memberService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMemberTrades();
    console.log(this.results);
  }
  loadMemberTrades() {
    this.memberService.getMemberTrades(this.user.userName).subscribe(results => {
      this.results = results;
    })
  }

}
