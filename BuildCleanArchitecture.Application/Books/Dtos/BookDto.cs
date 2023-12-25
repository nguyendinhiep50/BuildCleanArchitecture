using AutoMapper;
using BuildCleanArchitecture.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildCleanArchitecture.Application.Books.Dtos
{
    public class BookUpdateDto
    {
        public string? Name { get; set; }

        public DateTime? PublicationDate { get; set; }

        public virtual AuthorBook? AuthorBooks { get; set; } = null!;
    }

    public class BookAddDto : BookUpdateDto
    {
        public string? Id { get; set; }
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
                        Name = entity.Name,
                        AuthorBooks = dto.AuthorBooks,
                        PublicationDate = dto.PublicationDate,
                        UId = dto.UId,

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
