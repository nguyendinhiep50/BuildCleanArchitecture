using AutoMapper;
using BuildCleanArchitecture.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCleanArchitecture.Application.Books.Dtos
{
    public class BookUpdateDto
    {

    }

    public class BookAddDto : BookUpdateDto
    {

    }

    public class BookDto : BookUpdateDto
    {
        [Column("UID")]
        public Guid UId { get; set; }

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
        private class BookProfile : Profile
        {
            public BookProfile()
            {
                CreateMap<Book, BookDto>().ConvertUsing((entity, dto) =>
                {
                    return new BookDto
                    {
                        Id = entity.BookCode?.Trim() ?? string.Empty,
                        BookName = entity.BookName?.Trim(),
                        AccountContribute = entity.AccountContribute?.Trim(),
                        Group02 = entity.Group02?.Trim(),
                        Group01 = entity.Group01?.Trim(),
                        SurplusGapType = entity.SurplusGapType,
                        CreatedDate = entity.CreatedDate,
                        CreatedSpanTime = entity.CreatedSpanTime,
                        CreatedBy = entity.CreatedBy,
                        UpdatedDate = entity.UpdatedDate,
                        UpdatedSpanTime = entity.UpdatedSpanTime,
                        UpdatedBy = entity.UpdatedBy
                    };
                });
                CreateMap<BookDto, Book>();
                CreateMap<BookUpdateDto, Book>();
                CreateMap<BookAddDto, Book>();
            }
        }

    }
}
