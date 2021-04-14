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

```yaml
unit-and-integration-tests:
  name: testing solution
  runs-on: ubuntu-latest
  needs: codeql-analysis

  steps:
    - name: Checkout Repository
      uses: actions/checkout@v2

    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 5.0.x

    - name: Restore Dependencies
      run: dotnet restore ${{ env.SOLUTION_PATH }}

    - name: Build Solution
      run: dotnet build --no-restore ${{ env.SOLUTION_PATH }}

    - name: Run tests
      run: dotnet test --no-build --verbosity normal ${{ env.SOLUTION_PATH }}
```

De volgende job is het automatiseren van de unit en integration tests. Dit zijn twee aparte projecten die gedefineerd staan in de .sln project file. De actions spreken in principe voor zichzelf. .NET word opgezet voor de runner die de workflow gaat uitvoeren, de dependencies worden restored, en uiteindelijk zal hij gaan testen. Je kunt zien dat bij de commando's met 'dotnet' ik de environment variable gebruik die aan het begin is gedefineerd. Dit is de path van het .sln bestand.

**Note:** De instructie `needs: codeql-analysis` betekend dat deze job alleen uitgevoerd word wanneer de code-analysis job succesvol is afgerond.

```yaml
build-docker-container:
  name: docker build
  runs-on: ubuntu-latest
  needs: unit-and-integration-tests
  steps:
    - name: Build and Push Docker image
      uses: docker/build-push-action@v2
      with:
        username: ${{ github.actor }}
        password: ${{ secrets.GITHUB_TOKEN }}
        registry: docker.pkg.github.com
        repository: ${{ github.repository }}/foodcontainer
        tags: latest, ${{ github.run_number }}
```

Deze laatste job is verantwoordelijk voor het builden van een Docker image, om deze vervolgens te pushen naar GitHub packages. Wanneer er een package gepushed word met jouw dockerized applicatie, zal dit er zo uitzien:

![asdfa](https://user-images.githubusercontent.com/60918040/114735597-a31fb300-9d45-11eb-8ac3-aac1c0f1abaf.png)

Deze job is afhankelijk van de test job die moet slagen. Zoals je ziet gebruik ik ook hier weer een action die van de marketplace komt. Voor het pushen van een docker image gebruik je de standaard credentials van je account. Deze informatie kun je verkrijgen door middel van de GitHub context.

- github.actor: De login van de gebruiker die de workflow gestart heeft
- github.repository: De eigenaar en repository naam (rakker93/S3-POC-Microservice)
- github.run_number: Uniek nummer voor elke run van de workflow.

En voor de token kun je de gereserveerde environment variable (secret) gebruiken. Hiervoor gebruik je de 'secret' prefix. `secret.GITHUB_TOKEN`.

### Bronnen

- [What is CI / CD](https://www.redhat.com/en/topics/devops/what-is-ci-cd)
- [GitHub Actions Docs](https://docs.github.com/en/actions/learn-github-actions)
- [Build a CI / CD pipeline from scratch](https://www.youtube.com/watch?v=br48WIwhk2o)
- [GitHub context](https://docs.github.com/en/actions/reference/context-and-expression-syntax-for-github-actions#contexts)
- [Github environment variables](https://docs.github.com/en/actions/reference/environment-variables)
