# Deelvraag: Hoe gebruik ik dependency injection?

### Wat is Dependency Injection?

Een voorbeeld van een dependency is een class die een andere class gebruikt. In plaats van een object aan te maken met `new()`, gebruik je de instantie van diezelfde class in een constructor van een andere class waar je de dependency wil injecteren. Deze instantie die via de constructor binnen komt word dan opgeslagen in een private field. Je injecteert dan de dependency, wat dus dependency injection omschrijft.

En een voorbeeld van hoe het injecteren van een dependency in een constructor eruit ziet:

```c#
private IFoodRepository _foodRepository;

public ItemsController(IFoodRepository foodRepository)
{
  _foodRepository = foodRepository;
}
```

### Dependency Inversion Principle (D van S.O.L.I.D)

![DependencyInjection](https://user-images.githubusercontent.com/60918040/111621644-834dab80-87e8-11eb-9573-cc87e959ee28.png)

Dependency Injection gaat samen met de Dependency Inversion Principle. Dit principe zegt dat code afhankelijk moet zijn van instructies in plaats van implementaties. In C# termen kun je dit zien als classes en interfaces.

Zoals in de afbeelding hierboven zal een class afhankelijk zijn van een interface in plaats van een directe implementatie.

### Hoe word een Dependency aangemaakt?

Omdat een class niet direct een instantie van de dependency kan aanmaken (hij kent alleen het contract/interface), maakt hij de instantie aan via een zogenoemde **Service Container**. De IServiceProvider interface is hier verantwoordelijk voor in ASP.NET. Je registreert je dependencies in deze container (normaal in de startup class onder services). Wanneer je een instantie nodig hebt van een dependency zal de ServiceProvider automatisch een instantie teruggeven die dan geïnjecteerd word.
