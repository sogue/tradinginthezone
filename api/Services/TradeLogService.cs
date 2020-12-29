using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Http;

namespace API.Services
{
    public class TradeLogService : ITradeLogService
    {
        public IEnumerable<TradeLogDto> CreateTradeLogDtos(IEnumerable<TradeLog> tradeLogs)
        {
            var tradeLogDtos = GroupTradesByTicker(tradeLogs);
            return tradeLogDtos;
        }

        private static IEnumerable<TradeLogDto> GroupTradesByTicker(IEnumerable<TradeLog> tradeLogs)
        {
            return tradeLogs.GroupBy(i => new {i.Ticker}).Select(g => new TradeLogDto
            {
                Date = g.Select(i => i.Date).OrderBy(i => i).LastOrDefault(),
                Ticker = g.Key.Ticker,
                Volume = g.Sum(i => i.Volume),
                IsOpen = g.Sum(i => i.Volume) != 0,
                Profit = g.Sum(i => -i.Profit),
                Transactions = g.Select(i => i.Instrument).ToList()
            }).OrderByDescending(x => x.Profit);
        }

        public TraderDto CreateTraderProfile(IEnumerable<TradeLog> trades)
        {
            var tradeLogDtos = CreateTradeLogDtos(trades).ToList();

            return new TraderDto
            {
                TotalTrades = tradeLogDtos.Count,
                WinningTrades = tradeLogDtos.Where(y => y.IsOpen == false).Count(x => x.Profit >= 0),
                LosingTrades = tradeLogDtos.Where(y => y.IsOpen == false).Count(x => x.Profit < 0),
                OpenPositions = tradeLogDtos.Count(x => x.Volume != 0),
                TotalProfit = tradeLogDtos.Where(x => !x.IsOpen).Sum(log => log.Profit),
                WinRate = decimal.Divide(tradeLogDtos.Count(x => x.Profit >= 0), tradeLogDtos.Count(x => !x.IsOpen)),
                Trades = tradeLogDtos
            };
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

            return logs.OrderBy(x => x.Date).ToList();
        }

        private TradeLog ConvertLineToTradeLog(string line)
        {
            var splitLine = line.Split('|');
            return splitLine.Length == 16
                ? new TradeLog
                {
                    Ticker = splitLine[3].Split(' ').FirstOrDefault(),
                    Instrument = splitLine[3],
                    Date = DateTime.ParseExact(splitLine[7], "yyyyMMdd", CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(splitLine[10], CultureInfo.InvariantCulture),
                    Profit = decimal.Parse(splitLine[13], CultureInfo.InvariantCulture)
                }
                : null;
        }
    }
}