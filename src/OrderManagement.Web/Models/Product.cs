using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Web.Models;

// Il model rappresenta i dati con cui lavora l'app.
// Qui usiamo anche Data Annotations per definire le regole di validazione.
public class Product
{
    // Identificatore univoco del prodotto.
    public int Id { get; set; }

    // Required obbliga l'utente a compilare il campo.
    // StringLength limita il numero di caratteri.
    [Required(ErrorMessage = "Il nome e' obbligatorio.")]
    [StringLength(100, ErrorMessage = "Il nome non puo' superare i 100 caratteri.")]
    [Display(Name = "Nome")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La categoria e' obbligatoria.")]
    [StringLength(50, ErrorMessage = "La categoria non puo' superare i 50 caratteri.")]
    [Display(Name = "Categoria")]
    public string Category { get; set; } = string.Empty;

    // Range impone un valore minimo e massimo accettabile.
    // DataType aiuta Razor a capire come mostrare il dato.
    [Range(0.01, 99999, ErrorMessage = "Il prezzo deve essere maggiore di zero.")]
    [Display(Name = "Prezzo")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Range(0, 10000, ErrorMessage = "Lo stock deve essere un numero valido.")]
    [Display(Name = "Disponibilita'")]
    public int Stock { get; set; }

    // La descrizione e' opzionale, ma non puo' superare 300 caratteri.
    [StringLength(300, ErrorMessage = "La descrizione non puo' superare i 300 caratteri.")]
    [Display(Name = "Descrizione")]
    public string? Description { get; set; }
}
