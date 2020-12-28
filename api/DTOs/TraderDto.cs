using System;
using System.Collections.Generic;

namespace API.DTOs
{
    public class TraderDto
    {
        public int TotalTrades { get; set; }
        public int WinningTrades { get; set; }
        public int LosingTrades { get; set; }
        public int OpenPositions { get; set; }
        public decimal? WinRate { get; set; }
        public decimal? TotalProfit { get; set; }
        public ICollection<TradeLogDto> Trades { get; set; } = new List<TradeLogDto>();
    }
}