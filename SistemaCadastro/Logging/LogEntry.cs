using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCadastro.Logging
{
    [Table("ApiLogs")]
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string? LogLevel { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Category { get; set; }

        public string? Message { get; set; }

        public string? Exception { get; set; }

        public string? Path { get; set; }

        public string? Method { get; set; }

        public int? StatusCode { get; set; }
    }
}
