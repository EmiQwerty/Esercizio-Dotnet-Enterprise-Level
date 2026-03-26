using OrderManagement.Web.Models;

namespace OrderManagement.Web.Services;

// Questo servizio simula un piccolo database in memoria.
// E' utile per imparare MVC senza introdurre subito SQL o Entity Framework.
public class ProductStore
{
    // Lista iniziale usata come dati di esempio.
    private readonly List<Product> _products =
    [
        new Product
        {
            Id = 1,
            Name = "Notebook",
            Category = "Elettronica",
            Price = 999.90m,
            Stock = 8,
            Description = "Portatile usato come dato iniziale."
        },
        new Product
        {
            Id = 2,
            Name = "Mouse Wireless",
            Category = "Accessori",
            Price = 24.90m,
            Stock = 32,
            Description = "Esempio semplice per mostrare la lista."
        },
        new Product
        {
            Id = 3,
            Name = "Scrivania",
            Category = "Arredo",
            Price = 179.00m,
            Stock = 4,
            Description = "Prodotto usato per provare dettagli e modifica."
        }
    ];

    // Restituisce tutti i prodotti ordinati per nome.
    public IReadOnlyList<Product> GetAll()
    {
        return _products
            .OrderBy(product => product.Name)
            .ToList();
    }

    // Cerca un prodotto tramite id.
    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    // Assegna un nuovo id progressivo e inserisce il prodotto nella lista.
    public void Add(Product product)
    {
        product.Id = _products.Count == 0 ? 1 : _products.Max(item => item.Id) + 1;
        _products.Add(product);
    }

    // Aggiorna il prodotto esistente copiando i nuovi valori.
    public bool Update(Product product)
    {
        var existingProduct = GetById(product.Id);
        if (existingProduct is null)
        {
            return false;
        }

        existingProduct.Name = product.Name;
        existingProduct.Category = product.Category;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.Description = product.Description;

        return true;
    }

    // Elimina il prodotto dalla lista se esiste.
    public bool Delete(int id)
    {
        var product = GetById(id);
        if (product is null)
        {
            return false;
        }

        _products.Remove(product);
        return true;
    }
}
