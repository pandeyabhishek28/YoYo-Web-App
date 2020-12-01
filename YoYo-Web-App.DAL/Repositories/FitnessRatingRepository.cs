using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YoYo_Web_App.Domain.Models;
using YoYo_Web_App.Domain.Repositories;

namespace YoYo_Web_App.DAL.Repositories
{
    public class FitnessRatingRepository : IFitnessRatingRepository
    {
        private readonly string dataFilePath;
        public FitnessRatingRepository(string dataFilePath)
        {
            if (string.IsNullOrEmpty(dataFilePath))
                throw new ArgumentNullException("dataFilePath");
            if (!File.Exists(dataFilePath))
                throw new FileNotFoundException(dataFilePath);

            this.dataFilePath = dataFilePath;
        }

        public async Task<List<FitnessRating>> GetAllRatingsAsync()
        {
            return await GetAllFromJsonFile();
        }

        public async Task<List<FitnessRating>> GetAllFromJsonFile()
        {
            using (var stream = File.Open(dataFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    string json = await streamReader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<List<FitnessRating>>(json);
                }
            }
        }

        public async Task SaveAllToJsonFile(List<FitnessRating> fitnessRatings)
        {
            using (var stream = File.Open(dataFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    var stringJson = JsonConvert.SerializeObject(fitnessRatings);
                    await streamWriter.WriteAsync(stringJson);
                }
            }
        }
    }
}
