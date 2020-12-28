using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUserNameAsync(string userName);

        Task<MemberDto> GetMemberAsync(string userName);
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<IEnumerable<TradeLog>> GetMemberTradesAsync(string userName);
    }
}