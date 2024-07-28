namespace EventHub.Domain.Interfaces;

public interface ISerializeService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}