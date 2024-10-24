namespace protocol.rabbitqm.shared.Models
{
    public class Protocol
    {
        public string Id { get; set; }
        public string NumProtocol { get; set; }
        public string NumViaDocumento { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Nome { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public byte[]? Foto { get; set; }
    }
}
