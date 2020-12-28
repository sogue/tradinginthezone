using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class TradeLogService : ITradeLogService
    {
        public ICollection<TradeLogDto> CreateTradeLogDtosFromTradeLog(ICollection<TradeLog> tradeLogs)
        {
            var tradeLogDtos = GroupTradesByTicker(tradeLogs);
            tradeLogDtos = MarkOpenTrades(tradeLogDtos);
            return tradeLogDtos;
        }

        private static ICollection<TradeLogDto> GroupTradesByTicker(ICollection<TradeLog> tradeLogs)
        {
            return tradeLogs.GroupBy(i => new { i.Ticker }).Select(g => new TradeLogDto
            {
                Date = g.Select(i => i.Date).OrderBy(i => i).LastOrDefault(),
                Ticker = g.Key.Ticker,
                Volume = g.Sum(i => i.Volume),
                Profit = g.Sum(i => -i.Profit),
                Transactions = g.Select(i => i.Instrument).ToList(),
            }).OrderByDescending(x => x.Profit).ToList();
        }

        public ICollection<TradeLogDto> MarkOpenTrades(ICollection<TradeLogDto> tradeLogDtos)
        {
            foreach (var tradeLogDto in tradeLogDtos)
            {
                if (tradeLogDto.Volume != 0)
                    tradeLogDto.IsOpen = true;
            }

            return tradeLogDtos;
        }

        public ICollection<TradeLogDto> GetOpenTrades(ICollection<TradeLogDto> tradeLogDtos)
        {
            var openTrades = tradeLogDtos.Where(x => x.Volume != 0);
            return openTrades.ToList();
        }
        public double CalculateWinRate(ICollection<TradeLogDto> tradeLogDtos)
        {
            var winningTrades = tradeLogDtos.Count(log => log.Profit >= 0);
            return (double)winningTrades / tradeLogDtos.Count;
        }

        public decimal CalculateTotalProfit(ICollection<TradeLogDto> tradeLogDtos)
        {
            return tradeLogDtos.Where(log => !log.IsOpen).Sum(log => log.Profit).GetValueOrDefault();
        }

        public async Task<ICollection<TradeLog>> AddTradesAsync(IFormFile file)
        {
            ICollection<TradeLog> logs = new Collection<TradeLog>();

            using var streamReader = new StreamReader(file.OpenReadStream());
            string line;
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                var log = ConvertLineToTradeLog(line);
                if (log != null) logs.Add(log);
            }

            return logs;
        }



        private TradeLog ConvertLineToTradeLog(string line)
        {
            string[] splitLine = line.Split('|');
            return splitLine.Length == 16
                ? new TradeLog
                {
                    Ticker = splitLine[3].Split(' ').FirstOrDefault(),
                    Instrument = splitLine[3],
                    Date = DateTime.ParseExact(splitLine[7], "yyyyMMdd", CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(splitLine[10], CultureInfo.InvariantCulture),
                    Profit = decimal.Parse(splitLine[13], CultureInfo.InvariantCulture),
                }
                : null;
        }
    }
}