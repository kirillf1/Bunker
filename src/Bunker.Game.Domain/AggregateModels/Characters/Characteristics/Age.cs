﻿namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Age : ValueObject, ICharacteristic
{
    public int Years { get; }

    public Age(int years)
    {
        if (years <= 17 || years >= 100)
            throw new ArgumentException("Age must be more than 17 and less then 100");

        Years = years;
    }

    public Age()
    {
        Years = 18;
    }

    public string GetDescription()
    {
        return Years.ToString();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Years;
    }
}
