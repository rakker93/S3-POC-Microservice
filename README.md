# POC Microservice

### Introductie
Dit is mijn repository relevant voor mijn POC (Proof-of-concept) voor het maken van een microservice. De bedoeling van dit project is om een beter beeld te krijgen hoe je een microservice maakt met .NET. Ook het automatiseren van builds en tests behoort tot deze POC. De informatie die voor het behalen van dit doel word gebruikt, zal in een onderzoeks-document verwerkt worden. Voor dit onderzoek word een bepaalde methodiek gebruikt, genaamd DOT Framework.

---

### DOT Framework
DOT Framework (Design Oriented Triangulation Framework) helpt bij het uitvoeren van praktijk gericht onderzoek. Dit onderzoek is gericht op het product die (in dit geval een ICT'er) wil ontwikkelen.

Source: http://ictresearchmethods.nl/The_DOT_Framework

### Wat, Waarom, Hoe?
DOT Framework bestaat uit 3 verschillende niveau's die moeten helpen met de structuur van dit onderzoek. Hierin worden de 'Wat' 'Waarom' en 'Hoe' in beschreven. 

1. Wat (de vraag van het onderzoek)

In mijn persoonlijk project word een microservice architectuur gebruikt. In deze architectuur worden alle groten taken in de back-end verdeelt over zogenoemde services. Elke service heeft zijn eigen verantwoordelijkheid en mag maar een specifieke taak uitvoeren. Microservices zijn een complex onderwerp en er moet rekening gehouden worden met veel verschillende concepten. 

De hoofdvraag: **Hoe maak ik een microservice die gebruik maakt van MongoDB als data-store?**. 

2. Waarom (de reden van het onderzoek)

Om meer te weten te komen over bepaalde concepten heb ik besloten een proof-of-concept te maken die onderwerpen in het klein laat terugkomen. POC's dienen niet alleen als bewijslast, maar ook als geheugensteun om eventueel terug te kijken hoe iets precies werkt. Deze POC zal ook gebruikt worden bij het onderzoeken van ondere hoofdvragen gericht op bijvoorbeeld CI-CD, en Docker.

Probleemverkenning:

Om een beter beeld te krijgen van de complexiteit van dit project heb ik de hoofdvraag opgesplitst in kleinere vragen:

- Wat voor project-template moet ik gebruiken voor een microservice?
- Hoe implementeer ik asynchronous code in mijn microservice?
- Hoe communiceer ik met MongoDB in een .NET applicatie?
- Hoe zet ik een MongoDB server op?
- Hoe gebruik ik dependency injection?
- Hoe documenteer ik mijn microservice endpoints?
- Hoe test ik mijn microservice?
- Hoe dockerize ik mijn microservice?
- Hoe kan ik error-handling op een gebruiksvriendelijke manier toepassen?
- Hoe kan ik gebruik maken van een 'mapper' voor het automatiseren van het mappen naar / van DTO's?

- Probleemverkenning: Ga na wat de complexiteit van het probleem / de vraag is, en bestaat het uit meerdere vragen? Is het een nieuwe vraag, of is er al informatie beschikbaar bijvoorbeeld op internet? Op welk niveau bevind het probleem zich? Is het een vraag die wereld-weid niet bewantwoord is, of is het een individueel probleem?
- 

