using System.ComponentModel.DataAnnotations;

namespace BuildCleanArchitecture.Domain.Entities
{
    public class BaseEnitites
    {
        public string? Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? CreatedSpanTime { get; set; }

        public decimal? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedSpanTime { get; set; }

        public decimal? UpdatedBy { get; set; }

        public Boolean Status { get; set; }

        public Guid UId { get; set; }

    }
}
