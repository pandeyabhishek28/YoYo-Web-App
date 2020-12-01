using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoYo_Web_App.DAL.Services.Contracts;
using YoYo_Web_App.Domain.Models;
using YoYo_Web_App.Domain.Repositories;

namespace YoYo_Web_App.DAL.Services
{
    public class FitnessRatingService : IFitnessRatingService
    {
        private readonly IFitnessRatingRepository _fitnessRatingRepository;
        private readonly ILogger<FitnessRatingService> _logger;
        private readonly List<FitnessRating> fitnessRatings;

        public FitnessRatingService(IFitnessRatingRepository fitnessRatingRepository, ILogger<FitnessRatingService> logger)
        {
            _fitnessRatingRepository = fitnessRatingRepository ?? throw new ArgumentNullException("fitnessRatingRepository");
            _logger = logger;
            fitnessRatings = new List<FitnessRating>();
        }

        public async Task<List<FitnessRating>> GetAllFitnessRatingsAsync()
        {
            _logger.LogInformation("AllFitnessRatings requested.");
            fitnessRatings.Clear();
            fitnessRatings.AddRange(await _fitnessRatingRepository.GetAllRatingsAsync());
            return fitnessRatings;
        }

        public async Task<List<FitnessRating>> GetAllFitnessRatingsFromCache()
        {
            _logger.LogInformation("AllFitnessRatings requested from cache.");
            if (fitnessRatings.Count == 0)
            {
                _logger.LogInformation("now going to read.");
                return await GetAllFitnessRatingsAsync();
            }
            return fitnessRatings;
        }

        public async Task<List<FitnessRating>> SaveAllFitnessRatings(List<FitnessRating> fitnessRatings)
        {
            _logger.LogInformation("Saving fitness ratings back to file.");
            await _fitnessRatingRepository.SaveAllToJsonFile(fitnessRatings);
            return fitnessRatings;
        }

        public async Task<FitnessRating> AddFitnessRating(FitnessRating fitnessRating)
        {
            _logger.LogInformation("Adding fitness rating.");
            await GetAllFitnessRatingsAsync();
            fitnessRatings.Add(fitnessRating);
            await SaveAllFitnessRatings(fitnessRatings);
            return fitnessRating;
        }

        public async Task<FitnessRating> DeleteFitnessRating(FitnessRating fitnessRating)
        {
            _logger.LogInformation("Removing fitness rating.");
            await GetAllFitnessRatingsAsync();
            var removed = fitnessRatings.Remove(fitnessRating);

            if (removed)
                await SaveAllFitnessRatings(fitnessRatings);
            return fitnessRating;
        }
    }
}
