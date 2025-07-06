namespace Bunker.ResultCreator.API.Infrastructure.AI.Options;

public class ChatConfiguration
{
    public float Temperature { get; set; } = 0.5f;
    public int MaxOutputTokens { get; set; } = 1012;
    public int TopK { get; set; } = 60;
    public float TopP { get; set; } = 0.95f;
}
