using System.Threading.Tasks;
using ReactDemo.Application.Dtos;

namespace ReactDemo.Application.Services
{
    public interface IUserAppService
    {
        Task<bool> UserSignInAsync (UserDto userDto);

        Task UserSignOutAsync();
    }
}