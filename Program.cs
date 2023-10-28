using Azure;
using Azure.AI.OpenAI;
using IASquad.Poc.AzureOpenAi.Services;
using IASquad.Poc.AzureOpenAi.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// Register an Azure OpenAI Client
var azureOpenAIEndpoint = builder.Configuration.GetValue<string>("AzureOpenAi:Url");
var azureOpenAIKey = builder.Configuration.GetValue<string>("AzureOpenAi:Key");

builder.Services.AddScoped(client =>
    new OpenAIClient(new Uri(azureOpenAIEndpoint), new AzureKeyCredential(azureOpenAIKey)));

// Register Application Services
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
