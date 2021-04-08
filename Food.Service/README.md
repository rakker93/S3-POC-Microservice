# Food.Service 

[![CI-CD-FoodAPI](https://github.com/rakker93/S3-POC-Microservice/actions/workflows/workflow.yml/badge.svg)](https://github.com/rakker93/S3-POC-Microservice/actions/workflows/workflow.yml)

### Endpoints

Deze service beschikt over volledige CRUD functionaliteit (Create, Read, Update, Delete) voor food-items.

| Method | Route           | Description                                 |
| ------ | --------------- | ------------------------------------------- |
| GET    | /fooditems      | Verkrijg alle food-items                    |
| GET    | /fooditems/{id} | Verkrijg een specifiek item op basis van id |
| POST   | /fooditems      | Maak een nieuw food-item aan                |
| PUT    | /fooditems/{id} | Update een specifiek item op basis van id   |
| DELETE | /fooditems/{id} | Delete een specifiek item op basis van id   |

---

### Beschrijving van de HTTP attributen (verbs)

- GET: Voor het aanvragen van data.
- POST: Stuurt een aanvraag naar de server met een 'body'. Deze data kan dan door de server gebruikt worden of opgeslagen worden.
- DELETE: Stuurt een aanvraag naar de server om een item te verwijderen.
- PUT / PATCH: PUT stuurt een aanvraag naar de server met een 'body'. Wanneer er naar een bestaand item gewezen word, zal deze geupdated worden. PATCH doet hetzelfde maar word gebruikt om een specifieke attribute te updaten. Het verschil tussen PUT en PATCH is dat je met PUT het volledige object moet opsturen, en met PATCH hoef je alleen de attribute te speciferen die je wil updaten. Voorbeeld:

```json
PUT
{
    name: "MyName",
    age: "28"
    hobby: "MyHobby"
}

PATCH
{
    name: "MyName"
}
```
