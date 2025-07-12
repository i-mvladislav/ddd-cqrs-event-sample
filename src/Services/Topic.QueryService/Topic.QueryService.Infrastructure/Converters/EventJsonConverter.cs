using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Events;
using Core.Events.Comments.CreateComment;
using Core.Events.Comments.RemoveComment;
using Core.Events.Comments.UpdateComment;
using Core.Events.Topics.CreateTopic;
using Core.Events.Topics.LikeTopic;
using Core.Events.Topics.RemoveTopic;
using Core.Events.Topics.UpdateTopic;

namespace Topic.QueryService.Infrastructure.Converters;

public class EventJsonConverter : JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type objectType)
    {
        return Type.IsAssignableFrom(typeof(BaseEvent));
    }
    
    public override BaseEvent? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
        {
            throw new JsonException($"Ошибка десериализации {nameof(JsonDocument)}");
        }

        if (!doc.RootElement.TryGetProperty("EventType", out var type))
        {
            throw new JsonException("Не удалось определить тип");
        }

        var typeDiscriminator = type.GetString();
        var json = doc.RootElement.GetRawText();

        BaseEvent? result = typeDiscriminator switch
        {
            nameof(CreateTopicEvent) =>
                JsonSerializer.Deserialize<CreateTopicEvent>(json),
            nameof(UpdateTopicEvent) =>
                JsonSerializer.Deserialize<UpdateTopicEvent>(json),
            nameof(LikeTopicEvent) =>
                JsonSerializer.Deserialize<LikeTopicEvent>(json),
            nameof(RemoveTopicEvent) =>
                JsonSerializer.Deserialize<RemoveTopicEvent>(json),
            nameof(CreateCommentEvent) =>
                JsonSerializer.Deserialize<CreateCommentEvent>(json),
            nameof(UpdateCommentEvent) =>
                JsonSerializer.Deserialize<UpdateCommentEvent>(json),
            nameof(RemoveCommentEvent) =>
                JsonSerializer.Deserialize<RemoveCommentEvent>(json),
            _ => null,
        };

        if (result is null)
        {
            string errorMessage = $"Неизвестный тип события: {typeDiscriminator}";
            throw new JsonException(errorMessage);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}