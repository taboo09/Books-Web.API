using AutoMapper;

namespace Books.API.Profiles
{
    public class BooksProfile :  Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Models.Book>()
                .ForMember(x => x.Author, y => y.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));
        }
    }
}