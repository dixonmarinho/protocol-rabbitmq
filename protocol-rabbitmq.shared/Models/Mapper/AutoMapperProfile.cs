using AutoMapper;
using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.rabbitmq.shared.Models.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Protocol, ProtocolDTO>().ReverseMap();
        }
    }
}
