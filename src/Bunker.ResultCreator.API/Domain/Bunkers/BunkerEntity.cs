using System.Text;

namespace Bunker.ResultCreator.API.Domain.Bunkers;

public class BunkerEntity
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;

    public List<BunkerRoom> Rooms { get; set; } = new();
    public List<BunkerItem> Items { get; set; } = new();
    public List<BunkerEnvironment> Environments { get; set; } = new();

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine("Убежище:");
        sb.AppendLine(Description);

        foreach (var room in Rooms)
        {
            sb.AppendLine($"В убежище оборудовано: {room.Description}");
        }

        foreach (var item in Items)
        {
            sb.AppendLine($"В убежище есть: {item.Description}");
        }

        foreach (var environment in Environments)
        {
            sb.AppendLine($"В убежище живут: {environment.Description}");
        }

        return sb.ToString().TrimEnd();
    }
}
