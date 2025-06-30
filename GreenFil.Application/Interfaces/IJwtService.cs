namespace GreenFil.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(int userId, string nombreUsuario, string email, string rol);
}