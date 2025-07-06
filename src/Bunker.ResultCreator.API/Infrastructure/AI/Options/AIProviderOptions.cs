namespace Bunker.ResultCreator.API.Infrastructure.AI.Options;

public class AIProviderOptions
{
    public const string Section = "AI";

    public AIProviderType Provider { get; set; } = AIProviderType.Ollama;
    public ChatConfiguration Chat { get; set; } = new();

    public int ParallelAgentWorkers { get; set; } = 1;
}
