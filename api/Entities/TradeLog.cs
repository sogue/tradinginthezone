using System;

namespace API.Entities
{
    public class TradeLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Ticker { get; set; }
        public decimal Volume { get; set; }
        public string Instrument { get; set; }
        public int AppUserId { get; set; }
        public string Type { get; internal set; }
        public decimal BaseValue { get; internal set; }
        public decimal ExchangeRate { get; internal set; }
        public decimal Commission { get; internal set; }
        public decimal TotalValue { get; internal set; }
        public decimal Multiplier { get; internal set; }
        public string Currency { get; internal set; }
        public string Direction { get; internal set; }
        public string DirectionName { get; internal set; }
        public string Description { get; internal set; }
        public string Exchange { get; internal set; }
        public string ContractName { get; internal set; }
        public string TransactionId { get; internal set; }
    }
}