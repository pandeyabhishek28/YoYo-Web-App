using System.Collections.Generic;
using System.Threading.Tasks;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.Domain.Repositories
{
    public interface IFitnessRatingRepository
    {
        Task<List<FitnessRating>> GetAllRatingsAsync();
        Task<List<FitnessRating>> GetAllFromJsonFile();
        Task SaveAllToJsonFile(List<FitnessRating> fitnessRatings);
    }
}
