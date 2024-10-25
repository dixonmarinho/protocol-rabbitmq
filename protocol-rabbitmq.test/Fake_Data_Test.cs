using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.rabbitmq.test
{
    public static class Fake_Data_Test
    {
        public static List<ProtocolDTO> CreateSingleValidProtocolList()
        {
            var protocolList = new List<ProtocolDTO>();
            var protocol = new ProtocolDTO
            {
                NumProtocol = "123",
                NumViaDocumento = 1,
                CPF = "82792089334",
                RG = "123",
                Nome = "123",
                NomeMae = "123",
                NomePai = "123",
                Foto = null
            };
            protocolList.Add(protocol);
            return protocolList;
        }

        public static List<ProtocolDTO> GetInvalidProtocolList_MissingCPF()
        {
            var protocols = new List<ProtocolDTO>();
            protocols.Add(new ProtocolDTO
            {
                NumProtocol = "456",
                NumViaDocumento = 2,
                RG = "456",
                Nome = "Nome Válido",
                NomeMae = "Nome da Mãe",
                NomePai = "Nome do Pai",
                Foto = null
            });
            return protocols;
        }

    }
}
