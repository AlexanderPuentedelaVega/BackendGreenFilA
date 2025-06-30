using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;

namespace GreenFil.Infrastructure.Services;

public class UsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public UsuarioService(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        return await _unitOfWork.Usuarios.GetAllAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _unitOfWork.Usuarios.GetByIdAsync(id);
    }

    public async Task<UsuarioRespuestaDTO> CrearUsuarioAsync(Usuario usuario)
    {
        usuario.Fecharegistro = DateTime.UtcNow;
        await _unitOfWork.Usuarios.AddAsync(usuario);
        await _unitOfWork.CommitAsync();

        var token = _jwtService.GenerateToken(usuario.Id, usuario.Nombreusuario, usuario.Email, usuario.Rol ?? "usuario");

        return new UsuarioRespuestaDTO
        {
            NombreUsuario = usuario.Nombreusuario,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Token = token
        };
    }

    public async Task<bool> ActualizarUsuarioAsync(int id, Usuario updated)
    {
        var existente = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (existente == null) return false;

        existente.Nombreusuario = updated.Nombreusuario;
        existente.Email = updated.Email;
        existente.Passwordhash = updated.Passwordhash;
        existente.Rol = updated.Rol;
        existente.Puntos = updated.Puntos;

        _unitOfWork.Usuarios.Update(existente);
        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<bool> EliminarUsuarioAsync(int id)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
        if (usuario == null) return false;

        _unitOfWork.Usuarios.Remove(usuario);
        await _unitOfWork.CommitAsync();
        return true;
    }
}