using FluentValidation;
using protocol.rabbitmq.shared.Helpers;
using protocol.rabbitmq.shared.Models.DTOs;

namespace protocol.rabbitmq.shared.Models.Validations
{
    public class ProtocolValidation : AbstractValidator<ProtocolDTO>
    {
        public ProtocolValidation()
        {
            RuleFor(x => x.NumProtocol)
                .NotEmpty().WithMessage("O campo Número do Protocolo é obrigatório.")
                .MaximumLength(50).WithMessage("O campo Número do Protocolo deve ter no máximo 50 caracteres.");

            RuleFor(x => x.NumViaDocumento)
                .NotEmpty().WithMessage("O campo Número da Via do Documento é obrigatório.");

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("O campo CPF é obrigatório.")
                .Length(11).WithMessage("O Tamanhao do campo CPF é de 11 caracteres.")
                .Must(ValidateCPF).WithMessage("CPF Inválido.");

            RuleFor(x => x.RG)
                .NotEmpty().WithMessage("O campo RG é obrigatório.")
                .MaximumLength(20).WithMessage("O campo RG deve ter no máximo 10 caracteres.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O campo Nome é obrigatório.")
                .MaximumLength(100).WithMessage("O campo Nome deve ter no máximo 100 caracteres.");

            RuleFor(x => x.NomeMae)
                .NotEmpty().WithMessage("O campo Nome da Mãe é obrigatório.")
                .MaximumLength(100).WithMessage("O campo Nome da Mãe deve ter no máximo 100 caracteres.");

            RuleFor(x => x.NomePai)
                .MaximumLength(100).WithMessage("O campo Nome do Pai deve ter no máximo 100 caracteres.");

            //RuleFor(x => x.Foto)
            //    .MaximumLength(100).WithMessage("O campo Foto deve ter no máximo 100 caracteres.");

        }

        private bool ValidateCPF(string cpf)
        {
            return Helper.Validation.ValidateCPF(cpf);
        }

    }

}
