using Domus.Api.Helpers;
using Microsoft.OpenApi.Models;

namespace Domus.Api.Settings;

public class SwaggerSettings 
{
 	public string Version { get; set; } 
    public string Title { get; set; } 
    public string Description { get; set; } 
    public string TermsOfServiceUrl { get; set; } 
    public string ContactName { get; set; } 
    public string ContactEmail { get; set; } 
    public string ContactUrl { get; set; } 
    public string LicenseName { get; set; } 
    public string LicenseUrl { get; set; } 
    public SwaggerOptions Options { get; set; } = new();

    public OpenApiContact GetContact()
    {
        return new OpenApiContact
        {
            Name = ContactName,
            Url = new Uri(ContactUrl),
            Email = ContactEmail
        };
    }

    public OpenApiLicense GetLicense()
    {
        return new OpenApiLicense
        {
            Name = LicenseName,
            Url = new Uri(LicenseUrl)
        };
    }

    public Uri GetTermsOfService()
    {
        return new Uri(TermsOfServiceUrl);
    }

    public List<OpenApiServer> GetServers()
    {
        return Options.Servers.Where(s => !string.IsNullOrEmpty(s.Url)).Select(s => new OpenApiServer
        {
            Url = s.Url,
            Description = s.Description,
            Variables = s.Variables.Any() 
                ? s.Variables.ToDictionary(
                    v => v.Name,
                    v => new OpenApiServerVariable()
                    {
                        Description = v.Description,
                        Default = v.DefaultValue
                    }) 
                : new Dictionary<string, OpenApiServerVariable>()
            
        })
        .ToList();
    }

    public OpenApiSecurityScheme GetSecurityScheme()
    {
        var securityScheme = Options.SecurityScheme;
        return new OpenApiSecurityScheme
        {
            Name = securityScheme.Name,
            Description = securityScheme.Description,
            Type = securityScheme.GetSecuritySchemeType(),
            In = securityScheme.GetParameterLocation()
        };
    }

    public OpenApiSecurityRequirement GetSecurityRequirement()
    {
        var securityRequirement = Options.SecurityRequirement;
        return new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = securityRequirement.GetReferenceType(),
                        Id = securityRequirement.Id
                    }
                },
                Array.Empty<string>()
            }
        };
    }
}

public class SwaggerOptions
{
    public List<SwaggerServer> Servers { get; set; } = new();
    public SwaggerSecurityScheme SecurityScheme { get; set; }
    public SwaggerSecurityRequirement SecurityRequirement { get; set; }
}

public class SwaggerServer
{
    public string Url { get; set; } 
    public string Description { get; set; } 
    public List<SwaggerServerVariable> Variables { get; set; } = new();
}

public class SwaggerServerVariable
{
    public string Name { get; set; } 
    public string Description { get; set; } 
    public string DefaultValue { get; set; } 
}

public class SwaggerSecurityScheme
{
    public string Name { get; set; } 
    public string Description { get; set; } 
    public string Type { get; set; } 
    public string Location { get; set; } 

    public SecuritySchemeType GetSecuritySchemeType()
    {
        return EnumHelper.GetEnumValueFromString<SecuritySchemeType>(Type);
    }

    public ParameterLocation GetParameterLocation()
    {
        return EnumHelper.GetEnumValueFromString<ParameterLocation>(Location);
    }
}

public class SwaggerSecurityRequirement
{
    public string Type { get; set; } 
    public string Id { get; set; } 

    public ReferenceType GetReferenceType()
    {
        return EnumHelper.GetEnumValueFromString<ReferenceType>(Type);
    }
}
