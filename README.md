protocol.rabbitmq
Descrição

Este projeto implementa um sistema de gerenciamento de protocolos utilizando RabbitMQ para processamento assíncrono de mensagens. Ele permite a publicação de protocolos e o consumo/processamento desses protocolos por meio de filas. O sistema é construído em .NET 7.0 e utiliza ASP.NET Core para a API, Entity Framework Core para acesso a dados e Swagger para documentação da API.

Funcionamento

O sistema funciona da seguinte forma:

Publicação de Protocolos: A API recebe solicitações para criar novos protocolos. Esses protocolos são então publicados em uma fila do RabbitMQ.

Consumo de Protocolos: Um consumidor (worker) fica ouvindo a fila do RabbitMQ. Quando um novo protocolo é publicado na fila, o consumidor o recebe e processa.

Processamento de Protocolos: O processamento do protocolo pode envolver diversas ações, como salvar o protocolo em um banco de dados, enviar notificações, etc.

Armazenamento de Logs: O sistema utiliza Serilog para registrar os eventos e erros, armazenando-os em um banco de dados PostgreSQL e em arquivos de log.

Linguagem de Programação

C#

Componentes Utilizados

.NET 7.0

ASP.NET Core

Entity Framework Core

RabbitMQ

PostgreSQL

Serilog

Swagger

AutoMapper

JWT (JSON Web Token)

Microsoft.AspNetCore.Identity

Docker

Módulos Principais

protocol.rabbitmq.api: Contém a API REST para interagir com o sistema de protocolos.

OBS: usuário : user / senha : 123456

protocol.rabbitmq.service: Contém a lógica de negócio para gerenciar os protocolos, incluindo a interação com o RabbitMQ.

protocol.rabbitmq.data: Contém a camada de acesso a dados, utilizando Entity Framework Core para interagir com o banco de dados PostgreSQL.

protocol.rabbitmq.shared: Contém modelos, DTOs e interfaces compartilhadas entre os outros módulos.

protocol.rabbitmq.test: Contém os testes unitários para o projeto.

protocol.consumer: Implementa o consumidor que processa as mensagens da fila do RabbitMQ.

protocol.publisher: Implementa um exemplo de publicador de mensagens para a fila do RabbitMQ.

Como Executar

Para executar o projeto, você precisará ter o .NET 7.0 SDK, o PostgreSQL, o RabbitMQ e o Docker instalados.

Clone o repositório do GitHub.

Execute um dos seguintes scripts no diretório raiz do projeto:

start-api: Inicia a API REST em um container Docker.

start-producer: Inicia o publicador de exemplo em um container Docker.

start-consumer: Inicia o consumidor em um container Docker.

Observações:

Os projetos são executados em containers Docker.

É necessário fazer modificações no arquivo appsettings.json de cada projeto para configurar o acesso ao banco de dados PostgreSQL e ao RabbitMQ, se necessário.

Script da Base de Dados:

O script para criar a base de dados está localizado na pasta DML. No entanto, se a aplicação API for executada com a conexão com o banco de dados configurada corretamente, a estrutura da base de dados será criada automaticamente, sem a necessidade de executar o script DML.

Aplicar Migrations (opcional):

Modifique o arquivo appsettings.json na pasta data para configurar a string de conexão com o banco de dados. Se for usar migrations, utilize a seguinte string de conexão em AppDataFactory:

optionsBuilder.UseNpgsql("Server=10.0.1.1;Port=5432;User Id=postgres;Password=123;Database=protocol;Timeout=30;CommandTimeout=120");
content_copy
Use code with caution.
C#

Acesse o projeto protocol.rabbitmq.data.

Execute o comando dotnet ef database update para aplicar as migrations e criar a base de dados.

Tratamento de Erros e Logs

O sistema utiliza Serilog para registrar os eventos e erros, armazenando-os em um banco de dados PostgreSQL e em arquivos de log. A configuração do Serilog é feita no módulo protocol.rabbitmq.service, na classe DependencyInject, método AddServiceLog.

Os logs são gravados em diferentes níveis (Debug, Information, Warning, Error, Fatal) e são direcionados para o console, arquivos de log e para o banco de dados PostgreSQL.

Tratamento de Erros na Aplicação:

A aplicação utiliza um middleware de tratamento de erros global para capturar e tratar exceções não tratadas. Este middleware registra o erro utilizando o Serilog e retorna uma resposta HTTP 500 (Internal Server Error) para o cliente, com uma mensagem genérica de erro.

Exemplo de código do middleware de tratamento de erros:

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            _logger.Error($"Erro na requisição: {contextFeature.Error}");

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = "Ocorreu um erro interno no servidor."
            }.ToString());
        }
    });
});
content_copy
Use code with caution.
C#
Controllers

As controllers são classes que definem as rotas e ações da API. Elas são responsáveis por receber as requisições HTTP, processá-las e retornar as respostas.

Documentação das Controllers:

As controllers são documentadas utilizando Swagger. O Swagger gera uma interface web que permite visualizar e testar as rotas e ações da API.

Exemplo de Controller:

[ApiController]
[Route("api/[controller]")]
public class ProtocolsController : ControllerBase
{
    private readonly IServiceProtocol _serviceProtocol;

    public ProtocolsController(IServiceProtocol serviceProtocol)
    {
        _serviceProtocol = serviceProtocol;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProtocol(ProtocolDTO protocolDTO)
    {
        var result = await _serviceProtocol.CreateProtocolAsync(protocolDTO);
        if (result.success)
        {
            return CreatedAtAction(nameof(GetProtocol), new { id = result.data.Id }, result.data);
        }
        return BadRequest(result.xmessage);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProtocol(int id)
    {
        var result = await _serviceProtocol.GetProtocolByIdAsync(id);
        if (result.success)
        {
            return Ok(result.data);
        }
        return NotFound(result.xmessage);
    }
}
content_copy
Use code with caution.
C#

Utilização das Controllers:

As controllers são utilizadas para definir as rotas e ações da API. Elas são acessadas através de requisições HTTP.

Exemplo de utilização da Controller ProtocolsController:

Criar um novo protocolo:

Método HTTP: POST

URL: /api/protocols

Corpo da requisição: JSON com os dados do protocolo

Obter um protocolo por ID:

Método HTTP: GET

URL: /api/protocols/{id}

Swagger:

As controllers são documentadas utilizando Swagger. O Swagger gera uma interface web que permite visualizar e testar as rotas e ações da API.

Clean Architecture e Endpoints:

Este projeto utiliza o padrão de arquitetura Clean Architecture. As controllers são consideradas endpoints e fazem parte da camada de apresentação. Elas são responsáveis por receber as requisições HTTP, processá-las e retornar as respostas.

Observação importante sobre o exemplo de Controller:

O exemplo de controller ProtocolsController apresentado acima serve apenas para ilustrar o conceito de controllers e como elas são utilizadas em uma API ASP.NET Core. No sistema real, foi utilizada uma abordagem diferente e mais simplificada para a implementação das controllers.


Observações

Este projeto é um exemplo de como implementar um sistema de gerenciamento de protocolos com RabbitMQ.

A implementação do processamento de protocolos pode ser customizada de acordo com as necessidades do seu sistema.

Certifique-se de ter as dependências necessárias instaladas antes de executar o projeto.