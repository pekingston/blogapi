using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateSlimBuilder(args);
var configuration = builder.Configuration;
builder.WebHost.UseKestrelCore();


builder.Services.ConfigureHttpJsonOptions(options =>
{
    //options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

//Configuramos los puertos de Kestrel para Grpc
//Aqui se deberían de configurar certificados, no lo vamos a hacer porque de esto se va a encargar el proxy manager que tengamos
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps();
    });
});

//GRPC
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configurar el Reflection de GRPC solo en depuración como medida de seguridad
if(app.Environment.IsDevelopment())
    app.MapGrpcReflectionService();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGrpcService<GrpcLoginService>();

app.MapGet("*", () => "Este webhook usa GRPC, no se admiten peticiones GET");


//app.UseHttpsRedirection();


app.Run();
