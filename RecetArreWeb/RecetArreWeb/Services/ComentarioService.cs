using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IComentarioService
    {
        Task<PaginatedComentariosDto> ObtenerPorReceta(int recetaId, int page = 1, int pageSize = 20);
        Task<PaginatedComentariosDto> ObtenerArbolPorReceta(int recetaId, int page = 1, int pageSize = 50);
        Task<ComentarioDto?> Crear(ComentarioCreacionDto comentarioDto);
        Task<ComentarioDto?> Crear(int recetaId, ComentarioCreacionDto comentarioDto);
        Task<bool> Actualizar(int id, ComentarioModificacionDto comentarioDto);
        Task<bool> Eliminar(int id);
    }

    public class ComentarioService : IComentarioService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Comentarios";

        public ComentarioService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<PaginatedComentariosDto> ObtenerPorReceta(int recetaId, int page = 1, int pageSize = 20)
        {
            try
            {
                // The API returns an array of comments for the receta: GET /api/Comentarios/receta/{recetaId}
                var url = $"{endpoint}/receta/{recetaId}";
                var lista = await httpClient.GetFromJsonAsync<List<ComentarioDto>>(url);
                return new PaginatedComentariosDto
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = lista?.Count ?? 0,
                    Items = lista ?? new List<ComentarioDto>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios: {ex.Message}");
                return new PaginatedComentariosDto { Page = page, PageSize = pageSize };
            }
        }

        public async Task<PaginatedComentariosDto> ObtenerArbolPorReceta(int recetaId, int page = 1, int pageSize = 50)
        {
            try
            {
                // The provided API spec does not expose a /tree endpoint. Fallback to the same receta route.
                var url = $"{endpoint}/receta/{recetaId}";
                var lista = await httpClient.GetFromJsonAsync<List<ComentarioDto>>(url);
                return new PaginatedComentariosDto
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = lista?.Count ?? 0,
                    Items = lista ?? new List<ComentarioDto>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener arbol de comentarios: {ex.Message}");
                return new PaginatedComentariosDto { Page = page, PageSize = pageSize };
            }
        }

        public async Task<ComentarioDto?> Crear(ComentarioCreacionDto comentarioDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, comentarioDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ComentarioDto>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear comentario: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}");
                return null;
            }
        }

        public async Task<ComentarioDto?> Crear(int recetaId, ComentarioCreacionDto comentarioDto)
        {
            try
            {
                // The API expects POST to /api/Comentarios with recetaId in the request body.
                comentarioDto.RecetaId = recetaId;
                var response = await httpClient.PostAsJsonAsync(endpoint, comentarioDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ComentarioDto>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear comentario: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Actualizar(int id, ComentarioModificacionDto comentarioDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{endpoint}/{id}", comentarioDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar comentario {id}: {ex.Message}");
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
                Console.WriteLine($"Error al eliminar comentario {id}: {ex.Message}");
                return false;
            }
        }
    }
}
