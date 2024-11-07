namespace EventHub.Abstractions.Services;

/// <summary>
/// Defines a contract for a service that handles object serialization and deserialization.
/// </summary>
public interface ISerializeService
{
    /// <summary>
    /// Serializes an object to a string representation.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object to serialize.
    /// </typeparam>
    /// <param name="obj">
    /// The object to serialize.
    /// </param>
    /// <returns>
    /// A string representing the serialized object.
    /// </returns>
    string Serialize<T>(T obj);

    /// <summary>
    /// Serializes an object to a string representation with a specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object to serialize.
    /// </typeparam>
    /// <param name="obj">
    /// The object to serialize.
    /// </param>
    /// <param name="type">
    /// The type to be used during serialization.
    /// </param>
    /// <returns>
    /// A string representing the serialized object.
    /// </returns>
    string Serialize<T>(T obj, Type type);

    /// <summary>
    /// Deserializes a string to an object of the specified type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the object to deserialize to.
    /// </typeparam>
    /// <param name="text">
    /// A string representing the serialized object.
    /// </param>
    /// <returns>
    /// An object of type <typeparamref name="T"/> that has been deserialized from the string.
    /// </returns>
    T Deserialize<T>(string text);
}