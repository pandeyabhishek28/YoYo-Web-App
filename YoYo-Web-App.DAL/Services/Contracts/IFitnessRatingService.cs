using System.Collections.Generic;
using System.Threading.Tasks;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.DAL.Services.Contracts
{
    public interface IFitnessRatingService
    {
        Task<List<FitnessRating>> GetAllFitnessRatingsAsync();
        Task<List<FitnessRating>> GetAllFitnessRatingsFromCache();
        Task<FitnessRating> AddFitnessRating(FitnessRating fitnessRating);
        Task<FitnessRating> DeleteFitnessRating(FitnessRating fitnessRating);
        Task<List<FitnessRating>> SaveAllFitnessRatings(List<FitnessRating> fitnessRatings);
    }
}
