using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.publisher.MockData
{
    public partial class MockProtocol
    {
        public static List<ProtocolDTO> MockList(int totalmockdata)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var foto = Path.Combine(currentDirectory, "Images", "foto.png");
            var fotoBytes = File.ReadAllBytes(foto);

            var protocolCollection = new List<ProtocolDTO>();
            for (int i = 0; i < totalmockdata; i++)
            {
                // Creare Unique Guid
                var guid = Guid.NewGuid();
                var protocol = new ProtocolDTO
                {
                    NumProtocol = guid.ToString(),
                    NumViaDocumento = 1,
                    CPF = "82792089334",
                    RG = "123456",
                    Nome = "NEY DIXON SANTIAGO MARINHO",
                    NomeMae = "JOSELY RIBEIRO SANTIAGO",
                    NomePai = "LUIS SANTOS MARINHO",
                    Foto = fotoBytes
                };
                protocolCollection.Add(protocol);
            }
            return protocolCollection;
        }
    }
}
