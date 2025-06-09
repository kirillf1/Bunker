using System.Text;

namespace Bunker.ResultCreator.API.Domain.Characters;

public class Character
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AdditionalInformation { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool CanGiveBirth { get; set; }
    public string Health { get; set; } = string.Empty;
    public string Phobia { get; set; } = string.Empty;
    public string Sex { get; set; } = string.Empty;

    public string HobbyDescription { get; set; } = string.Empty;
    public byte HobbyExperience { get; set; }

    public string ProfessionDescription { get; set; } = string.Empty;
    public byte ProfessionExperienceYears { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }

    public List<CharacterItem> Items { get; set; } = new();
    public List<CharacterTrait> Traits { get; set; } = new();

    public SexType GetSexType()
    {
        if (Sex.Contains("мужчина", StringComparison.OrdinalIgnoreCase))
        {
            return SexType.Man;
        }
        else if (Sex.Contains("женщина", StringComparison.OrdinalIgnoreCase))
        {
            return SexType.Woman;
        }
        return SexType.Unknown;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Персонаж {Name}:");
        sb.AppendLine($"Профессия {ProfessionDescription} стаж: {ProfessionExperienceYears} лет");
        sb.AppendLine($"Пол: {Sex}");
        sb.AppendLine($"Возраст: {Age} {GetAgeWord(Age)}");
        sb.AppendLine($"Деторождение: {(CanGiveBirth ? "не childfree" : "childfree")}");
        sb.AppendLine($"Телосложение: вес: {Weight} кг., рост: {Height} см. - {GetBodyType()}");
        sb.AppendLine($"Здоровье: {Health}");

        foreach (var trait in Traits)
        {
            sb.AppendLine($"Черта характера: {trait.Description}");
        }

        sb.AppendLine($"Фобия: {Phobia}");
        sb.AppendLine($"Хобби: {HobbyDescription} опыт {HobbyExperience} {GetYearsWord(HobbyExperience)}");
        sb.AppendLine($"Дополнительная информация: {AdditionalInformation}");

        for (var i = 0; i < Items.Count; i++)
        {
            sb.AppendLine($"Багаж №{i + 1}: {Items[i].Description}");
        }

        return sb.ToString().TrimEnd();
    }

    private string GetBodyType()
    {
        if (Height <= 0 || Weight <= 0)
            throw new ArgumentException("Invalid character size");

        var bmi = Weight / (Height / 100 * Height / 100);

        return bmi switch
        {
            >= 18 and < 27 => "Нормальный вес",
            >= 27 and < 30 => "Избыточный вес",
            >= 30 and < 35 => "Ожирение I степени",
            >= 35 and < 40 => "Ожирение II степени",
            >= 40 => "Ожирение III степени",
            _ => "Недостаток массы тела",
        };
    }

    private static string GetAgeWord(int age)
    {
        if (age % 10 == 1 && age % 100 != 11)
            return "год";
        if ((age % 10 == 2 || age % 10 == 3 || age % 10 == 4) && (age % 100 < 10 || age % 100 >= 20))
            return "года";
        return "лет";
    }

    private static string GetYearsWord(int years)
    {
        if (years % 10 == 1 && years % 100 != 11)
            return "год";
        if ((years % 10 == 2 || years % 10 == 3 || years % 10 == 4) && (years % 100 < 10 || years % 100 >= 20))
            return "года";
        return "лет";
    }
}
