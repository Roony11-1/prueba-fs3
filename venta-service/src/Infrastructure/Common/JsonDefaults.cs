using System.Text.Json;

namespace VentaService.Infrastructure.Common;

public static class JsonDefaults
{
    public static readonly JsonSerializerOptions CamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull 
    };
}