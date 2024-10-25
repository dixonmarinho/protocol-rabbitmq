namespace protocol.rabbitmq.shared.Helpers
{
    public static partial class Helper
    {
        public static class Validation
        {
            public static bool ValidateCPF(string cpf)
            {
                // Remove caracteres não numéricos
                cpf = new string(cpf.Where(char.IsDigit).ToArray());

                // Verifica se o CPF tem 11 dígitos
                if (cpf.Length != 11)
                    return false;

                // Verifica se todos os dígitos são iguais
                if (cpf.Distinct().Count() == 1)
                    return false;

                // Calcula o primeiro dígito verificador
                int soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(cpf[i].ToString()) * (10 - i);
                int resto = soma % 11;
                int primeiroDigitoVerificador = (resto < 2) ? 0 : 11 - resto;

                // Calcula o segundo dígito verificador
                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(cpf[i].ToString()) * (11 - i);
                resto = soma % 11;
                int segundoDigitoVerificador = (resto < 2) ? 0 : 11 - resto;

                // Verifica se os dígitos verificadores calculados correspondem aos dígitos do CPF
                return int.Parse(cpf[9].ToString()) == primeiroDigitoVerificador &&
                       int.Parse(cpf[10].ToString()) == segundoDigitoVerificador;
            }
        }
    }
}
