namespace Bunker.GameComponents.API.Entities.CatastropheComponents
{
    public class CatastropheEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
        public CatastropheEntity() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.

        public CatastropheEntity(string description)
        {
            Id = Guid.CreateVersion7();
            Description = description;
        }
    }
}
