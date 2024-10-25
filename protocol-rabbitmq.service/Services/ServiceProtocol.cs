using AutoMapper;
using FluentValidation.Results;
using protocol.rabbitmq.data.Data;
using protocol.rabbitmq.shared;
using protocol.rabbitmq.shared.Interfaces;
using protocol.rabbitmq.shared.Models;
using protocol.rabbitmq.shared.Models.DTOs;
using protocol.rabbitmq.shared.Models.Validations;

namespace protocol.rabbitmq.service.Services
{
    public class ServiceProtocol : IServiceProtocol
    {
        private readonly IUnitOfWork<AppDataContext> context;
        private readonly IMapper map;
        private readonly IServiceLog log;
        private readonly IRepository<Protocol> repository;

        public ServiceProtocol(IUnitOfWork<AppDataContext> context, IMapper map, IServiceLog log)
        {
            this.context = context;
            this.map = map;
            this.log = log;
            this.repository = context.GetRepository<Protocol>();
        }

        /// <summary>
        /// Busca a Proxima Via = 1 = Via do Cliente, 2,3, 4, 5, 6, 7, 8, 9, 10... via (Perda, Extravio ou Roubo)
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetNextProtocolSequenceNumberAsync(string cpf)
        {
            var responseCollection = await repository.ListAsync(x => x.CPF == cpf);
            var response = responseCollection.data.FirstOrDefault();
            if (response == null)
                return 1;
            else
                return response.NumViaDocumento + 1;
        }

        public async Task<bool> IsProtocolRegisteredAsync(string cpf, string protocol)
        {
            var responseCollection = await repository.ListAsync(x => x.CPF == cpf & x.NumProtocol == protocol);
            if (responseCollection.data == null)
                return false;
            var response = responseCollection.data.FirstOrDefault();
            return (response != null);
        }

        public async Task<Result<bool>> AddProtocolAsync(ProtocolDTO? protocol)
        {
            if (protocol == null)
                return await Result<bool>.FailAsync("Protocolo Vazio");
            try
            {
                var protocolExist = await IsProtocolRegisteredAsync(protocol.CPF, protocol.NumProtocol);
                if (protocolExist == true)
                {
                    var msgErrro = $"Protocolo Existente : {protocol.NumProtocol} Via Nº {protocol.NumViaDocumento}, para o CPF {protocol.CPF}.";
                    log.Error(msgErrro);
                    return await Result<bool>.FailAsync(msgErrro);
                }

                // Processa os Protocolos inexistentes
                protocol.NumViaDocumento = await GetNextProtocolSequenceNumberAsync(protocol.CPF);
                // Valida os Dados
                var validationProtocol = new ProtocolValidation();
                var responseValidation = validationProtocol.Validate(protocol);
                if (responseValidation.IsValid == false)
                {
                    var errorCollection = responseValidation.Errors.Select(e => e.ErrorMessage).ToList();
                    var messageError = "";
                    foreach (var error in errorCollection)
                    {
                        messageError += error + " - ";
                    }
                    messageError = $"Protocolo Invalido : {messageError}";
                    log.Info(messageError);
                    return await Result<bool>.FailAsync(messageError);
                }

                var protocolMap = map.Map<Protocol>(protocol);
                var response = await repository.AddAsync(protocolMap);
                if (response.success == true)
                {
                    return Result<bool>.Success($"Protocolo {protocol.NumProtocol} Via Nº {protocol.NumViaDocumento}, para o CPF {protocol.CPF} Incluido com Sucesso");
                }
                else
                {
                    return Result<bool>.Success($"Falha ao Adicionar Protocolo {protocol.NumProtocol} Via Nº {protocol.NumViaDocumento}, para o CPF {protocol.CPF}.");
                }
            }
            catch (Exception e)
            {
                log.Error(e);
                return await Result<bool>.FailAsync(e.Message);
            }
        }


