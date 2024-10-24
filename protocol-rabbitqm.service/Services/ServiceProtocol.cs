using protocol.rabbitqm.data.Data;
using protocol.rabbitqm.shared;
using protocol.rabbitqm.shared.Interfaces;
using protocol.rabbitqm.shared.Models;
using protocol.rabbitqm.shared.Models.DTOs;

namespace protocol.rabbitqm.service.Services
{
    public class ServiceProtocol : IServiceProtocol
    {
        private readonly IUnitOfWork<AppDataContext> context;

        public ServiceProtocol(IUnitOfWork<AppDataContext> context)
        {
            this.context = context;
        }

        public async Task<Result<bool>> AddProtocolAsync(ProtocolDTO protocol)
        {
            var dataset = context.GetRepository<Protocol>();
            var response = await dataset.AddAsync(protocol);
            return Result<bool>.Success();
        }
    }
}
