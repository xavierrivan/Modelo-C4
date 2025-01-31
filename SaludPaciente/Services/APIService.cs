using SaludPaciente.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using SaludPaciente.Services;


namespace SaludPaciente.Services
{
    public class APIService : IAPIService
    {

        private static string _baseUrl;

        public APIService() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            _baseUrl = builder.GetSection("ApiSettings:BaseUrl").Value;
           
        }
        public async Task<string> DeletePaciente(int IdPaciente)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrl);
            var response = await httpClient.DeleteAsync($"Paciente/{IdPaciente}");

            if (response.IsSuccessStatusCode)
            {
                return "Paciente eliminado correctamente";
            }
            else
            {
                // Manejar el error aquí si es necesario
                throw new HttpRequestException($"Error al eliminar el Paciente. Código de estado: {response.StatusCode}");
            }
        }

        public Task<Paciente> GetPaciente(int IdPaciente)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Paciente>> GetPacientes()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri( _baseUrl );
            var response = await httpClient.GetAsync( "Paciente/");
            var json_response  = await response.Content.ReadAsStringAsync();
            List<Paciente> Pacientes = JsonConvert.DeserializeObject<List<Paciente>>(json_response);
            return Pacientes;

        }

        public async Task<Paciente> PostPaciente(Paciente Paciente)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrl);
            var json = JsonConvert.SerializeObject(Paciente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Paciente", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Paciente createdPaciente = JsonConvert.DeserializeObject<Paciente>(jsonResponse);
                return createdPaciente;
            }
            else
            {
                // Manejar el error aquí si es necesario
                throw new HttpRequestException($"Error al crear el Paciente. Código de estado: {response.StatusCode}");
            }
        }

        public async Task<Paciente> PutPaciente(int IdPaciente, Paciente Paciente)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseUrl);
            var json = JsonConvert.SerializeObject(Paciente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"Paciente/{IdPaciente}", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Paciente updatedPaciente = JsonConvert.DeserializeObject<Paciente>(jsonResponse);
                return updatedPaciente;
            }
            else
            {
                // Manejar el error aquí si es necesario
                throw new HttpRequestException($"Error al actualizar el Paciente. Código de estado: {response.StatusCode}");
            }
        }
    }
}
