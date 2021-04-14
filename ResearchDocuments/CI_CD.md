# Hoe kan ik continous integration / delivery toepassen op mijn microservice?

### Wat is CI / CD?

CI / CD is een methode om regelmatig applicaties uit te geven op een geautomatiseerde manier. Het is een oplossing voor problemen die komen kijken bij het integreren van nieuwe code. CI / CD gaat over de automatiseren en monitoren van een applicatie gedurende de ontwikkeling en ontwikkeling na release. Je kunt dit zien als een bepaald proces, waarin verschillende stappen zitten om uiteindelijk tot het uitgeven van je applicatie te komen (release).

Een proces zoals dit word ook wel een `pipeline` genoemd. Een pipeline kan compleet aangepast worden aan de requirements van de applicatie. Een aantal stappen die in een pipeline kunnen voorkomen zijn:

- Code Quality Analysis
- Geautomatiseerd builden en testen
- Geautomatiseerd pushen van de applicatie naar een repository
- Het containerizen van applicaties
- Het deployen van applicaties

Dit zijn een aantal voorbeelden die standaard kunnen voorkomen in een normale pipeline. Maar wat zijn de verschillen tussen CI en CD? Die heb ik op een rij gezet:

- Continous Integration

CI is een geautomatiseerd proces voornamelijk voor developers. Een goede CI pipeline zal bij veranderingen in code de applicatie builden, testen en samenvoegen (wanneer je branches gebruikt). Het is een goede oplossing voor wanneer er veel branches aanwezig zijn tijdens de ontwikkeling, die dan met elkaar zouden kunnen conflicteren.

- Continous delivery / deployment

CD is een geautomatiseerd proces die verantwoordelijk is voor alles wat na CI komt. Wanneer er over 'delivery' gesproken word, betekent dit dat veranderingen in de applicatie getest word omvervolgens geupload te worden naar een repository (GitHub) of een container registry (Github Packages of Docker Hub). Normaal gesproken deployed een andere team deze repository / container image in productie.

Wanneer er over 'deployment' word gesproken, dan zal het release proces (het gebruiken van de repository / docker image) ook meegenomen worden in de geautomatiseerde pipeline.

### CI / CD Met GitHub Actions

Er zijn verschillende tools die je kunt gebruiken voor CI / CD, zoals:

- GitHub Actions
- Azure DevOps
- Jenkins
- CircleCI
- GitLab

Voor mijn POC heb ik gekozen voor GitHub Actions. De reden hiervoor is omdat er goede documentatie is, veel voorbeelden, het is makkelijk te gebruiken en direct uitvoerbaar in je repository, oneindig aantal minuten voor je workflows, toegang tot github packages om je artifacts of builds te uploaden etc.

**Note:** GitHub gebruikt het woord 'workflow' in plaats van pipeline, maar het betekent in principe hetzelfde. Workflows bestaan uit jobs die uitgevoerd worden door een agent of runner die door GitHub gehost worden. Vervolgens bestaat een job uit een aantal stappen die gebruik maken van actions die door GitHub aangeboden worden, of third-party actions die je van de marketplace kan halen (gratis of betaald).

Voor deze POC heb ik de onderstaande workflow gemaakt. Workflows behoren in een .yaml bestand gemaakt te worden, en vervolgens in een .github/workflows map geplaatst te worden. GitHub zal de workflows die zich in deze map bevinden automatisch detecteren. In deze documentatie heb ik de workflow opgesplitst in korte stukken zodat er per onderdeel uitleg gegeven kan worden:

```yaml
name: CI-CD-FoodAPI

on:
  workflow_dispatch:

env:
  SOLUTION_PATH: ./Food.Service/FoodAPI.sln
```

In het eerste stuk van je workflow specificeer je normaal de workflow naam en de events waar je de workflow op wil laten triggeren. Normaal gesproken trigger je je workflow bij het pushen aar een branch, pull request, of mergen. In dit geval gebruik ik `workflow_dispatch`, wat betekend dat er een knop beschikbaar word gesteld in GitHub om de workflow handmatig te triggeren. Dit heb ik gedaan om de logs schoon te houden en om overzicht te bewaren tijdens het leren.

Onder het stuk `env` specificeer je environment variables die gebruikt gaan worden in de rest van de flow. In dit geval gebruik ik dit om duplicatie van bepaalde commando's te reduceren. SOLUTION_PATH zal later gebruikt worden voor het builden en testen, omdat hier de solution (.sln) file voor nodig is. Om in je workflow te refereren naar een environment variable gebruik je de `${{ env.SOLUTION_PATH }}` syntax.

```yaml
jobs:
  codeql-analysis:
    name: static-code checking
    runs-on: ubuntu-latest
    strategy:
      fail-fast: true
      matrix:
        language: ["csharp"]

    steps:
      - name: Checkout Repository
        uses: actions/checkout@v2

      - name: Init CodeQL
        uses: github/codeql-action/init@v1
        with:
          languages: ${{ matrix.language }}

      - name: Autobuild
        uses: github/codeql-action/autobuild@v1

      - name: Perform CodeQL Analysis
        uses: github/codeql-action/analyze@v1
```

De code hierboven bestaat uit de eerste job die verantwoordelijk is voor de static code analysis. De code die zich in de repository bevind word gecontrolleerd op standaard fouten, zoals het direct opslaan van een wachtwoord in je code. Wanneer er problemen gevonden worden in je code, krijg je hier een bericht van in je email. Ook zal de rest van de workflow falen en zullen jobs die later aan de beurt zijn niet uitgevoerd worden.

- `name:` De naam van de job
- `runs-on:` Type systeem waarop de workflow word uitgevoerd
- `strategy:` Github cancelt alle jobs wanneer een stap in een matrix faalt
- `matrix:` Specificeert een set van configuraties (in dit geval alleen language) voor het specifieren van de coding language waar de static code analysis op zal draaien
- `steps:` De verschillende stappen in een job

Zoals je onder 'steps' kan zien, word er onder `uses` een path gebruikt. Dit zijn de vooraf gedefineerde actions van GitHub. Dit heeft verder geen configuratie nodig. Voor meer informatie over deze actions kun je terecht bij de marketplace.

### Bronnen

- [What is CI / CD](https://www.redhat.com/en/topics/devops/what-is-ci-cd)
- [GitHub Actions Docs](https://docs.github.com/en/actions/learn-github-actions)
- [Build a CI / CD pipeline from scratch](https://www.youtube.com/watch?v=br48WIwhk2o)
