using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class Book : BaseEnitites
    {
        [Key]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? PublicationDate { get; set; }

        public virtual AuthorBook AuthorBook { get; set; } = null!;

        public virtual CatalogBook CatalogBook { get; set; } = null!;
    }
}
