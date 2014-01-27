-   Inleiding
    -   Groepsleden
    -   Doel

-   Deployment pipeline
-   Configuratie van omgevingen
    -   Algemene server
    -   Web server
    -   Matching servers
    -   Data crawling servers

-   Software componenten
    -   Crawler
    -   Matcher
    -   Front-end
    -   Overige componenten

-   Testrapport
    -   Gebruikte test tools
    -   Unit tests


Inleiding
---------

Dit is de documentatie voor alle vier de onderdelen van project 56 van
de Hogeschool Rotterdam. In dit document word er duidelijk gemaakt hoe
de verschillende onderdelen zijn gebouwd, hoe deze tot stand zijn
gekomen en welke talen en frameworks zijn gebruikt. Daarnaast word de
samenwerking tussen de verschillende onderdelen hier zorgvuldig
beschreven.

### Groepsleden

  Naam                 Studentnummer
  -------------------- ---------------
  Giovanni Paul        08871550
  Erik Schamper        0864951
  Timon van Spronsen   0866142
  Bart Vollebregt      0858692

### Doel

Het doel van de opdrachtgever is geld te verdienen door het gerbuiken
van het programma. Het programma crawled profielen van LinkedIn en
crawled vacatures van diverse websites om deze vervolgens op te slaan in
een database. De profielen en de vacatures in deze database moeten
vervolgens middels een algoritme aan elkaar gematched worden. Ten slotte
moeten deze matches worden weergegeven op een gebruiksvriendelijke
website waar de matches worden weergeveven voor headhunters zodat deze
de bedrijven en personen kunnen worden benaderd door een van de
opdrachtgever zijn medewerkers.

De specifieke eisen per onderdeel worden later in dit document
beschreven.

Deployment pipeline
-------------------

Voor de deployment pipeline wordt er gebruik gemaakt van TeamCity, dit
is een Continuous Integration tool.

Na het ontwikkelen en testen van een nieuwe feature wordt dit gecommit
naar een version control tool, in ons geval is dat Git.\
TeamCity pollt elke 60 seconde of er een update is in een van de Git
repositories, zo ja, dan haalt TeamCity de nieuwe software binnen en
gaat het deze builden, testen en deployen.

