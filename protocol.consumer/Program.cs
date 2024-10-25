// Criar um provedor de serviços
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using protocol.rabbitmq.service;
using protocol.rabbitmq.shared.Interfaces;


var builder = WebApplication.CreateBuilder(args);
var app = builder.StartAppConsole("Consumer Protocol");

// Recuperando o serviço de Host
var service = app.Services.GetRequiredService<IServiceRabbitMQ>();
// Consome os dados
service.StartConsumer();
