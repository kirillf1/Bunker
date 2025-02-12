namespace Bunker.Game.Domain.AggregateModels.Catastrophes;

public class Catastrophe : Entity<Guid>, IAggregateRoot
{
    public Guid GameSessionId { get; }
    public string Description { get; private set; }
    public bool IsReadOnly { get; private set; }

    public Catastrophe(Guid id, Guid gameSessionId, string description)
        : base(id)
    {
        GameSessionId = gameSessionId;
        Description = description;
        IsReadOnly = false;
    }

    public void UpdateDescription(string description)
    {
        if (IsReadOnly)
        {
            throw new InvalidGameOperationException("Catastrophe is readonly");
        }

        Description = description;
    }

    public void MarkReadOnly()
    {
        IsReadOnly = true;
    }
}
