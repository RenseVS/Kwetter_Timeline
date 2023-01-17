using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Serilog;
using StackExchange.Redis;
using Timeline_Service.Services;
using Timeline_Service.Services.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("RedisConnection")));
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddScoped<TimelineService>();
builder.Services.AddScoped<CelebertyService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth0:Domain"];
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TweetMadeWithFollowerConsumer>();
    x.AddConsumer<UserCreatedConsumer>();

    x.UsingRabbitMq((cxt, cfg) =>
    {
        cfg.Host(new Uri("amqps://ckgicxje:3GxKLqpiE0FSyqfHALy-ch1iYx5TeJ0E@kangaroo.rmq.cloudamqp.com/ckgicxje"), h =>
        {
            h.Username("ckgicxje");
            h.Password("3GxKLqpiE0FSyqfHALy-ch1iYx5TeJ0E");
        });

        cfg.ReceiveEndpoint("Timeline_Service", c =>
        {
            c.ConfigureConsumer<TweetMadeWithFollowerConsumer>(cxt);
            c.ConfigureConsumer<UserCreatedConsumer>(cxt);
        });
    });
});

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341"));

/*builder.Services.AddAuthorization(o =>
{
    o.FallbackPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

    o.AddPolicy("moderator", policy =>
      policy.RequireClaim("permissions", "moderator"));
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }