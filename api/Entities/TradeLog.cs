using System;

namespace API.Entities
{
    public class TradeLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Ticker { get; set; }
        public decimal? Profit { get; set; }
        public decimal Volume { get; set; }
        public string Instrument { get; set; }
        public int AppUserId { get; set; }
    }
}