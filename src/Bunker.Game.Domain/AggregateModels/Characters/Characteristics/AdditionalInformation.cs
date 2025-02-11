namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class AdditionalInformation : ValueObject, ICharacteristic
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

    public string GetDescription()
    {
        return Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
    }
}
