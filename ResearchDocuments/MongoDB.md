# Deelvraag: Hoe zet ik een MongoDB server op die communiceert met een .NET applicatie?

# Table of Content

1. Wat is MongoDB?
2. Nieuwe Mongo database starten in een Docker container
3. MongoDB Driver met .NET

---

### Wat is MongoDB?

MongoDB is een open-source 'document' database die makkelijk schaalbaar, en flexibel is. Het is makkelijker te gebruiken dan een standaard relationele database, maar ook geschikt voor complexere scenario's. MongoDB ondersteunt verschillende programmeer-talen om te kunnen communiceren met een database (clients). In het geval van .NET bestaat hier een NuGet package voor, namelijk: `MongoDB.Driver`

MongoDB slaat data op in document-bestanden die sterk op JSON lijkt. Attributes / velden kunnen per document verschillen omdat je niet gebonden bent aan een schema. De data kan altijd veranderen.

---

### Nieuwe Mongo database starten in een Docker container

Voor het initialiseren van een nieuwe server gebruik ik de officiele MongoDB image. Met deze image kan ik via Docker een 'schone' instantie van MongoDB draaien zonder veel te configureren. Dit vereist wel dat docker geïnstalleerd is. In mijn geval gebruik ik voor deze POC Docker Desktop. Voor meer informatie over Docker heb ik het volgende document geschreven: [Research Docker]()

Met het volgende commando kun je een nieuwe MongoDB server starten:
`docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo`

`-d` Betekend dat de container in 'detached modus' draait.

`-rm` Wanneer de container gestopt word, zal deze ook verwijderd worden.

`--name` Bind een naam aan de container.

`-p` Map de poort van de host machine naar de container. De standaard poort voor Mongo is 27017.

`-v` Geeft aan dat je een volume wil gebruiken om te bepalen hoe database bestanden opgeslagen worden. Het pad links van de dubbele punt **:** geeft een daadwerkelijke locatie aan op de host machine, waar de database de data persistent opslaat. Rechts van de dubbele punt is de locatie in de container.

`mongo` De naam van de image die gebruikt word. Wanneer deze image zich niet bevind op de host machine zal deze gedownload worden van Docker Hub.

Note: Gebruik `docker ps` om te verifiëren of de container gestart is.

Het is ook mogelijk om MongoExpress te gebruiken tijdens development. MongoExpress beschikt over een UI waar je de Mongo database in kan managen, en waar je documenten kan inzien. Het word niet geadviseerd om express te gebruiken in productie, omdat JavaScript injection mogelijk zou **kunnen** zijn. Voor deze POC maak ik hier wel gebruik van omdat het alleen voor onderzoek doeleinden is, en omdat het makkelijker is om te bevestigen dat bepaalde operaties in mijn code werken.

MongoExpress staat los van de Mongo database. Dit zijn twee aparte containers die met elkaar verbonden moeten worden. Commando's typen in Powershell kan onhandig zijn wanneer je meerdere containers tegelijk moet gebruiken. Om deze reden maak ik gebruik van docker-compose. Voor meer informatie over Docker verwijs ik naar mijn onderzoek: [Research Docker]()

```yml
version: "3.1"

# De services of 'containers' die gestart worden met docker-compose
services:
  # De mongo database met een standaard gebruikersnaam en wachtwoord
  mongo:
    image: mongo
    restart: always
    ports:
      - 27017:27017
    environment:
      MONGO_INITDB_ROOT_USERNAME: root
      MONGO_INITDB_ROOT_PASSWORD: example

  # De mongo express UI die de mongo database in de andere container managed. Is bereikbaar via de browser op: localhost:8081
  mongo-express:
    image: mongo-express
    restart: always
    ports:
      - 8081:8081
    environment:
      ME_CONFIG_MONGODB_ADMINUSERNAME: root
      ME_CONFIG_MONGODB_ADMINPASSWORD: example
```

Om de 2 containers tegelijk te starten gebruik het commando: `docker-compose -f configuratie.yml up`. De -f tag geeft aan dat docker compose een configuratie bestand moet gebruiken (configuratie.yml). Zonder specifiek configuratie bestand zal Docker zoeken naar een 'docker-compose.yml' bestand.

Via de browser is het nu mogelijk om via MongoExpress te communiceren met deze database. Het is mogelijk om handmatig een database aan te maken, maar de Mongo client voor .NET zal er zelf een maken wanneer er geen aanwezig is bij het uitvoeren van code. De standaard URL voor het maken van een connectie naar deze database is `mongodb://localhost:27017` met de standaard poort.

---

### MongoDB Driver met .NET

1. Zorg dat de Mongo client geïnstalleerd is.

Om de package via de command-line te installeren gebruik: `dotnet add package MongoDB.Driver`

Note: Naast het installeren van de Mongo .NET client en het starten van een Mongo container, zijn er nog wat voorbereidingen nodig in de .NET applicatie. In deze POC maak ik gebruik van 2 patterns, namelijk de Repository en Dependency Injection patterns. Voor meer informatie wil ik verwijzen naar mijn onderzoek:

Repository pattern: [Research Repository Pattern]()
Dependency Injection pattern: [Research Dependency Injection Pattern]()

2. Configureren Mongo in .NET

vertel over database connectie client in repository en over extension method die je kan maken

Normaal gesproken voeg je nieuwe services of dependencies toe in startup.cs met services.Add\*. Om in plaats van alle logic in de startup.cs class te schrijven, is het beter om een extension method te maken, zodat je met een simpele statement Mongo support kan toevoegen aan je applicatie. Een extension method voegt functionaliteit toe aan een class zonder de class zelf te wijzigen. Omdat we Mongo support willen toevoegen via de ConfigureService method die om een IServiceCollection interface vraagt, moeten we deze interface gebruiken in onze extension method om functionaliteit te kunnen toevoegen.

---

### Bronnen

Voor het onderzoek naar MongoDB heb ik de officiele documentatie gebruikt:

[Documentation Mongo Container - Docker Hub](https://hub.docker.com/_/mongo)
[Documentation Mongo - MongoDB with .NET](https://www.mongodb.com/blog/post/quick-start-c-sharp-and-mongodb-starting-and-setup)
[Documentation Mongo - What is MongoDB](https://www.mongodb.com/what-is-mongodb)
[Youtube Microsoft - Connecting API to a data-store](https://www.youtube.com/watch?v=jxlKfHGMG-g&list=PLdo4fOcmZ0oVjOKgzsWqdFVvzGL2_d72v&index=11)
