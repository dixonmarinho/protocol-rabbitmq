// Criar um provedor de serviços
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using protocol.publisher.MockData;
using protocol.rabbitmq.service;
using protocol.rabbitmq.shared.Interfaces;


var builder = WebApplication.CreateBuilder(args);
var app = builder.StartAppConsole("Publish Protocol");
var service = app.Services.GetRequiredService<IServiceRabbitMQ>();

var totalMockData = 10; // Insira o total de dados mockados
await service.PublishAsync(MockProtocol.MockList(totalMockData));