namespace Corp.Operations;

/// <summary>
/// AI channel enumeration class.
/// </summary>
public record AiChannelEnum(int Ordinal, string Id, string Name)
    : StringEnumeration(Id, Name, Ordinal)
{
    public const int KeyLength = 20;

    public static AiChannelEnum Chat => new(10, nameof(AiChannel.chat), "Chat");
    public static AiChannelEnum InlineAction => new(20, nameof(AiChannel.inline_action), "Inline Action");
    public static AiChannelEnum Api => new(30, nameof(AiChannel.api), "API");

    public static IEnumerable<AiChannelEnum> GetAll() => GetAll<AiChannelEnum>();
}
