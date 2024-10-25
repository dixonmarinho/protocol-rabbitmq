using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace protocol.rabbitmq.shared.Models
{
    public class Protocol
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string NumProtocol { get; set; }
        public int NumViaDocumento { get; set; }
        [StringLength(11)]
        public string CPF { get; set; }
        [MaxLength(20)]
        public string RG { get; set; }
        [MaxLength(200)]
        public string Nome { get; set; }
        [MaxLength(200)]
        public string NomeMae { get; set; }
        [MaxLength(200)]
        public string NomePai { get; set; }
        public byte[]? Foto { get; set; }
    }
}
