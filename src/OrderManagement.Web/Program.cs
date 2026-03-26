using OrderManagement.Web.Services;

// Program.cs e' il punto di ingresso dell'app ASP.NET Core.
// Qui configuriamo i servizi disponibili e il flusso delle richieste HTTP.
var builder = WebApplication.CreateBuilder(args);

// Abilita il pattern MVC classico: controller + view Razor.
builder.Services.AddControllersWithViews();

// Registra un archivio dati in memoria.
// AddSingleton significa: usa una sola istanza per tutta la vita dell'applicazione.
builder.Services.AddSingleton<ProductStore>();

var app = builder.Build();

// Da qui in poi configuriamo la pipeline HTTP, cioe' l'ordine dei middleware
// che elaborano ogni richiesta in ingresso.
if (!app.Environment.IsDevelopment())
{
    // HSTS dice al browser di usare HTTPS per le richieste future.
    app.UseHsts();
}

// Reindirizza automaticamente da HTTP a HTTPS.
app.UseHttpsRedirection();

// Abilita il sistema di routing che collega URL -> controller/action.
app.UseRouting();

// In un progetto con login/ruoli, qui entrerebbe in gioco l'autorizzazione.
app.UseAuthorization();

// Mappa gli asset statici del progetto, come CSS, JS e immagini.
app.MapStaticAssets();

// Route MVC di default:
// /               -> HomeController.Index()
// /Products       -> ProductsController.Index()
// /Products/Edit/3 -> ProductsController.Edit(3)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// Avvia l'app e mette il server in ascolto.
app.Run();
