export interface TradeLog {
    id: number;
    date: Date;
    ticker: string;
    profit?: any;
    volume: number;
    instrument?: any;
    appUserId: number;
    type: string;
    baseValue: number;
    exchangeRate: number;
    commission: number;
    totalValue: number;
    multiplier: number;
    currency: string;
    direction: string;
    directionName: string;
    description: string;
    exchange: string;
    contractName: string;
    transactionId: string;
}