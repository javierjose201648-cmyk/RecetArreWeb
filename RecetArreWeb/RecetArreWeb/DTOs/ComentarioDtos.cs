using System.ComponentModel.DataAnnotations;

namespace RecetArreWeb.DTOs
{
    public class UsuarioResumenDto
    {
        public string Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
    }

    public class ComentarioDto
    {
        public int Id { get; set; }
        public string Contenido { get; set; } = default!;
        public DateTime CreadoUtc { get; set; }
        public int RecetaId { get; set; }
        public string UsuarioId { get; set; } = default!;
        public UsuarioResumenDto? Usuario { get; set; }
        public int? ParentId { get; set; }
        public int RepliesCount { get; set; }
        public bool IsEdited { get; set; }
        // Optional list used by tree endpoint
        public List<ComentarioDto>? Replies { get; set; }
    }

    public class ComentarioCreacionDto
    {
        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "El comentario debe tener entre 1 y 1000 caracteres")]
        public string Contenido { get; set; } = default!;

        // When creating via /api/recetas/{recetaId}/comentarios the recetaId is provided in route,
        // but backend expects RecetaId in the body for POST /api/Comentarios. Include it here.
        [Required]
        public int RecetaId { get; set; }

        // Keep ParentId here to support replies.
        public int? ParentId { get; set; }
    }

    public class ComentarioModificacionDto
    {
        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "El comentario debe tener entre 1 y 1000 caracteres")]
        public string Contenido { get; set; } = default!;

        // Optional row version / concurrency token (base64) if backend returns one
        public string? RowVersion { get; set; }
    }

    public class PaginatedComentariosDto
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ComentarioDto> Items { get; set; } = new();
    }
}
