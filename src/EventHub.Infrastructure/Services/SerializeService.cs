using EventHub.Abstractions;
using EventHub.Abstractions.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EventHub.Infrastructure.Services;

/// <summary>
/// Provides functionality for serializing and deserializing objects to and from various formats.
/// </summary>
/// <remarks>
/// This class implements the <see cref="ISerializeService"/> interface and provides concrete methods
/// for serializing objects to a specific format (e.g., JSON) and deserializing them back to objects.
/// </remarks>
public class SerializeService : ISerializeService
{
    public T Deserialize<T>(string text)
    {
        return JsonConvert.DeserializeObject<T>(text);
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }
        });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonConvert.SerializeObject(obj, type, new JsonSerializerSettings());
    }
}