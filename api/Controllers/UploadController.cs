using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UploadController : BaseApiController
    {
        public IUserRepository _userRepository { get; }
        public IMapper _mapper { get; }
        private readonly DataContext _context;

        private readonly ITradeLogService _tradeLogService;

        public UploadController(IUserRepository userRepository, IMapper mapper, ITradeLogService tradeLogService,
            DataContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tradeLogService = tradeLogService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ICollection<TradeLog>>> AddTrades(IFormFile file)
        {
            if (file == null) return BadRequest("Failed to import trades");

            string username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AppUser user = await _userRepository.GetUserByUserNameAsync(username);

            ICollection<TradeLog> result = await _tradeLogService.AddTradesAsync(file);


            if (result?.Count > 0)
            {
                user.TradeLogs = result;
                _context.Update(user);
                await _context.SaveChangesAsync();

                return Ok(result);
            }

            return BadRequest("Failed to import trades");
        }
    }
}