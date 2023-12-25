using BuildCleanArchitecture.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Enities
{
    public class CatalogBook : BaseEnitites
    {
        [Key]
        public string? Id { get; set; }

        public string? Name { get; set; }
    }
}
