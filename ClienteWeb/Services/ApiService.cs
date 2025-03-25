using ClienteWeb.Models.ViewModels;
using System.Text;
using System.Text.Json;

namespace ClienteWeb.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7243/api/Apartamento";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ApartamentoViewModel>> GetApartamentosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ApartamentoViewModel>>("");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetApartamentosAsync: {ex.Message}");
                return new List<ApartamentoViewModel>();
            }
        }

        // 2. Obtener un apartamento por ID
        public async Task<ApartamentoViewModel?> GetApartamentoByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ApartamentoViewModel>($"{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetApartamentoByIdAsync({id}): {ex.Message}");
                return null;
            }
        }

        // 3. Crear un nuevo apartamento
        public async Task<bool> CreateApartamentoAsync(ApartamentoViewModel model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("", model);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateApartamentoAsync: {ex.Message}");
                return false;
            }
        }

        // 4. Actualizar un apartamento
        public async Task<bool> UpdateApartamentoAsync(int id, ApartamentoViewModel model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{id}", model);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateApartamentoAsync({id}): {ex.Message}");
                return false;
            }
        }

        // 5. Eliminar un apartamento
        public async Task<bool> DeleteApartamentoAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteApartamentoAsync({id}): {ex.Message}");
                return false;
            }
        }

        //Autenticar por usuario
        public async Task<ApartamentoViewModel?> GetApartamentoPorUsuario(string usuarioResponsable)
        {
            return await _httpClient.GetFromJsonAsync<ApartamentoViewModel>($"usuario/{usuarioResponsable}");
        }

    }
}
