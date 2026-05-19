using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IRatingService
    {
        Task<List<RatingDto>> ObtenerPorReceta(int recetaId);
        Task<RatingDto?> Crear(RatingCreacionDto dto);
        Task<bool> Actualizar(int id, RatingModificacionDto dto);
        Task<bool> Eliminar(int id);
    }

    public class RatingService : IRatingService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Ratings";

        public RatingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<RatingDto>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var url = $"{endpoint}/receta/{recetaId}";
                var lista = await httpClient.GetFromJsonAsync<List<RatingDto>>(url);
                return lista ?? new List<RatingDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener ratings: {ex.Message}");
                return new List<RatingDto>();
            }
        }

        public async Task<RatingDto?> Crear(RatingCreacionDto dto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, dto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RatingDto>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear rating: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear rating: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Actualizar(int id, RatingModificacionDto dto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{endpoint}/{id}", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar rating {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar rating {id}: {ex.Message}");
                return false;
            }
        }
    }
}
