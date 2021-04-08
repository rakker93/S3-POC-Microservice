# Deelvraag: Hoe implementeer ik de repository pattern in mijn project?

### Wat is een repository?

Een repository is een abstractie tussen de data en business-logic lagen van een applicatie.

![Repository](https://user-images.githubusercontent.com/60918040/111624474-091f2600-87ec-11eb-9b30-929e64d75096.png)

### Voorbeeld in code

Voor deze POC heb ik gebruik gemaak van de repository pattern. Dit zijn de volgende bestanden:

- IFoodRepository.cs (het contract)
- FoodRepository.cs (de implementatie)

Dit word geregistreerd in de dependency container via de ConfigureServices methode in de startup.cs class.

```c#
services.AddSingleton<IFoodRepository, FoodRepository>();
```

Door middel van dependency injection word de IFoodRepository interface gebruikt in de FoodItemsController.

```c#
private readonly IFoodRepository _foodRepository;

public FoodItemController(IFoodRepository foodRepository)
{
    this._foodRepository = foodRepository;
}
```

Elke keer wanneer er een methode aangeroepen word die in het contract staat (interface), weet .NET bij welke implementatie hij moet zijn en welke methoden aangeroepen moet worden. Wanneer ik van database zou willen veranderen, hoef ik alleen een andere implementatie mee tegeven aan de dependency injection container. De code in de controller hoeft niet te veranderen. De nieuwe implementatie class moet wel nog steeds aan het contract voldoen.
