using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Shared.Common;
using Shared.Swagger.Filters;
using Shared.Swagger.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Swagger;

public static class ServiceCollectionExtensions
{
    public const string HeaderName = "X-Api-Key";

    #region Swagger for Minimal APIS

    public static IServiceCollection AddSwaggerDocs(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("swagger");
        var options = section.BindOptions<SwaggerOptions>();
        services.Configure<SwaggerOptions>(section);
        
        if (!options.Enabled)
        {
            return services;
        }
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(swagger =>
        {
            swagger.SchemaFilter<ExcludePropertiesFilter>();
            swagger.EnableAnnotations();
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc(options.Version, new OpenApiInfo
            {
                Title = options.Title,
                Version = options.Version
            });
        });

        return services;
    }
    
    public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetRequiredService<IOptions<SwaggerOptions>>().Value;
        if (!options.Enabled)
        {
            return app;
        }
        
        app.UseSwagger();
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = string.IsNullOrWhiteSpace(options.Route) ? "swagger" : options.Route;
            reDoc.SpecUrl($"/swagger/{options.Version}/swagger.json");
            reDoc.DocumentTitle = options.Title;
        });

        return app;
    }

    #endregion
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="assembly"></param>
    /// <param name="swaggerSectionName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    [Obsolete("Use AddSwaggerDocs instead")]
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services,
        IConfiguration configuration,
        Assembly assembly, string swaggerSectionName = "SwaggerOptions")
    {
        services.AddEndpointsApiExplorer();
        services.AddVersionedApiExplorer(options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        services
            .AddOptions<SwaggerOptions>().Bind(configuration.GetSection(swaggerSectionName))
            .ValidateDataAnnotations();

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        services.AddSwaggerGen(
            options =>
            {
                // options.DescribeAllParametersInCamelCase();
                options.OperationFilter<SwaggerDefaultValues>();
                var xmlFile = XmlCommentsFilePath(assembly);
                if (File.Exists(xmlFile)) options.IncludeXmlComments(xmlFile);
                
                options.EnableAnnotations();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                              Enter 'Bearer' [space] and then your token in the text input below.
                              \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityDefinition(HeaderName,
                    new OpenApiSecurityScheme
                    {
                        Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                        In = ParameterLocation.Header,
                        Name = HeaderName,
                        Type = SecuritySchemeType.ApiKey
                    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = HeaderName,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = HeaderName}
                        },
                        new string[] { }
                    }
                });

                //https://rimdev.io/swagger-grouping-with-controller-name-fallback-using-swashbuckle-aspnetcore/
                options.TagActionsBy(api =>
                {
                    if (api.GroupName != null) return new[] {api.GroupName};

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                        return new[] {controllerActionDescriptor.ControllerName};

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });

                options.DocInclusionPredicate((name, api) => true);
            });

        


        return services;

        static string XmlCommentsFilePath(Assembly assembly)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var fileName = assembly.GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }

    [Obsolete("Use UseSwaggerDocs instead")]
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app,
        IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
            });

        return app;
    }
}