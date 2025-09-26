using GestionOrdenes.DTOs;

namespace GestionOrdenes.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> ValidateUserAsync(string username, string password);
        string GenerateJwtToken(string username, string role);
    }
}