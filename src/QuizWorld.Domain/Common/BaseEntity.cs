using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace QuizWorld.Domain.Common;

/// <summary>
/// Represents a base entity.
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// The unique identifier of the entity.
    /// </summary>
    [BsonId]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary> 
    /// The unique identifier of the entity in string format.
    /// </summary>
    [BsonElement("_sId")]
    [JsonIgnore]
    public string IdString
    {
        get => Id.ToString();
        set { _ = Id.ToString(); }
    }

    /// <summary>
    /// Catch all property for storing additional data.
    /// </summary>
    [JsonIgnore]
    [BsonExtraElements]
    public BsonDocument CatchAll { get; set; }
}
