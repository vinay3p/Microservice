using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static MassTransit.Transports.InMemory.Topology.Builders.PublishEndpointTopologyBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(o =>
//{
//    o.Events = new JwtBearerEvents
//    {
//        OnMessageReceived = ValidateJwtToken,
//    };
//});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key)
    };
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq();
});

builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Bank Api", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(swgr => {
    swgr.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
});
app.MapControllers();

app.Run();


Task ValidateJwtToken(MessageReceivedContext context)
{
    context.Token = GetTokenFromHeader(context.Request.Headers);

    var tokenHandler = new JwtSecurityTokenHandler();
    //var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]);
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    tokenHandler.ValidateToken(context.Token, new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
        ClockSkew = TimeSpan.Zero
    }, out SecurityToken validatedToken);

    var jwtToken = (JwtSecurityToken)validatedToken;
    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

    // attach user to context on successful jwt validation
    //context.Items["User"] = userService.GetById(userId);
    return Task.CompletedTask;
}

static string GetTokenFromHeader(IHeaderDictionary requestHeaders)
{
    if (!requestHeaders.TryGetValue("Authorization", out var authorizationHeader))
        throw new InvalidOperationException("Authorization token does not exists");

    var authorization = authorizationHeader.FirstOrDefault()!.Split(" ");

    var type = authorization[0];

    if (type != "Bearer") throw new InvalidOperationException("You should provide a Bearer token");

    var value = authorization[1] ?? throw new InvalidOperationException("Authorization token does not exists");
    return value;
}
