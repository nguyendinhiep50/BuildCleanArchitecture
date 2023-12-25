using BuildCleanArchitecture.Domain.Enities;
using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class Book : BaseEnitites
    {
        [Required]
        public string? Name { get; set; }

        public DateTime? PublicationDate { get; set; }

        public virtual AuthorBook AuthorBooks { get; set; } = null!;

        public virtual ICollection<CatalogBook> CatalogBooks { get; set; } = new List<CatalogBook>();
    }
}