        private ValidationResult ValidateProtocol(ProtocolDTO protocol)
        {
            var validationProtocol = new ProtocolValidation();
            return validationProtocol.Validate(protocol);
        }


        public async Task<Result<List<bool>>> AddProtocolAsync(List<ProtocolDTO>? protocols)
        {
            if (protocols == null || !protocols.Any())
                return await Result<List<bool>>.FailAsync("Lista de Protocolos Vazia");

            var responseCollection = new List<Result<bool>>();
            var protocolsToAdd = new List<ProtocolDTO>();

            foreach (var protocol in protocols)
            {
                if (protocol == null)
                {
                    responseCollection.Add(await Result<bool>.FailAsync("Protocolo Vazio"));
                    continue; // Ignora protocolos vazios
                }

                var protocolExist = await IsProtocolRegisteredAsync(protocol.CPF, protocol.NumProtocol);
                if (protocolExist == false)
                {
                    // Define o próximo número da via do documento
                    protocol.NumViaDocumento = await GetNextProtocolSequenceNumberAsync(protocol.CPF);
                    protocolsToAdd.Add(protocol);
                }
                else
                {
                    var msgErro = $"Protocolo Existente: {protocol.NumProtocol} Via Nº {protocol.NumViaDocumento}, para o CPF {protocol.CPF}.";
                    var responseError = await Result<bool>.FailAsync(msgErro);
                    responseCollection.Add(responseError);
                    log.Error(msgErro);
                    continue; // Ignora protocolo existente
                }

                // Valida os Dados
                var responseValidation = ValidateProtocol(protocol);
                if (responseValidation.IsValid)
                {
                    var errorCollection = responseValidation.Errors.Select(e => e.ErrorMessage).ToList();
                    var messageError = string.Join(" - ", errorCollection);
                    messageError = $"Protocolo Inválido: {messageError}";
                    log.Info(messageError);
                    responseCollection.Add(await Result<bool>.FailAsync(messageError));
                    continue; // Ignora protocolo inválido
                }

                // Mapeia o protocolo para a entidade de banco de dados
                protocolsToAdd.Add(protocol); // Adiciona o protocolo mapeado para a lista de protocolos a serem adicionados
            }

            // Adiciona todos os protocolos em lote
            if (protocolsToAdd.Any())
            {
                var protocolsAddMap = map.Map<List<Protocol>>(protocolsToAdd);
                var response = await repository.AddBatchAsync(protocolsAddMap);
                if (response.success)
                {
                    // Adiciona mensagens de sucesso para cada protocolo
                    foreach (var protocol in protocolsAddMap)
                    {
                        responseCollection.Add(Result<bool>.Success($"Protocolo {protocol.NumProtocol} Via Nº {protocol.NumViaDocumento}, para o CPF {protocol.CPF} incluído com sucesso."));
                    }
                }
                else
                {
                    foreach (var protocol in protocolsAddMap)
                    {
                        responseCollection.Add(Result<bool>.Fail($"Falha ao adicionar Protocolo {protocol.NumProtocol} Via Nº {protocol.NumViaDocumento}, para o CPF {protocol.CPF}."));
                    }
                }
            }
            return Result<List<bool>>.Success(responseCollection.Select(r => r.success).ToList());
        }



        public async Task<Result<List<ProtocolDTO>>> SearchProtocolsAsync(string document)
        {
            if (string.IsNullOrEmpty(document))
                return Result<List<ProtocolDTO>>.Fail("Informe o Protocolo, CPF ou RG");

            var response = await repository.ListAsync(x => x.CPF == document || x.NumProtocol == document || x.RG == document);
            if (response.success == true)
            {
                if (response.data.Count > 0)
                    return Result<List<ProtocolDTO>>.Success(map.Map<List<ProtocolDTO>>(response.data));
                else
                    return Result<List<ProtocolDTO>>.Fail("Nenhum protocolo existente para este documento");
            }
            else
                return Result<List<ProtocolDTO>>.Fail("Nenhum protocolo existente para este documento");

        }
    }
}
