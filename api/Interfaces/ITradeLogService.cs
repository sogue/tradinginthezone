using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITradeLogService
    {
        ICollection<TradeLogDto> CreateTradeLogDtosFromTradeLog(ICollection<TradeLog> tradeLogs);

        Task<ICollection<TradeLog>> AddTradesAsync(IFormFile file);

        double CalculateWinRate(ICollection<TradeLogDto> tradeLogs);

        decimal CalculateTotalProfit(ICollection<TradeLogDto> tradeLogs);

    }
}