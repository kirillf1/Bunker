namespace Bunker.GameComponents.API.Entities.BunkerComponents
{
    public class BunkerDescriptionEntity
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
        public BunkerDescriptionEntity()
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Рассмотрите возможность добавления модификатора "required" или объявления значения, допускающего значение NULL.
        { }

        public BunkerDescriptionEntity(string text)
        {
            Id = Guid.CreateVersion7();
            Text = text;
        }
    }
}
