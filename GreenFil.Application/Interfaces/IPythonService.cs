namespace GreenFil.Application.Interfaces;

public interface IPythonService
{
    Task<string> GenerateStlFromImage(string imagePath, int usuarioId, string nombreModelo);
    Task<(string glbPath, string stlPath)> GenerateStlFromText(string prompt, string outputName);
}