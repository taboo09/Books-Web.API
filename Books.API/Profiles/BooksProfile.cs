using System.Collections.Generic;
using AutoMapper;
using Books.API.Models;

namespace Books.API.Profiles
{
    public class BooksProfile :  Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Models.Book>()
                .ForMember(x => x.Author, y => y.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<Models.BookForCreation, Entities.Book>();

            CreateMap<Entities.Book, BookWithCovers>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<IEnumerable<ExternalModels.BookCover>, BookWithCovers>()
                .ForMember(dest => dest.BookCovers, opt => opt.MapFrom(src => src));

            CreateMap<ExternalModels.BookCover, Models.BookCover>();
        }
    }
}