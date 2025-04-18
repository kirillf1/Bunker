namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Age : ValueObject, ICharacteristic
{
    public const int MAX_GAME_CHARACTER_YEARS = 100;
    public const int MIN_GAME_CHARACTER_YEARS = 17;

    public int Years { get; }

    public Age(int years)
    {
        if (years < MIN_GAME_CHARACTER_YEARS || years > MAX_GAME_CHARACTER_YEARS)
            throw new ArgumentException(
                $"Age must be more than {MIN_GAME_CHARACTER_YEARS} and less then {MAX_GAME_CHARACTER_YEARS}"
            );

        Years = years;
    }

    public Age()
    {
        Years = MIN_GAME_CHARACTER_YEARS;
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
