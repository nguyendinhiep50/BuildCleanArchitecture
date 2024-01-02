using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class CatalogBook : BaseEnitites
    {
        [Key]
        public string? Id { get; set; }

        public string? Name { get; set; }
    }
}
