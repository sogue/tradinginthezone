export interface TradeLogDetail {
  id: number;
  date: Date;
  ticker: string;
  profit: number;
  volume: number;
  instrument: string;
  appUserId: number;
}