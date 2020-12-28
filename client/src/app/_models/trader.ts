import { TradeLog } from "./trade-log";

export interface Trader {
  totalTrades: number;
  winningTrades: number;
  losingTrades: number;
  openPositions: number;
  winRate: number;
  totalProfit: number;
  trades: TradeLog[];
}