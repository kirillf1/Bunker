namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Hobby : ValueObject, ICharacteristic
{
    public const byte MAX_GAME_EXPERIENCE_YEARS = 3;
    public const byte MIN_GAME_EXPERIENCE_YEARS = 1;

    public string Description { get; }
    public byte Experience { get; }

    public Hobby(string description, byte hobbyExperience)
    {
        if (hobbyExperience < MIN_GAME_EXPERIENCE_YEARS || hobbyExperience > MAX_GAME_EXPERIENCE_YEARS)
        {
            throw new ArgumentException(
                $"Hobby experience must be between {MIN_GAME_EXPERIENCE_YEARS} and {MAX_GAME_EXPERIENCE_YEARS} years."
            );
        }

        Description = description;
        Experience = hobbyExperience;
    }

#pragma warning disable CS8618
    private Hobby() { }
#pragma warning restore CS8618

    public string GetDescription()
    {
        return Description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Description;
        yield return Experience;
    }
}
