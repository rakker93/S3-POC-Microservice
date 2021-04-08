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

