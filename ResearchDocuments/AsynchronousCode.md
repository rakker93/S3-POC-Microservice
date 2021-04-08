# Hoe implementeer ik asynchronous code in mijn microservice?

### Wat is asynchronous code?

Om beter te begrijpen wat asynchronous code oplost, is het goed om te weten wat de verschillen zijn.

1. Synchronous code
   Wanneer je een call maakt naar een synchrone methode geef je een taak aan een zogenoemde thread. Een voorbeeld hiervan is het opvragen van informatie vanuit een database. De thread wacht tot dat deze taak volbracht is. De thread neemt ook geen nieuwe taken aan totdat de eerste initiele taak volbracht is. Dit betekend wanneer er iemand anders data wil fetchen uit de database, hij zal moeten wachten. Dit is niet ideaal in context van een API.

2. Asynchronous code
   Wanneer je een call maakt naar een asynchronous method, zal de thread niet wachten totdat de taak volbracht is. De thread accepteert dan ook nieuwe taken. Wanneer de initiele taak volbracht is, neemt de thread deze data op en geeft deze terug. Het is ook mogelijk om een thread te laten wachten, in dat geval zal een andere thread de taak op zich nemen. Dit betekend dat er verschillende taken langs elkaar heen kunnen lopen. Dit is zeer ideaal in een applicatie die door meerdere mensen tegelijk gebruikt word. Zeker wanneer de applicatie groeit (en daarmee ook de data in een data-store), mag je niet uitgaan van tijd. Dit kan slechte gevolgen hebben voor je applicatie.

### Hoe gebruik ik asynchronous methods in C#?

In dit voorbeeld gebruik ik een methode die data opvraagt uit een database. Deze methode bevind zich in een repository. Voor meer informatie over de repository pattern: [Repository Pattern]()

```c#
public async Task<FoodItem> GetByIdAsync(int id)
{
  return await _foodItemCollection.GetById(id);
}

public async Task RemoveAsync(int id)
{
  await _foodItemCollection.Remove(id);
}
```

De eerste methode geeft een foodItem terug uit de database die gebaseerd is op een model. Dit is een class die de waarden van alle attributen vasthoud die gefetched worden uit de database. Dit gebeurt op basis van een id.

De tweede methode geeft niks terug (void) en voert alleen een actie uit. In dit geval het verwijderen van een foodItem uit de database.

Wanneer je een asynchronous method schrijft moet je deze decoraten met de keyword 'async'. Alle asyncrone methoden moeten gebruik maken van het type 'Task'. Het is via generics mogelijk om je eigen type terug te geven:

- Task: Is gelijk aan void, je returned niks.
- Task<T>: Je returned een bepaalde type van je asyncrhone methode.

**Note:** Een methode markeren als async wil niet zeggen dat deze asynchronous uitgevoerd word. Wanneer je geen 'await' keyword gebruikt in je methode, zal deze altijd synchronous uitgevoerd worden.

### Voorbeeld await

Dit voorbeeld komt van stackoverflow: [Async Await Examples](https://stackoverflow.com/questions/14455293/how-and-when-to-use-async-and-await)

```c#
Console.WriteLine(DateTime.Now);

// This block takes 1 second to run because all
// 5 tasks are running simultaneously
{
    var a = Task.Delay(1000);
    var b = Task.Delay(1000);
    var c = Task.Delay(1000);
    var d = Task.Delay(1000);
    var e = Task.Delay(1000);

    await a;
    await b;
    await c;
    await d;
    await e;
}

Console.WriteLine(DateTime.Now);

// This block takes 5 seconds to run because each "await"
// pauses the code until the task finishes
{
    await Task.Delay(1000);
    await Task.Delay(1000);
    await Task.Delay(1000);
    await Task.Delay(1000);
    await Task.Delay(1000);
}
Console.WriteLine(DateTime.Now);
```