De volledige deployment pipeline is te zien in figuur 1.\
![Figuur 1 - Deployment pipeline](https://www.penflip.com/giovannipaul/product-documentatie-5-6/blob/master/images/Deployment_pipeline.png/?raw=true)

Configuratie van omgevingen
---------------------------

### Algemene server

De algemene server wordt gebruikt om de databases, REST API en TeamCity
te hosten.

#### Hardware vereisten

Voor de algemene server is minimale een opslagruimte van 35 GB vereist.\
Daarnaast moet de server voorzien zijn van minimaal 4 GB RAM en een
Intel Xeon processor.

#### Software vereisten

-   Windows Server 2008 R2\
-   .NET Framework 4.5.1\
-   Visual Studio Express 2013 for Web\
-   Web Deploy 3.5\
-   TeamCity 8.0.0\
-   Eve REST API framework 0.2\
-   Python 2.7.6

### Web server

De web server wordt gebruikt om de front-end te hosten.

#### Hardware vereisten

Voor de algemene server is minimale een opslagruimte van 15 GB vereist.\
Daarnaast moet de server voorzien zijn van minimaal 4 GB RAM en een
Intel Xeon processor.

#### Software vereisten

-   Windows Server 2008 R2\
-   .NET Framework 4.5.1\
-   Visual Studio Express 2013 for Web\
-   Web Deploy 3.5\
-   Web Deploy 3.5 for Hosting Servers\
-   TeamCity Build Agent\
-   IIS 7.5

### Matching servers

De matching servers worden gebruikt om LinkedIn profielen te matchen met
vacatures uit de database, hier worden 15 servers voor ingezet om het
proces te versnellen.

#### Hardware vereisten

Voor de algemene server is minimale een opslagruimte van 10 GB vereist.\
Daarnaast moet de server voorzien zijn van minimaal 4 GB RAM en een
Intel Xeon processor.

#### Software vereisten

-   Windows Server 2008 R2\
-   .NET Framework 4.5.1

### Data crawling servers

Deze webservers worden gebruikt om data te crawlen van o.a. LinkedIn,
hier worden 5 servers voor ingezet om het proces te versnellen.

#### Hardware vereisten

Voor de algemene server is minimale een opslagruimte van 5 GB vereist.\
Daarnaast moet de server voorzien zijn van minimaal 1 GB RAM en een 1
GHz X86 processor.

#### Software vereisten

-   Ubuntu Server 12.04 LTS\
-   Python 2.7.6\
-   Pip 1.5.1\
-   Setuptools 1.1.6\
-   OpenSSL 1.0.1e\
-   PyMongo 2.6.3\
-   PyOpenSSL 0.13.1\
-   Scrapy 0.22\
-   Tor binary 0.2.2.39-1\
-   Twisted 13.1.0\
-   Zope.interface 4.0.5

Software componenten
--------------------

Het project bestaat uit een vijftal software componenten.

  Software component   Gebruikte programmeertaal   Gebruikte architecture en/of design pattern(s)
  -------------------- --------------------------- ------------------------------------------------
  Crawler              Python                      Niet van toepassing
  Communicator         C\#                         Adapter, singleton
  Matcher              C\#                         Abstract factory, factory
  Front-end            C\#                         Model View Controller
  Eve REST API         Python                      Niet van toepassing

### Crawler

#### Samenvatting

Het doel van de crawlers is het ophalen van informatie uit de publieke
profielen van linkedin gebruikers. Hierbij maakt de crawler gebruik van
de HTML tags die worden meegegeven aan de klasse. De gegevens van het
profiel worden daarna doorgegeven aan de communicator. De profielen
worden verkregen door de crawler te laten loopen door alle linkedin
profielen. Deze profielen zijn op de volgende pagina verkrijgbaar:
`http://nl.linkedin.com/directory/`. De vacatures worden op dezelfde
manier benaderd. Er wordt een apart script gemaakt voor elke website die
gecrawled moet worden.

#### Uitgebreide werking

Allereerst moeten `Tor` en `Polipo` opgestart worden met een specifieke
configuratie. Daarna wordt scrapy gestart met een argument om te bepalen
welke letters er gecrawled worden. `Scrapy` start ons crawler programma
met dit argument, en de start links worden opgebouwd.\
Bij het binnenhalen van elke link wordt er eerst bepaald op welk niveau
de link zich bevindt. Een niveau is meestal een nieuwe pagina met links.
Een zeldzaam niveau is een zoekpagina. Het belangrijkste niveau is een
profiel zelf.\
Zodra we een profiel gevonden hebben wordt deze door een `parser`
gehaald. Deze `parser` haalt alle belangrijke informatie van de pagina
af, en geeft ons een `python Dictionary` terug, gevuld met al deze
informatie. We bouwen een `profile` object nog iets verder op met o.a.
een URL en een unieke `hash`. Dit alles stuurt `scrapy` naar een
`pipeline`. Een `pipeline` in `scrapy` is een stukje code die bepaald
wat er met een resultaat moet gebeuren. In ons geval moet dit opgeslagen
worden in een database. Onze `pipeline` bestaat dus uit een stukje code
wat ervoor zorgt dat het resultaat opgeslagen wordt in een `mongodb`
database.\
Binnen dit process wordt continue `Tor` gebruikt. Wij gebruiken `Tor` om
te voorkomen dat wij van de LinkedIn website worden verbannen: zodra we
een foutmelding krijgen vragen wij een nieuw IP aan met Tor.

#### Middelen

De crawler is gemaakt door middel van de scripttaal `Python` met het
framework `Scrapy`. Wij hebben hiervoor gekozen omdat `scrapy` een zeer
uitgebreid en snel framework is waar veel support voor wordt aangeboden
op het internet.\
De crawler draait via `Tor` en `Polipo`. `Tor` is een project om anoniem
op het web te surfen. `Polipo` is proxy software waarmee wij
`HTTP requests` kunnen maken via `Tor` (`Tor` ondersteund standaard
alleen `SOCKS`).\
Alle resultaten worden uiteindelijk opgeslagen in een database. Hiervoor
gebruiken wij `mongodb`.

### Matcher

#### Samenvatting

De matcher zorgt ervoor dat de profielen en vacatures die bestaan in de
database worden opgehaald en door middel van een algoritme worden
gematched. De gegevens van de profielen worden per 25 opgehaald en
vervolgens in een queue geplaatst. Daarnaast worden alle gegevens van de
vacatures opgehaald. Door middel van een dubbele for-loop wordt er per
profiel over elke vacature een algoritme uitgevoerd. Dit algoritme zorgt
ervoor dat elke persoon-vacature vergelijking een score krijgt tussen de
0 en 100. Wanneer dit getal hoger is dan 0, wordt er een match
aangemaakt en teruggestuurd naar de REST communicator.

#### Uitgebreide werking

De `MatcherCommander` word aangemaakt bij het laden van het programma.
Het programma heeft door middel van de `TextAnalyser` class een lijst
met `tags` opgehaald van de Stackoverflow API die later in het programma
essentieel gaat zijn voor de werking van het algoritme. Daarnaast word
er een instantie aangemaakt van het type
`ConcurrentDictionary<Matcher, Thread>` genaamd `ThreadStack` die een
overzicht bewaard van alle Matchers die momenteel aan het Matchen zijn.
Het programma is daarna volledig geladen en in staat om meerdere
instanties van een `Matcher` aan te maken.

Bij het opstarten van een `Matcher` word eerst de `SpawnMatcher()`
method aangeroepen. Deze maakt een nieuwe instantie van een `Thread` en
geeft deze mee aan de Matcher. Daarna worden beide instanties opgeslagen
in de `ThreadStack`. Elke thread kan op elk moment worden gestopt door
de methodes `FinishMatcher` of `StopMatcher` aan te roepen.

Indien de `_isRunning` variabele van de `Matcher` instantie true is,
word er een lijst van `Profiles` opgehaald en de volledige lijst van
`Vacancies`. Daarna zal er door middel van een dubbele for-loop elk
`Profile` worden vergelijken met elke `Vacancy`. Uit de `Profile` worden
vervolgens alle gegevens opgehaald over een bepaald persoon.

De `Profile.experiences`, `Profile.interests`, `Profile.specialities` en
`Profile.honors` worden opgehaald van een `Profile` en de inhoud word
vergeleken met elk woord in de `tags` lijst. Indien er een woord uit de
Stackoverflow tag lijst identiek is aan een woord binnen een van
bovengenoemde variabelen word deze toegevoegd aan de
`matchingWordsProfile`. Hetzelfde gebeurt met de `Vacancy` en al haar
beschrijvingen (`Vacancy.details`, `Vacancy.title` en `Vacancy.company`)
en word opgeslagen aan een `matchingWordsVacancy`.

Door middel van de `CompareLists` methode in `TextAnalyser` worden beide
matchingWords lijsten vergeleken en alle unieke woorden worden
ge-returned in een nieuwe lijst `matchingWordsCompared`. Ten slotte word
de hoeveelheid woorden in `matchingWordsCompared` gedeeld door de
hoeveelheid woorden in `matchinWordsVacancy`. Hieruit word de voorlopige
score bepaald. De rest van de score is afhankelijk van welk algoritme je
gebruikt. Bijvoorbeeld `ExperienceAlgorithm` word de tijd dat je ergens
gewerkt hebt ook meegerekend in de uiteindelijke score.

Als de score boven de 0 is, word er een `Match` aangemaakt met de
`SaveMatch` methode en naar de database verstuurd via de communicator.

#### Middelen

De gehele matcher is gemaakt door middel van de programmeertaal C\# met
het .NET framework. Om verbinding te maken met de database word er
gebruik gemaakt van onze zelfgemaakte REST communicator. Ten slotte word
de stackoverflow API geintegreerd in ons programma om de lijst met de
meest actuele tags bij te werken.

### Front-end

#### Samenvatting

De front-end is gemaakt voor de headhunters, de headhunters kunnen hier
alle profielen en vacatures die in de database zijn opgenomen bekijken.\
Ook kunnen zij de matches die door het matchingsysteem zijn gemaakt
bekijken.\
De front-end is opgenomen in de continuous delivery pipeline, dit houd
in dat bij een nieuwe commit op de Git repository, het project
automatisch wordt gebuild en deployed naar de webserver.

#### Uitgebreide werking

De front-end maakt gebruikt van de communicator om de profielen,
vacatures en matches op te halen uit de database door middel van de REST
API.\
De methodes die daarvoor gebruikt worden zijn:\
- pc.GetProfiles(pageNo);\
- pc.GetVacancies(pageNo);\
- pc.GetMatches(pageNo);

#### Middelen

De front-end is gemaakt met in C\# met ASP.NET en het MVC 5 framework.
De front-end maakt net als de matcher gebruik van de communicator om de
REST API te bereiken en alle nodige informatie uit de database te halen.

### Overige componenten

#### Eve REST API

De Eve REST API zorgt voor de aanlevering van data uit de mongoDB
database. Deze data wordt aangeleverd in JSON formaat en is bereikbaar
vanuit alle andere onderdelen van het systeem. Om data op te kunnen
halen via de API moet het onderdeel dat de request maakt zich
Authenticeren. Dit gebeurd door middel van HTPP Basic Authentication. Er
is een usernname en wachtwoord vereist welke vervolgens moeten deze
gebruikersnaam en wachtwoord worden gecodeerd door middel van Base64.
Naast het ophalen van data is het via de API ook mogelijk om data op te
slaag, aan te passen en te verwijderen. Deze REST API biedt de
mogelijkheid om eenvoudiger uit te breiden naar andere platformen.

Testrapport
-----------

### Gebruikte test tools

VSTS

### Unit tests

Er zijn vele verschillende unit testen gemaakt om het matchen van
profielen en vacatures te testen. De volgende Unit tests worden
uitgevoerd:\
- Matchen door alle Algoritmes met een leeg profiel en een lege
vacature\
- Matchen door alle Algoritmes met een vol profiel en een lege vacature\
- Matchen door alle Algoritmes met een vol profiel en een volle
vacature\
- Matchen door alle Algoritmes met een profiel die alleen experiences
bezit en een volle vacature\
- Matchen door alle Algoritmes met een profiel die alleen skills bezit
en een volle vacature\
- Matchen door alle Algoritmes met een profiel die alleen languages
bezit en een volle vacature\
- Matchen door alle Algoritmes met een profiel die alleen honours bezit
en een volle vacature. Deze is speciaal aangezien honours worden
meegerekend in het experiencealgoritme. Zelfs wanneer er geen
experiences zijn.
