export interface TradeLog {
  id: number;
  date: Date;
  ticker: string;
  profit: number;
  volume: number;
  isOpen: boolean;
  transactions: string[];
}