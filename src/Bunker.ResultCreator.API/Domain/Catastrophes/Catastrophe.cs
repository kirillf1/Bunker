namespace Bunker.ResultCreator.API.Domain.Catastrophes;

public class Catastrophe
{
    public required string Description { get; set; }

    public override string ToString()
    {
        return $"Катаклизм :{Environment.NewLine}{Description}";
    }
}
