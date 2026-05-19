using System.ComponentModel.DataAnnotations;

namespace RecetArreWeb.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int RecetaId { get; set; }
        public string UsuarioId { get; set; } = default!;
        public UsuarioResumenDto? Usuario { get; set; }
        public double Valor { get; set; }
        public DateTime CreadoUtc { get; set; }
    }

    public class RatingCreacionDto
    {
        [Required]
        public int RecetaId { get; set; }

        [Required]
        [Range(0.5, 5.0)]
        public double Valor { get; set; }
    }

    public class RatingModificacionDto
    {
        [Required]
        [Range(0.5, 5.0)]
        public double Valor { get; set; }
    }
}
