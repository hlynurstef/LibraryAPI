using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Services
{
    public interface IBooksService
    {
         IEnumerable<BookDTO> GetBooks();
    }
}