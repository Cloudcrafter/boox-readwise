using AutoMapper;
using Boox.NoteParser.Models;
using Boox.ReadwiseApi.Domain.Models;

namespace Boox.Readwise.Mappers
{
    public class HighlightsProfile : Profile
    {
        public HighlightsProfile()
        {
            CreateMap<Book, ReadwiseHighlight>();
        }
    }
}