# MVC Base Demo

Questa repository contiene un progetto **ASP.NET Core MVC** volutamente semplice, pensato per aiutare un principiante a capire davvero come funziona MVC senza introdurre subito database, Entity Framework o architetture complesse.

L'idea e' questa:

- il **Model** rappresenta i dati
- il **Controller** riceve le richieste HTTP
- la **View** genera l'HTML da mostrare all'utente

In questo esempio il dominio e' molto semplice: gestiamo una piccola lista di prodotti in memoria.

## Obiettivo didattico

Questo progetto serve a capire:

- come nasce una richiesta in un browser
- come ASP.NET Core la instrada verso un controller
- come il controller prepara i dati
- come una view Razor mostra quei dati
- come funzionano form, model binding e validazione

## Struttura del progetto

```text
OrderManagement.slnx
README.md
src/
  OrderManagement.Web/
    Controllers/
      HomeController.cs
      ProductsController.cs
    Models/
      Product.cs
    Services/
      ProductStore.cs
    Views/
      Home/
        Index.cshtml
        Privacy.cshtml
      Products/
        Index.cshtml
        Details.cshtml
        Create.cshtml
        Edit.cshtml
        Delete.cshtml
      Shared/
        _Layout.cshtml
        _ProductForm.cshtml
        _ValidationScriptsPartial.cshtml
      _ViewImports.cshtml
      _ViewStart.cshtml
    wwwroot/
      css/
      js/
    Program.cs
    appsettings.json
```

## Come avviare il progetto

Apri il terminale nella cartella della repository ed esegui:

```bash
dotnet run --project src/OrderManagement.Web/OrderManagement.Web.csproj
```

Poi apri nel browser l'indirizzo mostrato dal terminale.

La pagina principale ti porta alla sezione `Prodotti`, che contiene una piccola demo CRUD.

## Cos'e' MVC in pratica

### M = Model

Il model e' la rappresentazione dei dati.

Nel progetto il model principale e':

- `src/OrderManagement.Web/Models/Product.cs`

Questo file descrive un prodotto con proprieta' come:

- `Id`
- `Name`
- `Category`
- `Price`
- `Stock`
- `Description`

Contiene anche regole di validazione tramite **Data Annotations**:

- `[Required]`
- `[StringLength]`
- `[Range]`
- `[Display]`

Quindi il model non serve solo a contenere dati, ma anche a dire quali dati sono validi.

### V = View

La view e' il file Razor `.cshtml` che genera HTML.

Esempi:

- `src/OrderManagement.Web/Views/Products/Index.cshtml`
- `src/OrderManagement.Web/Views/Products/Create.cshtml`
- `src/OrderManagement.Web/Views/Products/Edit.cshtml`
- `src/OrderManagement.Web/Views/Products/Details.cshtml`
- `src/OrderManagement.Web/Views/Products/Delete.cshtml`

Una view puo' essere **fortemente tipizzata**:

```cshtml
@model OrderManagement.Web.Models.Product
```

oppure puo' ricevere una lista:

```cshtml
@model IReadOnlyList<OrderManagement.Web.Models.Product>
```

Questo significa che la view sa esattamente quale tipo di dato ricevera' dal controller.

### C = Controller

Il controller e' la classe che gestisce le richieste.

Nel progetto il controller piu' importante e':

- `src/OrderManagement.Web/Controllers/ProductsController.cs`

Contiene action come:

- `Index()`
- `Details(int id)`
- `Create()`
- `Create(Product product)` con `POST`
- `Edit(int id)`
- `Edit(int id, Product product)` con `POST`
- `Delete(int id)`
- `DeleteConfirmed(int id)` con `POST`

Il controller fa tipicamente queste cose:

1. riceve la richiesta
2. legge o modifica i dati
3. decide quale view restituire
4. passa alla view il model corretto

## Flusso completo di una richiesta

Vediamo il caso piu' semplice: apertura della lista prodotti.

