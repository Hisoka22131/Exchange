using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Exchange.Common;
using Exchange.Common.OperationResult;
using Exchange.Common.Resources;
using Exchange.Core.Options;
using Exchange.Domain.Interfaces;
using Exchange.Web.Blazor;
using Exchange.Web.Endpoints;
using Exchange.Web.MetaData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Exchange.Web.Extensions;

public static class WebServiceCollectionExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static void AddWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<IExchangeMetaData, ExchangeMetaDataHttpHeader>();
        
        services.AddBlazorServices(configuration);

        var allowedOrigins = configuration["Cors:AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];
        
        services.AddCors(options =>
        {
            options.AddPolicy(
                name: "AllowAll",
                configurePolicy: configurePolicy =>
                {
                    configurePolicy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddEndpoints();
        services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // services.AddHttpLoggingHandler();

        services.AddCustomSwaggerGen();
        services.AddAuthServices(configuration);
    }

    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.ContentType = "application/json";

                var response = new EmptyOperationResult();

                response.AddError(CommonErrors.CommonError("Internal error"));

                var jsonResponse = JsonSerializer.Serialize(response, JsonSerializerOptions);

                await context.Response.WriteAsync(jsonResponse);
            });
        });
    }

    private static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetRequiredSection(AuthOptions.SectionName);

        if (configurationSection is null)
            throw new ArgumentNullException(nameof(configurationSection));

        var authOptions = configurationSection.Get<AuthOptions>()!;

        services
            .AddOptions<AuthOptions>()
            .Bind(configurationSection);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey)),
                    ValidateLifetime = true
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy => { policy.RequireRole("Admin"); });
    }

    private static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        return services.AddSwaggerGen(q =>
        {
            q.SwaggerDoc("v1", new OpenApiInfo { Title = "Restaurant", Version = "v1" });
            q.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            q.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}