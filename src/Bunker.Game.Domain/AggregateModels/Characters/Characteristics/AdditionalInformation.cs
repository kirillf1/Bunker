namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class AdditionalInformation : ValueObject
{
    public string Description { get; }

    public AdditionalInformation(string description)
    {
        Description = description;
    }

    public override string ToString()
    {
        return "Дополнительная информация: " + Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
