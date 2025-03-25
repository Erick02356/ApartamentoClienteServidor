using ApiApartamentos.DTOs;
using ClienteEscritorio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClienteEscritorio.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7243/api/Apartamento";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        //public async Task<List<ApartamentoViewModel>> GetApartamentosAsync()
        //    => await _httpClient.GetFromJsonAsync<List<ApartamentoViewModel>>(_apiUrl) ?? new List<ApartamentoViewModel>();
        public async Task<List<ApartamentoFullDTO>> GetApartamentosWinFormsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ApartamentoFullDTO>>($"{_apiUrl}/cliente/winforms");
        }


        public async Task<ApartamentoViewModel> GetApartamentoByIdAsync(int id)
            => await _httpClient.GetFromJsonAsync<ApartamentoViewModel>($"{_apiUrl}/{id}");

        public async Task<bool> CreateApartamentoAsync(ApartamentoViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(_apiUrl, model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateApartamentoAsync(int id, ApartamentoViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteApartamentoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
