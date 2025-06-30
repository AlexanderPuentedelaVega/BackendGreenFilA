using GreenFil.Application.DTOs;

namespace GreenFil.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResultDTO?> LoginAsync(LoginDTO dto);
}