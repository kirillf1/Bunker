namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Size : ValueObject
{
    private double height;

    private double weight;

    public double Height
    {
        get => height;
        private set
        {
            height = value;
            if (height > 210)
                height = 210;
            else if (height < 130)
                height = 130;
        }
    }

    public double Weight
    {
        get => weight;
        private set
        {
            weight = value;
            if (weight > 230)
                weight = 230;
            else if (weight < 35)
                weight = 35;
        }
    }

    public Size(double height, double weight)
    {
        Weight = weight;
        Height = height;
    }

    private Size() { }

    public string GetAverageIndexBodyDescription()
    {
        var index = Weight / (Height / 100 * Height / 100);
        return index switch
        {
            >= 18 and < 27 => "Нормальный вес",
            >= 27 and < 30 => "Избыточный вес",
            >= 30 and < 35 => "Ожирение I степени",
            >= 35 and < 40 => "Ожирение II степени",
            >= 40 => "Ожирение III степени",
            _ => "Недостаток массы тела",
        };
    }

    public override string ToString()
    {
        return $"Телосложение: вес: {Weight} кг., рост: {Height} см. - {GetAverageIndexBodyDescription()}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Height;
        yield return Weight;
    }
}
