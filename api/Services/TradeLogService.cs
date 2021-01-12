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
            IEnumerable<TradeLogDto> tradeLogDtos = GroupTradesByTicker(tradeLogs);
            return tradeLogDtos;
        }

        private static IEnumerable<TradeLogDto> GroupTradesByTicker(IEnumerable<TradeLog> tradeLogs)
        {
            return tradeLogs.GroupBy(i => new { i.Ticker }).Select(g => new TradeLogDto
            {
                Date = g.Select(i => i.Date).OrderBy(i => i).LastOrDefault(),
                Ticker = g.Key.Ticker,
                Volume = g.Sum(i => i.Volume),
                IsOpen = g.Sum(i => i.Volume) != 0,
                Profit = g.Sum(i => -i.TotalValue),
                Transactions = g.Select(i => i.Description).ToList()
            }).OrderByDescending(x => x.Profit);
        }

        public TraderDto CreateTraderProfile(IEnumerable<TradeLog> trades)
        {
            if (trades == null) return new TraderDto();
            List<TradeLogDto> tradeLogDtos = CreateTradeLogDtos(trades).ToList();

            return tradeLogDtos.Count > 1
                ? MapTraderDto(tradeLogDtos)
                : new TraderDto();

        }

        private TraderDto MapTraderDto(List<TradeLogDto> tradeLogDtos)
        {
            return new TraderDto
            {
                TotalTrades = tradeLogDtos.Count,
                WinningTrades = tradeLogDtos.Where(y => y.IsOpen == false).Count(x => x.Profit >= 0),
                LosingTrades = tradeLogDtos.Where(y => y.IsOpen == false).Count(x => x.Profit < 0),
                OpenPositions = tradeLogDtos.Count(x => x.Volume != 0),
                TotalProfit = tradeLogDtos.Where(x => !x.IsOpen).Sum(log => log.Profit),
                WinRate =
                    decimal.Divide(tradeLogDtos.Count(x => x.Profit >= 0), tradeLogDtos.Count(x => !x.IsOpen)),
                Trades = tradeLogDtos
            };
        }

        public async Task<ICollection<TradeLog>> AddTradesAsync(IFormFile file)
        {
            ICollection<TradeLog> logs = new Collection<TradeLog>();

            using StreamReader streamReader = new StreamReader(file.OpenReadStream());
            string line;
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                TradeLog log = ConvertLineToTradeLog(line);
                if (log != null) logs.Add(log);
            }

            return logs.OrderBy(x => x.Date).ToList();
        }

        private TradeLog ConvertLineToTradeLog(string line)
        {

            string[] splitLine = line.Split('|');

            return splitLine.Length == 16
                ? NewMethod(splitLine)
                : null;
        }

        private static TradeLog NewMethod(string[] splitLine)
        {
            var log = new TradeLog
            {
                Type = splitLine[0],
                TransactionId = splitLine[1],
                ContractName = splitLine[2],
                Ticker = splitLine[3].Split(' ').FirstOrDefault(),
                Description = splitLine[3],
                Exchange = splitLine[4],
                DirectionName = splitLine[5],
                Direction = splitLine[6],
                Date = DateTime.ParseExact(splitLine[7] + splitLine[8], "yyyyMMddHH:mm:ss", CultureInfo.InvariantCulture),
                Currency = splitLine[9],
                Volume = decimal.Parse(splitLine[10], CultureInfo.InvariantCulture),
                Multiplier = decimal.Parse(splitLine[11], CultureInfo.InvariantCulture),
                BaseValue = decimal.Parse(splitLine[12], CultureInfo.InvariantCulture),
                TotalValue = decimal.Parse(splitLine[13], CultureInfo.InvariantCulture),
                Commission = decimal.Parse(splitLine[14], CultureInfo.InvariantCulture),
                ExchangeRate = decimal.Parse(splitLine[15], CultureInfo.InvariantCulture)
            };
            return log;
        }
    }
}