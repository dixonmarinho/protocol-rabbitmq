using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace protocol.rabbitmq.shared.Models.DTOs
{
    public class ProtocolDTO 
    {
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
