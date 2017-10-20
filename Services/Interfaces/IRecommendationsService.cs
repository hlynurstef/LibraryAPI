using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Services
{
    public interface IRecommendationsService
    {
         IEnumerable<BookDTO> GetRecommendationsForUser(int userId);
    }
}