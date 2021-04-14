# Hoe test ik mijn microservice?

Voor het testen van deze microservice heb ik onderzoek gedaan naar Unit testing door middel van mocked / fake data, en integration testing. Het verschil tussen de twee is de scope waarin je test. Unit tests zijn vaak kleinere componenten in je applicatie die individueel getest worden. Integration tests testen een bepaald geheel van de applicatie. In context van een API zal dit de flow zijn van een HTTP client die een call maakt naar de API, tot aan de response die terug komt van de API. Vaak komt hier dan ook een in-memory of echte database bij kijken die na het testen weer vernietigd word. In deze POC heb ik gekozen voor een in-memory oplossing, omdat dit makkelijker is om te integreren in een CI pipeline.

### Unit Testing met FakeItEasy

Voor het maken van de Unit tests heb ik de mocking library [FakeItEasy](https://fakeiteasy.github.io/) gebruikt. Dit is een 'faking' framework die fake objects, mocks of stubs kan maken. Het voordeel van deze library is dat het makkelijk te gebruiken en leesbaar is:

```c#
// Arrange
const int fakeFoodItemCount = 1;

var fakeFoodItem = A.CollectionOfDummy<FoodItem>(fakeFoodItemCount).ToList();
var fakeFoodRepository = A.Fake<IFoodRepository>();
var foodItemController = new FoodItemController(fakeFoodRepository);

A.CallTo(() => fakeFoodRepository.GetAllAsync())
    .Returns(Task.FromResult(fakeFoodItem));

// Act
var actionResult = (await foodItemController.GetAllAsync()).Result as OkObjectResult;
var returnedFoodItems = (actionResult.Value as IEnumerable<FoodItemDto>).ToList();

// Assert
Assert.Equal(fakeFoodItemCount, returnedFoodItems.Count);
```

Het voorbeeld hierboven gebruikt de triple A pattern (Arrange, Act, Assert). Arrange zijn alle acties die uitgevoerd moeten worden voor de test. Act is wanneer je actie gaat ondernemen en daadwerkelijk een call maakt naar de component die je wil testen. Assert is de evaluatie, waar je controleert of het resultaat klopt.

- Arrange

```c#
const int fakeFoodItemCount = 1;

var fakeFoodItem = A.CollectionOfDummy<FoodItem>(fakeFoodItemCount).ToList();
var fakeFoodRepository = A.Fake<IFoodRepository>();
var foodItemController = new FoodItemController(fakeFoodRepository);

A.CallTo(() => fakeFoodRepository.GetAllAsync())
    .Returns(Task.FromResult(fakeFoodItem));
```

Omdat de methode die getest gaat worden een "GetAll" methode is, word er een count meegegeven om te bepalen hoeveel fakes er aangemaakt gaan worden. Deze integer word meegegeven aan `A.CollectionOfDummy<FoodItem>(fakeFoodItemCount).ToList();`. FakeItEasy maakt dan een collectie aan met fake data van het type wat meegegeven is. In dit geval is dit `FoodItem`.

Vervolgens maak je ook een fake aan van je repository en de controller die gebruik maakt van deze repository door middel van dependency injection. Geef de faked repository mee aan je controller.

Als laatste geef je aan wat je terug wil krijgen wanneer je een call maakt naar de GetAll methode in je faked repository. In dit geval is het de lijst die met `A.CollectionOfDummy` is aangemaakt. Omdat de GetAll methode asynchroon word uitgevoerd, geef je een Task terug.

- Act

```c#
var actionResult = (await foodItemController.GetAllAsync()).Result as OkObjectResult;
var returnedFoodItems = (actionResult.Value as IEnumerable<FoodItemDto>).ToList();
```

In de code hierboven word de methode uit de faked repository aangeroepen. Het resultaat van deze call word opgevangen als een OkObjectResult, omdat de endpoint van de API een OK response terug zou moeten geven. Deze response bevat ook de payload, die de fake data bevat. Deze lijst krijg je terug als een IEnumerable, dus is er een call naar ToList nodig. returnedFoodItems is nu een lijst van fooditem DTO's zoals verwacht.

- Assert

```c#
Assert.Equal(fakeFoodItemCount, returnedFoodItems.Count);
```

In het voorbeeld hierboven controleer je of het resultaat het verwachte resultaat is. In dit geval een simpele controle of het aantal fooditems in de lijst die bij Arrange gespecificeert is gelijk staat aan het aantal dat teruggeven is uit de response.

### Integration Testing met Mongo2Go

Integratie testen kan op verschillende manieren. Ik ben begonnen met een verdieping in integratie testing met continous integration in gedachte. Dit betekend dat de tests geautomatiseerd moeten worden. Na wat informatie opzoeken op internet, ben ik tot de conclusie gekomen dat een in-memory oplossing hier geschikt voor zou moeten zijn.

Omdat ik voor deze POC MongoDB heb gebruikt, is dit wat lastiger op te zetten. ASP .NET beschikt over een in-memory package die je met SQLServer kan gebruiken. Zonder veel moeite kun je hier tests in draaien met een ORM zoals EntityFramework. Helaas is er geen out-of-the-box support voor NO-SQL, waardoor ik verder ben gaan zoeken. Voor de in-memory database requirement ben ik uitgekomen op [Mongo2Go](https://github.com/Mongo2Go/Mongo2Go). Mongo2Go maakt het mogelijk om een in-memory database op te zetten van MongoDB. Het communiceren met de in-memory database word gedaan met dezelfde package (MongoDB Driver).

De opzet van de integratie tests heb ik opgesplitst naar 2 verschillende classes:

- MongoRunner (Zet de in-memory database op met de juiste collecties. Bevat een methode om de in-memory instantie te starten)
- IntegrationTest (Abstractie die de MongoRunner class en WebApplicationFactory (testserver) met de HttpClient samenvoegt)

De test classes die aangemaakt worden voor integratie tests maken gebruik (erven van) de IntegrationTest class. Dit betekend dat er per 'onderwerp' onder de integratie tests maar een keer een in-memory database en testserver word opgestart.

### Details IntegrationTest class

Deze class initieert op de achtergrond de Testserver die de API start, de HTTPClient die de API gaat aanroepen, en de in-memory database van MongoDB. De opzet heb ik specifiek in de volgende stappen gedaan:

```c#
protected readonly HttpClient _httpClient;
private readonly WebApplicationFactory<Startup> _applicationFactory;

protected IntegrationTest()
{
    SetupMongoRunner();

    _applicationFactory = new WebApplicationFactory<Startup>();
    var appConfiguration = _applicationFactory.Services.GetRequiredService<IConfiguration>();

    appConfiguration.GetSection("ConnectionStrings")["MongoConnection"] = Runner.ConnectionString;

    _httpClient = _applicationFactory.CreateClient();

    this.ToString();
}
```

1. De eerste stap is het aanmaken, configureren en opstarten van de in-memory database van MongoDB.
2. Maak een nieuwe instantie van `WebApplicationFactory`, en geef de startup van de API mee. Wanneer de Testserver start, zal hij een in-memory versie van jouw API starten. Alles wat er nodig is, is om de startup class mee te geven.
3. Verkrijg de Configuratie van de API, zodat deze aangepast kan worden.
4. Verander de connection-string die tijdens development is gebruikt naar de in-memory database van MongoRunner.
5. Maak een nieuwe HTTP Client aan via de WebApplicationFactory en sla deze op. Via deze client ga je de endpoints van de testserver aanroepen.

Voor meer informatie zie de Microsoft documentatie over [WebApplicationFactory]()

### IntegratieTests uitvoeren op de TestServer

Hieronder kun je de controller methode zien die een fooditem teruggeeft met een OK response.

```c#
        [HttpGet("integrationtest")]
        public ActionResult<string> IntegrationTestEndpoint()
        {
            var foodItem = new FoodItem()
            {
                Id = Guid.NewGuid(),
                Name = "DummyFoodItem",
                Description = "This Dummy is deserialized from JSON"
            };

            return Ok(foodItem);
        }
```

De volgende code test de controller methode door middel van de classes die opgezet zijn voor integratie tests. Hiervoor word xUnit gebruikt. Omdat alle code asynchroon uitgevoerd word, moet ook de test als 'async' worden gemarkeerd.

```c#
        [Fact]
        public async Task MyFirstIntegrationTest_HelloFromEndpoint()
        {
            var expectedFoodName = "DummyFoodItem";
            var expectedFoodDesc = "This Dummy is deserialized from JSON";
            FoodItem result = null;

            try
            {
                result = await _httpClient.GetFromJsonAsync<FoodItem>(ApiRoutes.FoodItems.TestResponse);

                Runner.Dispose();
                _httpClient.Dispose();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception thrown when executing test MyFirstIntegrationTest_HelloFromEndpoint(): {exception.Message}");
            }

            Assert.NotNull(result);
            Assert.IsType<FoodItem>(result);
            Assert.Equal(result.Name, expectedFoodName);
            Assert.Equal(result.Description, expectedFoodDesc);
        }
```

Ook hier word de triple A pattern toegepast voor het testen. Eerst word de data voorbereid die je verwacht te ontvangen. In dit geval een fooditem met geinitialiseerde properties. Ook word er een fooditem meegegeven die nog 'null' is. Dit object word vervangen door het object dat opgevraagd word via de API endpoint call.

```c#
result = await _httpClient.GetFromJsonAsync<FoodItem>(ApiRoutes.FoodItems.TestResponse);
```

Hier gebruiken we de HTTP client die via de IntegrationTest class is geinstantieerd. De methode die hiervoor word gebruikt is GetFromJsonAsync omdat de response van de API endpoint in JSON formaat word teruggegeven. De type die gespecificeert word zal gebruikt worden om de JSON te deserializen naar een object van het type `FoodItem`. Als argument geef je uiteindelijk de url mee waar de API op de testserver gehost word. Dit zal voor de in-memory database localhost op poort 80 zijn. Een voorbeeld van een endpoint: `http://localhost:80/fooditems/myendpoint`

Vervolgens worden de Testserver en de Mongo Runner afgesloten en worden resources vrijgegeven met de Dispose() methoden.

De variable `result` van het type `FoodItem` zou nu hetzelfde object moeten bevatten die via de integrationtest endpoint terug is gegeven. Je kan dit nu vergelijken met de expected data die je tijdens het voorbereiden hebt gedefineerd.

### Bronnen

[FakeItEasy Website](https://fakeiteasy.github.io/)
[Mongo2Go Git](https://github.com/Mongo2Go/Mongo2Go)
