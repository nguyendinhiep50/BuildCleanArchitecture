using AutoMapper;
using BuildCleanArchitecture.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCleanArchitecture.Application.CatalogBooks.Dtos
{
    public class CatalogBookUpdateDto
    {
        public string? Name { get; set; }

        public DateTime? PublicationDate { get; set; }

    }

    public class CatalogBookAddDto : CatalogBookUpdateDto
    {
        public string? Id { get; set; }
    }

    public class CatalogBookDto : CatalogBookUpdateDto
    {
        [Column("UID")]
        public Guid UId { get; set; }

        [Column("UID")]
        public Book? Books { get; set; }

        [Column("date0")]
        public DateTime? CreatedDate { get; set; }

        [Column("time0")]
        public string? CreatedSpanTime { get; set; }

        [Column("user_id0")]
        public decimal? CreatedBy { get; set; }

        [Column("date2")]
        public DateTime? UpdatedDate { get; set; }

        [Column("time2")]
        public string? UpdatedSpanTime { get; set; }

        [Column("user_id2")]
        public decimal? UpdatedBy { get; set; }
        private class CatalogBookProfile : Profile
        {
            public CatalogBookProfile()
            {
                CreateMap<CatalogBook, CatalogBookDto>().ConvertUsing((entity, dto) =>
                {
                    return new CatalogBookDto
                    {
                        Name = entity.Name,
                        UId = entity.UId,

                        CreatedDate = entity.CreatedDate,
                        CreatedSpanTime = entity.CreatedSpanTime,
                        CreatedBy = entity.CreatedBy,
                        UpdatedDate = entity.UpdatedDate,
                        UpdatedSpanTime = entity.UpdatedSpanTime,
                        UpdatedBy = entity.UpdatedBy
                    };
                });
                CreateMap<CatalogBookDto, CatalogBook>();
                CreateMap<CatalogBookUpdateDto, CatalogBook>();
                CreateMap<CatalogBookAddDto, CatalogBook>();
            }
        }
    }
}