using SaludPaciente.Models;

namespace SaludPaciente.Services
{
    public interface IAPIService
    {
        Task<List<Paciente>> GetPacientes();
        Task<Paciente> GetPaciente(int IdPaciente);
        Task<Paciente> PostPaciente(Paciente Paciente);

        Task<Paciente> PutPaciente(int IdPaciente, Paciente Paciente);
        Task<string> DeletePaciente(int IdProdcuto);

    }
}
