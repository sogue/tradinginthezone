import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { Member } from '../_models/member';
import { TradeLog } from '../_models/trade-log';
import { TradeLogDetail } from '../_models/trade-log-detail';
import { Trader } from '../_models/trader';
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
  trader: Trader;
  trades: TradeLog[];
  searchText: string;
  constructor(public accountService: AccountService, public memberService: MembersService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadTrader();
  }
  loadTrader() {
    this.memberService.getMemberTrades(this.user.userName).subscribe(result => {
      this.trader = result;
      this.trades = result.trades;
    })
  }

}