### Caso: utente visita `/Products`

1. Il browser invia una richiesta HTTP.
2. ASP.NET Core usa il routing configurato in `Program.cs`.
3. La richiesta arriva a `ProductsController`.
4. Viene eseguita l'action `Index()`.
5. Il controller legge i dati da `ProductStore`.
6. Il controller restituisce `View(products)`.
7. Razor usa `Views/Products/Index.cshtml`.
8. La pagina HTML finale viene inviata al browser.

In forma molto breve:

```text
Browser -> Routing -> Controller -> Service/Store -> View -> HTML
```

## I file piu' importanti spiegati bene

### 1. `Program.cs`

File:

- `src/OrderManagement.Web/Program.cs`

Questo e' il punto di avvio dell'applicazione.

Cose importanti che fa:

- crea il builder dell'app
- registra i servizi nel container DI
- abilita MVC con `AddControllersWithViews()`
- registra `ProductStore` con `AddSingleton<ProductStore>()`
- configura middleware come routing e HTTPS
- definisce la route di default

Qui c'e' un concetto importante: **Dependency Injection**.

Quando registri:

```csharp
builder.Services.AddSingleton<ProductStore>();
```

stai dicendo al framework:

"Quando a qualcuno serve `ProductStore`, ci penso io a fornirlo."

### 2. `Product.cs`

File:

- `src/OrderManagement.Web/Models/Product.cs`

Questo e' il model.

Aspetti da notare:

- ha proprieta' semplici
- usa validazioni per obbligatorieta' e limiti
- usa `Display` per cambiare il testo mostrato nelle label

Quando un utente invia una form, ASP.NET Core prova automaticamente a costruire un oggetto `Product` usando i dati ricevuti. Questo processo si chiama **model binding**.

### 3. `ProductStore.cs`

File:

- `src/OrderManagement.Web/Services/ProductStore.cs`

Questo file simula un archivio dati.

Non usa un database: contiene una lista in memoria con alcuni prodotti iniziali.

Metodi principali:

- `GetAll()`
- `GetById(int id)`
- `Add(Product product)`
- `Update(Product product)`
- `Delete(int id)`

Dal punto di vista didattico e' utile perche' permette di spiegare il flusso CRUD senza introdurre SQL o Entity Framework.

## Come leggere `ProductsController`

File:

- `src/OrderManagement.Web/Controllers/ProductsController.cs`

Questo controller e' il cuore della demo.

### Costruttore

```csharp
public ProductsController(ProductStore productStore)
{
    _productStore = productStore;
}
```

ASP.NET Core crea il controller e gli passa automaticamente `ProductStore` grazie alla Dependency Injection.

### `Index()`

```csharp
public IActionResult Index()
{
    var products = _productStore.GetAll();
    return View(products);
}
```

Legge tutti i prodotti e li passa alla view.

### `Details(int id)`

Riceve un id dalla route, cerca il prodotto e mostra la relativa view.

Se non trova nulla, restituisce:

```csharp
return NotFound();
```

### `Create()` GET

```csharp
public IActionResult Create()
{
    return View(new Product());
}
```

Mostra la form vuota.

