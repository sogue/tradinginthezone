import { TradeLog } from './trade-log';

export interface Member {
  id: number;
  username: string;
  created: Date;
  lastActive: Date;
  tradeLogs: TradeLog[];
}