namespace Bunker.Application.Shared.Contracts.IntegrationEvents.GameResults.GameComponents;

public record CharacterData(
    Guid Id,
    string Name,
    string AdditionalInformation,
    int Age,
    bool CanGiveBirth,
    string Health,
    string Phobia,
    string Sex,
    string HobbyDescription,
    byte HobbyExperience,
    string ProfessionDescription,
    byte ProfessionExperienceYears,
    double Height,
    double Weight,
    IEnumerable<CharacterItemData> Items,
    IEnumerable<CharacterTraitData> Traits
);
