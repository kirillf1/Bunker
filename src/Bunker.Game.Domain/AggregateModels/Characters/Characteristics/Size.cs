namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Size : ValueObject, ICharacteristic
{
    private double _height;

    private double _weight;

    public double Height
    {
        get => _height;
        private set
        {
            _height = value;
            if (_height > 210)
                _height = 210;
            else if (_height < 130)
                _height = 130;
        }
    }

    public double Weight
    {
        get => _weight;
        private set
        {
            _weight = value;
            if (_weight > 230)
                _weight = 230;
            else if (_weight < 35)
                _weight = 35;
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

    public string GetDescription()
    {
        return $"Телосложение: вес: {Weight} кг., рост: {Height} см. - {GetAverageIndexBodyDescription()}";
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
