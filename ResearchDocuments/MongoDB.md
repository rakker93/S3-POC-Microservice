# Hoe zet ik een MongoDB server op die communiceert met een .NET applicatie?

### Wat is MongoDB?

MongoDB is een open-source 'document' database die makkelijk schaalbaar, en flexibel is. Het is makkelijker te gebruiken dan een standaard relationele database, maar ook geschikt voor complexere scenario's. MongoDB ondersteunt verschillende programmeer-talen om te kunnen communiceren met een database (clients). In het geval van .NET bestaat hier een NuGet package voor, namelijk: `MongoDB.Driver`

MongoDB slaat data op in document-bestanden die sterk op JSON lijkt. Attributes / velden kunnen per document verschillen omdat je niet gebonden bent aan een schema. De data kan altijd veranderen.

### Nieuwe Mongo database starten in een Docker container

Om mijn pc schoon te houden, en omdat Docker containers 'out of the box' werken heb ik ervoor gekozen om de officiele Mongo image te gebruiken. Met deze image kan ik via Docker een 'schone' instantie van MongoDB draaien zonder veel te configureren. Dit vereist wel dat docker geïnstalleerd is. In mijn geval gebruik ik voor deze POC Docker Desktop. Voor meer informatie over Docker heb ik het volgende document geschreven: [Research Docker]()

Met het volgende commando kun je een nieuwe MongoDB server starten:
`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo`

`-d` Betekend dat de container in 'detached modus' draait.

`-rm` Wanneer de container gestopt word, zal deze ook verwijderd worden.

`--name` Bind een naam aan de container.

`-p` Map de poort van de host machine naar de container. De standaard poort voor Mongo is 27017.

`-v` Geeft aan dat je een volume wil gebruiken om te bepalen hoe database bestanden opgeslagen worden. Het pad links van de dubbele punt **:** geeft een daadwerkelijke locatie aan op de host machine, waar de database de data persistent opslaat. Rechts van de dubbele punt is de locatie in de container.

`mongo` De naam van de image die gebruikt word. Wanneer deze image zich niet bevind op de host machine zal deze gedownload worden van Docker Hub.

Note: Gebruik `docker ps` om te verifiëren of de container gestart is.

### MongoDB Driver NuGet package integreren met .NET

1. Zorg dat de Mongo client geïnstalleerd is.

Om de package via de command-line te installeren gebruik: `dotnet add package MongoDB.Driver`

Note: Naast het installeren van de Mongo .NET client en het starten van een Mongo container, zijn er nog wat voorbereidingen nodig in de .NET applicatie. Om zo gestructureerd mogelijk te werk te gaan, zijn er veel soorten design patterns verzonnen die het schrijven van code beter leesbaar en overzichtelijker maken. Een van die design patterns is de 'Repository pattern'. De Repository pattern zal dan ook gebruikt worden bij het maken van deze POC.

Voor meer informatie over de Repository pattern: [Research Repository Pattern]()

2. Configureren Mongo in .NET

### Bronnen

Voor het onderzoek naar MongoDB heb ik de officiele documentatie gebruikt:

[Documentation Mongo - MongoDB with .NET](https://www.mongodb.com/blog/post/quick-start-c-sharp-and-mongodb-starting-and-setup)
[Documentation Mongo - What is MongoDB](https://www.mongodb.com/what-is-mongodb)
[Youtube Microsoft - Connecting API to a data-store](https://www.youtube.com/watch?v=jxlKfHGMG-g&list=PLdo4fOcmZ0oVjOKgzsWqdFVvzGL2_d72v&index=11)
