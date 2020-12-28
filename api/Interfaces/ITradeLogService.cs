using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ITradeLogService
    {
        Task<ICollection<TradeLog>> AddTradesAsync(IFormFile file);
        TraderDto CreateTraderProfile(IEnumerable<TradeLog> trades);
    }
}