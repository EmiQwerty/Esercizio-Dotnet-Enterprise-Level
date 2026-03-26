using Microsoft.AspNetCore.Mvc;
using OrderManagement.Web.Models;
using OrderManagement.Web.Services;

namespace OrderManagement.Web.Controllers;

public class ProductsController : Controller
{
    private readonly ProductStore _productStore;

    public ProductsController(ProductStore productStore)
    {
        _productStore = productStore;
    }

    public IActionResult Index()
    {
        var products = _productStore.GetAll();
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var product = _productStore.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    public IActionResult Create()
    {
        return View(new Product());
    }

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
    public IActionResult Edit(int id, Product product)
    {
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
