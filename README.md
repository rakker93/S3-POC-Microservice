# Introductie

Dit is mijn repository relevant voor mijn POC (Proof-of-concept) voor het maken van een microservice. De bedoeling van dit project is om een beter beeld te krijgen hoe je een microservice maakt met .NET. Ook het automatiseren van builds en tests behoort tot deze POC. De informatie die voor het behalen van dit doel word gebruikt, zal in een onderzoeks-document verwerkt worden. Voor dit onderzoek word een bepaalde methodiek gebruikt, genaamd DOT Framework.

# Table of Content

- Test
- Test2
- Test3

# DOT Framework

![dotframework](https://user-images.githubusercontent.com/60918040/110812513-f6569f80-8287-11eb-82c8-2236eff7ad06.jpg)

DOT Framework (Design Oriented Triangulation Framework) helpt bij het uitvoeren van praktijk gericht onderzoek. Dit onderzoek is gericht op het product die (in dit geval een ICT'er) wil ontwikkelen.

Source: [DOT Framework](http://ictresearchmethods.nl/The_DOT_Framework)

# Wat, Waarom, Hoe?

DOT Framework bestaat uit 3 verschillende niveau's die moeten helpen met de structuur van dit onderzoek. Hierin worden de 'Wat' 'Waarom' en 'Hoe' in beschreven.

### Wat (de vraag van het onderzoek)

In mijn persoonlijk project word een microservice architectuur gebruikt. In deze architectuur worden alle groten taken in de back-end verdeelt over zogenoemde services. Elke service heeft zijn eigen verantwoordelijkheid en mag maar een specifieke taak uitvoeren. Microservices zijn een complex onderwerp en er moet rekening gehouden worden met veel verschillende concepten. Hier is een algemene onderzoeksvraag uit ontstaan waarop het onderzoek is gebaseerd:

**Hoe maak ik een microservice die gebruik maakt van MongoDB als data-store?**

---

### Waarom (de reden van het onderzoek)

Om meer te weten te komen over bepaalde concepten heb ik besloten een proof-of-concept te maken die onderwerpen in het klein laat terugkomen. POC's dienen niet alleen als bewijslast, maar ook als geheugensteun om eventueel terug te kijken hoe iets precies werkt. Deze POC zal ook gebruikt worden bij het onderzoeken van andere hoofdvragen gericht op CI-CD, en Docker.

Wat lost het onderzoek op?

In voorgaande semesters hebben we systemen gemaakt waar alle server-side functies zich bevinden in een (en dezelfde) webserver. Dit word ook wel een monolith architectuur genoemd. Dit gaat ook vaak samen met een database en wanneer de applicatie deployed word dan deploy je alles in een keer. Voordelen en nadelen van een monolith type applicatie zijn:

| Pros                                                                    | Cons                                                                                              |
| ----------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------- |
| Makkelijk te integreren bij nieuwe projecten                            | Kan te complex worden, wanneer het project groter word                                            |
| Makkelijk om code te hergebruiken, omdat alles het zelfde project leeft | Mergen van code kan lastig zijn                                                                   |
| Makkelijker om applicatie lokaal uit te voeren                          | Geen isolatie tussen modules en kan lastig op te schalen zijn                                     |
| Makkelijker te debuggen                                                 | Langzame builds en deployments                                                                    |
| Makkelijker te integreren met CI/CD, omdat er maar 1 pipeline nodig is  | Rollback naar oudere versie bij een klein probleem zorgt ervoor dat de hele applicatie terug moet |

Microservices is een architectuur waar een applicatie gestructureerd word door middel van zelfstandig inzetbare services. Deze services worden vaak beheerd door kleinere teams die ieder verantwoordelijk zijn voor een service. Een service heeft zijn eigen verantwoordelijkheid, en in veel gevallen ook een eigen data-store. Voordelen en nadelen van een microservice type architectuur zijn:

| Pros                                                                       | Cons                                                                        |
| -------------------------------------------------------------------------- | --------------------------------------------------------------------------- |
| Makkelijker te managen ivm kleinere code base                              | Moeilijker te begrijpen ivm complexiteit                                    |
| Snellere builds                                                            | Code dat door meer services gebruikt word, is opgedeelt in aparte libraries |
| Onafhankelijk, sneller te deployen                                         | Kan moeilijker zijn om te debuggen / troubleshooten                         |
| Geisoleerd van falen. Als een service faalt, faalt niet de hele applicatie |                                                                             |
| Meer flexibel in wijzigingen code                                          |                                                                             |
| Makkelijker te managen voor teams                                          |                                                                             |

Probleemverkenning:

Om een beter beeld te krijgen van de complexiteit van dit project heb ik de hoofdvraag opgesplitst in kleinere vragen:

- Hoe implementeer ik asynchronous code in mijn microservice?
- Hoe communiceer ik met MongoDB in een .NET applicatie?
- Hoe zet ik een MongoDB server op?
- Hoe gebruik ik dependency injection?
- Hoe documenteer ik mijn microservice endpoints?
- Hoe test ik mijn microservice?
- Hoe dockerize ik mijn microservice?
- Hoe kan ik error-handling op een gebruiksvriendelijke manier toepassen?
- Hoe kan ik gebruik maken van een 'mapper' voor het automatiseren van het mappen naar / van DTO's?
- Hoe implementeer ik de repository pattern in mijn project?

Dit zijn de deelvragen die uiteindelijk moeten helpen om de hoofdvraag te kunnen beantwoorden. De vragen die ik hier stel zullen niet allemaal relevant voor een minimale POC, maar zijn wel onderwerpen die goed samengaan bij het maken van een service. Deelvragen kunnen ook uit kleinere vragen bestaan, die ik beschrijf in een appart .md document.

Probleem-niveau:

Het probleem-niveau ligt op het individu. Alle informatie die ik nodig heb voor het maken van een service met .NET is vindbaar op het internet.

Informatiebronnen:

Ik gebruik .NET voor het maken van mijn microservices, dus ik kan de Microsoft documentatie gebruiken. Ook heeft Microsoft verschillende tutorials voor het maken van een basis microservice. Om een breder perspectief te krijgen gebruik ik youtube en eventueel stackoverflow om extra informatie in te winnen. Microsoft is in dit geval mijn 'source of truth'. Wanneer er bij een andere bron over nieuwe concepten word gesproken, dan verifieer ik dit door het ook op te zoeken in de Microsoft documentatie of een Microsoft gerelateerd artikel. Een kleine zoekopdracht heeft de volgende bronnen opgeleverd:

1. Documentatie
   [Microsoft - .NET REST API](https://dotnet.microsoft.com/apps/aspnet/apis) |
   [Microsoft - Example Repository Pattern](https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application) |
   [C-Sharp Corner - Getting started with the Repository Pattern](https://www.c-sharpcorner.com/UploadFile/b1df45/getting-started-with-repository-pattern-using-C-Sharp/)

2. Video
   [Microsoft - Your first Microsoft](https://dotnet.microsoft.com/learn/aspnet/microservice-tutorial/intro) |
   [Microsoft - Beginner's Series to Web API's](https://www.youtube.com/watch?v=h0KG8OKKgKs&list=PLdo4fOcmZ0oVjOKgzsWqdFVvzGL2_d72v) |
   [Les Jackson / Youtube - .NET Rest API](https://www.youtube.com/watch?v=fmvcAzHpsk8)

3. Discussions (forum)
   [StackOverflow - Repository Pattern](https://stackoverflow.com/questions/11985736/repository-pattern-step-by-step-explanation)

---

### Hoe (onderzoek strategieën en methoden)

DOT Framework heeft 5 verschillende strategieën over de aanpak van onderzoek:

![library](https://user-images.githubusercontent.com/60918040/110812993-65cc8f00-8288-11eb-82ee-d0a44300ec30.png)
- Bieb: Onderzoek naar wat al bestaat of al ooit gedaan is. Je gaat na welke theorieen er zijn over je onderwerp die in de context van je eigen project passen. Je zoekt informatie op of gebruikt bijvoorbeeld documentatie die door andere mensen geschreven is.

![field](https://user-images.githubusercontent.com/60918040/110813000-66652580-8288-11eb-925d-f92d6b1b4894.png)
- Veld: Je gebruikt een veld-strategie om een beter inzicht te krijgen wat de eindgebruikers van jouw product verwachten en wat ze hier mee willen bereiken, hoe ze het gebruiken etc.

![lab](https://user-images.githubusercontent.com/60918040/110812997-66652580-8288-11eb-9f50-14c7b60e94b1.png)
- Lab: Test je product door gebruik te maken van verschillende test methodieken die aangegeven worden in de lab strategie. Security, System, Unit testen maar ook statistische testen zijn allemaal methoden die je kunt gebruiken hiervoor.

![showroom](https://user-images.githubusercontent.com/60918040/110812995-65cc8f00-8288-11eb-8d14-a1405531641e.png)
- Showroom: Laat de "positie" van jouw product zien tegenover die van andere. Je kunt je product laten zien aan experts of beschrijven wat er anders is aan jouw product ten opzichte van andere.

![Logo-workshop](https://user-images.githubusercontent.com/60918040/110812989-6533f880-8288-11eb-92e2-014256d1d29e.png)
- Workshop: Je gaat na wat je mogelijkheden zijn door middel van prototyping (POC), design en eventueel tutorials volgen. Je doet dit om inzicht te krijgen in de mogelijkheden in context van jouw product.

Voor de methoden die bij elke strategie hoort gebruik ik de volgende bron: [DOT Framework Methods](http://ictresearchmethods.nl/Methods)

Omdat dit project een klein POC is, waar vooral veel informatie ingewonnen word van bestaande bronnen, heb ik gekozen om de volgende strategieën en methoden te gebruiken voor het beantwoorden van de hoofd en deelvragen:

![combi](https://user-images.githubusercontent.com/60918040/110813680-04f18680-8289-11eb-8a8d-8dced7a2ccb5.png)