### `Create(Product product)` POST

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Product product)
{
    if (!ModelState.IsValid)
    {
        return View(product);
    }

    _productStore.Add(product);
    return RedirectToAction(nameof(Index));
}
```

Questo e' un punto fondamentale.

Succede questo:

1. il browser invia i campi della form
2. ASP.NET Core costruisce un oggetto `Product`
3. valida il model
4. se non e' valido, la form viene rimostrata con gli errori
5. se e' valido, il prodotto viene aggiunto
6. l'utente viene reindirizzato alla lista

### `Edit()` GET e POST

Funziona come la creazione, ma parte da un prodotto gia' esistente.

### `Delete()` GET e POST

Qui si vede una buona pratica base:

- la `GET` mostra una pagina di conferma
- la `POST` esegue davvero la cancellazione

Questo evita di usare una semplice visita a un link per modificare dati.

## Le view Razor principali

### `Views/Products/Index.cshtml`

Mostra una tabella con tutti i prodotti.

Qui puoi spiegare:

- `@model` per ricevere i dati
- `foreach` per stampare le righe
- tag helper `asp-action` e `asp-route-id`

Esempio:

```cshtml
<a asp-action="Details" asp-route-id="@product.Id">Dettagli</a>
```

Non stai scrivendo manualmente l'URL: lo genera MVC.

### `Views/Products/Create.cshtml`

Mostra la form di creazione.

Usa:

- `form asp-action="Create"`
- `method="post"`
- `<partial name="_ProductForm" model="Model" />`

La partial evita di duplicare la stessa form tra `Create` e `Edit`.

### `Views/Shared/_ProductForm.cshtml`

Questa e' una **partial view**.

Serve a riutilizzare i campi del form in piu' pagine.

Qui puoi spiegare:

- `asp-for` collega gli input alle proprieta' del model
- `asp-validation-for` mostra gli errori di validazione
- `asp-validation-summary` mostra errori generali

Esempio:

```cshtml
<input asp-for="Name" class="form-control" />
<span asp-validation-for="Name" class="text-danger"></span>
```

### `Views/Shared/_Layout.cshtml`

Questo e' il layout comune dell'applicazione.

Contiene:

- struttura HTML generale
- navbar
- footer
- `@RenderBody()` per inserire il contenuto delle singole view

Pensa al layout come alla "cornice" condivisa da tutte le pagine.

### `_ViewStart.cshtml`

File:

- `src/OrderManagement.Web/Views/_ViewStart.cshtml`

Serve a dire a tutte le view quale layout usare:

```cshtml
Layout = "_Layout";
```

### `_ViewImports.cshtml`

Serve a importare namespace e tag helper comuni a tutte le view.

## Routing spiegato semplice

In `Program.cs` c'e' questa route:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

Significa:

- se non specifichi nulla, apre `HomeController` -> `Index`
- l'ultimo parametro `id` e' opzionale

Esempi:

- `/` -> `HomeController.Index()`
- `/Products` -> `ProductsController.Index()`
- `/Products/Details/3` -> `ProductsController.Details(3)`
- `/Products/Edit/2` -> `ProductsController.Edit(2)`

## Form, POST e validazione

Quando invii una form di creazione o modifica, entrano in gioco quattro concetti centrali:

### 1. Model Binding

ASP.NET Core prende i campi della form e prova a costruire un oggetto `Product`.

### 2. Model Validation

Le regole nel model vengono controllate.

Esempio:

- `Name` obbligatorio
- `Price` maggiore di zero

### 3. ModelState

Nel controller puoi controllare:

```csharp
if (!ModelState.IsValid)
{
    return View(product);
}
```

Se ci sono errori, la view viene ricaricata con i messaggi.

### 4. Anti-forgery token

Le action `POST` usano:

```csharp
[ValidateAntiForgeryToken]
```

E le form Razor generano automaticamente il token antifalsificazione.

Questo e' un meccanismo di sicurezza base per evitare richieste malevole.

## Dependency Injection spiegata da principiante

Nel progetto c'e' un solo servizio personalizzato:

- `ProductStore`

Viene registrato in `Program.cs` e poi richiesto nel costruttore del controller.

Questo e' utile per due motivi:

- il controller non crea da solo le sue dipendenze
- il codice e' piu' ordinato e piu' facile da sostituire in futuro

Oggi `ProductStore` usa una lista in memoria.

Domani potresti sostituirlo con:

- Entity Framework
- un database SQL
- una API esterna

senza cambiare l'idea generale del controller.

## Perche' questo progetto non usa ancora un database

Per un principiante e' meglio separare i problemi.

Prima si capisce bene MVC:

- route
- controller
- model
- view
- form
- validazione

Poi si introduce il database.

Se aggiungi subito EF Core, migrazioni e SQL, spesso chi inizia confonde i concetti.

## Come spiegare questo progetto a un amico

Puoi seguire questo ordine:

1. apri `Program.cs` e spiega che e' il punto di partenza
2. apri `Product.cs` e spiega il model
3. apri `ProductStore.cs` e spiega dove stanno i dati
4. apri `ProductsController.cs` e spiega le action
5. apri `Views/Products/Index.cshtml` e mostra come una lista diventa HTML
6. apri `Views/Products/Create.cshtml` e `_ProductForm.cshtml` e spiega la form
7. avvia il progetto e fai una prova completa da browser

## Mini percorso di studio consigliato

### Livello 1: capire la navigazione

Fagli osservare:

- clic su `Prodotti`
- arrivo a `ProductsController.Index()`
- la lista renderizzata in `Views/Products/Index.cshtml`

### Livello 2: capire il passaggio dati

Apri `Index()` del controller e fagli vedere:

```csharp
var products = _productStore.GetAll();
return View(products);
```

Qui il concetto chiave e':

"Il controller recupera i dati e li passa alla view."

### Livello 3: capire la form

Apri `Create.cshtml` e poi la action `Create(Product product)`.

Concetto chiave:

"L'utente compila la form, il framework ricostruisce il model, il controller lo valida."

### Livello 4: capire il CRUD

Spiegagli che il CRUD e':

- `Create` -> crea
- `Read` -> legge
- `Update` -> modifica
- `Delete` -> elimina

In questo progetto:

- `Index` e `Details` sono parte del `Read`
- `Create` crea
- `Edit` aggiorna
- `Delete` elimina

## Esercizi utili per imparare davvero

Se vuoi farlo esercitare, questi sono ottimi passi:

1. Aggiungere il campo `Brand` al model `Product`.
2. Mostrare `Brand` nella lista e nei dettagli.
3. Rendere obbligatoria la descrizione.
4. Aggiungere una pagina con i soli prodotti con stock basso.
5. Ordinare i prodotti per prezzo invece che per nome.
6. Aggiungere un messaggio di successo dopo la creazione.
7. Sostituire `ProductStore` con un database in un secondo momento.

## Differenza tra questo esempio e un progetto reale

In un progetto reale potresti avere:

- database
- Entity Framework Core
- servizi separati
- repository
- autenticazione
- logging
- gestione errori piu' robusta

Questo esempio invece e' volutamente ridotto all'essenziale per capire le basi.

## Domande che un principiante dovrebbe sapersi fare

Se vuoi verificare se ha capito, prova a chiedergli:

1. Chi decide quale view restituire?
2. Dove vengono definite le regole di validazione?
3. Come arriva l'`id` alla action `Details(int id)`?
4. Perche' esistono due action `Create`, una GET e una POST?
5. A cosa serve `_ProductForm.cshtml`?
6. Che differenza c'e' tra `Model` e `ModelState`?

Se sa rispondere bene a queste, ha gia' capito il cuore di MVC.

## Nota sul framework target

Il progetto attualmente e' stato creato dal template installato nella macchina. Se vuoi allinearlo a `.NET 8` in modo esplicito, puoi modificare il `TargetFramework` nel file:

- `src/OrderManagement.Web/OrderManagement.Web.csproj`

portandolo a:

```xml
<TargetFramework>net8.0</TargetFramework>
```

Questo non cambia il modo in cui impari MVC, ma allinea il progetto alla versione richiesta.

## Conclusione

Se vuoi capire MVC davvero, non devi partire da mille cartelle e tecnologie insieme.

Devi capire bene questo ciclo:

1. arriva una richiesta
2. il controller la intercetta
3. recupera o aggiorna dati
4. passa un model alla view
5. la view genera HTML

Questo progetto serve esattamente a fissare questo meccanismo.
