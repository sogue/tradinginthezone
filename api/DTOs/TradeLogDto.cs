using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class TradeLogDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Ticker { get; set; }
        public decimal? Profit { get; set; }
        public decimal Volume { get; set; }
        public bool IsOpen { get; set; }
        public ICollection<string> Transactions { get; set; } = new List<string>();
    }
}