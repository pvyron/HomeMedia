﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace HomeMedia.ExternalApi;

public static class ServiceInstallers
{
    public static IServiceCollection AddJsonOptions(this IServiceCollection services)
    {
        JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            MaxDepth = 10,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true,
            DefaultBufferSize = 128
        };
        serializerOptions.Converters.Add(new JsonStringEnumConverter());

        return services.AddSingleton(s => serializerOptions);
    }
}
