using BuildCleanArchitecture.Domain.Enities;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class Book : BaseEnitites
    {
        public string? Name { get; set; }

        public DateTime? PublicationDate { get; set; }

        public virtual AuthorBook AuthorBooks { get; set; } = null!;

        public virtual ICollection<CatalogBook> CatalogBooks { get; set; } = new List<CatalogBook>();
    }
}
