using AspTree.DTO;
using AspTree.Exceptions;
using AspTree.Model;
using AspTree.Model.ErrorTracking;
using AspTree.Services;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DataTreeContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TreeDB")
    )
);

builder.Services.AddDbContext<ErrorJournalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TreeDB")
    )
);

builder.Services.AddScoped<DataNodeService>();
builder.Services.AddScoped<ErrorJournalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler(application =>
{
    application.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = Text.Plain;


        var exceptionHandlerPath = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPath?.Error ?? new Exception("Error of unkonwn origin!!!");


        var journal = context.RequestServices.GetRequiredService<ErrorJournalService>();

        var urlParameters = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";

        context.Request.Body.Position = 0;
        var bodyParameters = await new StreamReader(context.Request.Body).ReadToEndAsync();


        var journalRecord = await journal.CreateFromException(exception, urlParameters, bodyParameters);

        var response = exceptionHandlerPath?.Error is SecureException se
            ? new ErrorResponse(ErrorResponse.ErrorType.Secure, journalRecord.EventId, se.Message)
            : new ErrorResponse(ErrorResponse.ErrorType.Exception, journalRecord.EventId, $"Internal server error ID = {journalRecord.EventId}");

        var responseStr = JsonSerializer.Serialize(response, new JsonSerializerOptions() { WriteIndented = true });
        await context.Response.WriteAsync(responseStr);
    });
});


app.MapControllers();

app.Run();
