namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Profession : ValueObject, ICharacteristic
{
    public const byte MAX_GAME_EXPERIENCE_YEARS = 5;
    public const byte MIN_GAME_EXPERIENCE_YEARS = 1;

    public string Description { get; }
    public byte ExperienceYears { get; }

    public Profession(string description, byte experienceYears)
    {
        if (experienceYears < MIN_GAME_EXPERIENCE_YEARS || experienceYears > MAX_GAME_EXPERIENCE_YEARS)
        {
            throw new ArgumentException(
                $"Experience must be between {MIN_GAME_EXPERIENCE_YEARS} and {MAX_GAME_EXPERIENCE_YEARS} years."
            );
        }

        Description = description;
        ExperienceYears = experienceYears;
    }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
    private Profession() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

    public string GetDescription()
    {
        return Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return ExperienceYears;
    }
}
