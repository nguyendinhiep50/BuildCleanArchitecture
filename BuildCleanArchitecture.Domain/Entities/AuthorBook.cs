using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class AuthorBook : BaseEnitites
    {
        [Key]
        public string? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? DateBirth { get; set; }
    }
}
