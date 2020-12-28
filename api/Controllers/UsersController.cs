using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        public IUserRepository _userRepository { get; }
        private readonly ITradeLogService _tradeLogService;

        public UsersController(IUserRepository userRepository, ITradeLogService tradeLogService)
        {
            _userRepository = userRepository;
            _tradeLogService = tradeLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            IEnumerable<MemberDto> members = await _userRepository.GetMembersAsync();
            return Ok(members);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string userName)
        {
            return await _userRepository.GetMemberAsync(userName);
        }

        [HttpGet("trades/{username}")]
        public async Task<ActionResult<IEnumerable<TradeLog>>> GetTrades(string userName)
        {
            IEnumerable<TradeLog> trades = await _userRepository.GetMemberTradesAsync(userName);
            return Ok(trades);
        }

        [HttpGet("profiles/{username}")]
        public async Task<ActionResult<TraderDto>> GetTraderProfile(string userName)
        {
            IEnumerable<TradeLog> trades = await _userRepository.GetMemberTradesAsync(userName);
            return Ok(_tradeLogService.CreateTraderProfile(trades));
        }
    }
}