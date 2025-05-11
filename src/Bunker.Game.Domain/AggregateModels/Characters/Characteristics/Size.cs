namespace Bunker.Game.Domain.AggregateModels.Characters.Characteristics;

public class Size : ValueObject, ICharacteristic
{
    public const int MAX_CHARACTER_HEIGHT = 210;
    public const int MIN_CHARACTER_HEIGHT = 130;

    public const int MAX_CHARACTER_WEIGHT = 150;
    public const int MIN_CHARACTER_WEIGHT = 40;

    private double _height;

    private double _weight;

    public double Height
    {
        get => _height;
        private set
        {
            if (value < MIN_CHARACTER_HEIGHT || value > MAX_CHARACTER_HEIGHT)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"Height must be between {MIN_CHARACTER_HEIGHT} and {MAX_CHARACTER_HEIGHT} cm."
                );
            }

            _height = value;
        }
    }

    public double Weight
    {
        get => _weight;
        private set
        {
            if (value < MIN_CHARACTER_WEIGHT || value > MAX_CHARACTER_WEIGHT)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"Weight must be between {MIN_CHARACTER_WEIGHT} and {MAX_CHARACTER_WEIGHT} kg."
                );
            }

            _weight = value;
        }
    }

    public Size(double height, double weight)
    {
        Weight = weight;
        Height = height;
    }

    private Size()
    {
        Weight = MIN_CHARACTER_WEIGHT;
        Height = MIN_CHARACTER_HEIGHT;
    }

    public double GetAverageIndexBody()
    {
        return Weight / (Height / 100 * Height / 100);
    }

    public string GetAverageIndexBodyDescription()
    {
        var index = GetAverageIndexBody();
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
