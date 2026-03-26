using Microsoft.AspNetCore.Mvc;
using OrderManagement.Web.Models;
using OrderManagement.Web.Services;

namespace OrderManagement.Web.Controllers;

// Questo controller mostra il cuore del pattern MVC:
// riceve la richiesta, usa un servizio per i dati e sceglie quale view restituire.
public class ProductsController : Controller
{
    private readonly ProductStore _productStore;

    // Il ProductStore arriva tramite Dependency Injection.
    // Il controller non crea da solo la dipendenza: la riceve gia' pronta.
    public ProductsController(ProductStore productStore)
    {
        _productStore = productStore;
    }

    // GET /Products
    // Recupera tutti i prodotti e li passa alla view Index.
    public IActionResult Index()
    {
        var products = _productStore.GetAll();
        return View(products);
    }

    // GET /Products/Details/5
    // Mostra un singolo prodotto identificato dall'id presente nell'URL.
    public IActionResult Details(int id)
    {
        var product = _productStore.GetById(id);
        if (product is null)
        {
            // Restituisce 404 se il prodotto richiesto non esiste.
            return NotFound();
        }

        return View(product);
    }

    // GET /Products/Create
    // Mostra la form vuota per creare un nuovo prodotto.
    public IActionResult Create()
    {
        return View(new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // POST /Products/Create
    // Riceve i dati della form, li valida e salva il nuovo prodotto.
    public IActionResult Create(Product product)
    {
        // ModelState contiene l'esito delle validazioni definite nel model.
        if (!ModelState.IsValid)
        {
            // Se i dati non sono validi, la form viene mostrata di nuovo
            // mantenendo valori inseriti e messaggi di errore.
            return View(product);
        }

        _productStore.Add(product);

        // Dopo una POST riuscita usiamo il redirect per evitare reinvii accidentali.
        return RedirectToAction(nameof(Index));
    }

    // GET /Products/Edit/5
    // Carica un prodotto esistente e riempie la form di modifica.
    public IActionResult Edit(int id)
    {
        var product = _productStore.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    // POST /Products/Edit/5
    // Aggiorna il prodotto dopo il submit della form.
    public IActionResult Edit(int id, Product product)
    {
        // Controllo di sicurezza: l'id nella route deve coincidere con quello del model.
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(product);
        }

        var updated = _productStore.Update(product);
        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET /Products/Delete/5
    // Mostra una pagina di conferma prima di cancellare davvero il record.
    public IActionResult Delete(int id)
    {
        var product = _productStore.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    // POST /Products/Delete/5
    // Esegue la cancellazione reale dopo la conferma dell'utente.
    public IActionResult DeleteConfirmed(int id)
    {
        var deleted = _productStore.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}
