using System.Collections.Generic;
using LibraryAPI.Models.DTOModels;

namespace LibraryAPI.Repositories {
    public interface IRecommendationsRepository {
        IEnumerable<BookDTO> GetRecommendationsForUser (int userId);
    }
}