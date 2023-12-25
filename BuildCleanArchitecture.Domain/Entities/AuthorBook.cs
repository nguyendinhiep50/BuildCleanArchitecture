using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class AuthorBook : BaseEnitites
    {
        [Required]
        public string? Name { get; set; }

        public DateTime? DateBirth { get; set; }
    }
}
