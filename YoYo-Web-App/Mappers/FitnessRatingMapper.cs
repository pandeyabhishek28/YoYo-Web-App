using System.Collections.Generic;
using System.Linq;
using YoYo_Web_App.Domain.Models;
using YoYo_Web_App.DTOModels;

namespace YoYo_Web_App.Mappers
{
    public static class FitnessRatingMapper
    {
        public static FitnessRating Map(FitnessRatingDTO fitnessRatingDTO)
        {
            return new FitnessRating
            {
                AccumulatedShuttleDistance = fitnessRatingDTO.AccumulatedShuttleDistance,
                SpeedLevel = fitnessRatingDTO.SpeedLevel,
                ShuttleNo = fitnessRatingDTO.ShuttleNo,
                Speed = fitnessRatingDTO.Speed,
                LevelTime = fitnessRatingDTO.LevelTime,
                CommulativeTime = Map(fitnessRatingDTO.CommulativeTime),
                StartTime = Map(fitnessRatingDTO.StartTime),
                ApproxVo2Max = fitnessRatingDTO.ApproxVo2Max
            };
        }

        public static FitnessRatingDTO Map(FitnessRating fitnessRating)
        {
            return new FitnessRatingDTO
            {
                AccumulatedShuttleDistance = fitnessRating.AccumulatedShuttleDistance,
                SpeedLevel = fitnessRating.SpeedLevel,
                ShuttleNo = fitnessRating.ShuttleNo,
                Speed = fitnessRating.Speed,
                LevelTime = fitnessRating.LevelTime,
                CommulativeTime = Map(fitnessRating.CommulativeTime),
                StartTime = Map(fitnessRating.StartTime),
                ApproxVo2Max = fitnessRating.ApproxVo2Max
            };
        }

        public static List<FitnessRating> Map(List<FitnessRatingDTO> fitnessRatingDTOs)
        {
            return fitnessRatingDTOs.Select(x => Map(x)).ToList();
        }

        public static List<FitnessRatingDTO> Map(List<FitnessRating> fitnessRatings)
        {
            return fitnessRatings.Select(x => Map(x)).ToList();
        }
        public static Time Map(TimeDTO timeDto)
        {
            if (timeDto is null) new Time();
            return new Time(timeDto.Hours, timeDto.Minutes);
        }

        public static TimeDTO Map(Time time)
        {
            if (time is null) return new TimeDTO();
            return new TimeDTO
            {
                Hours = time.Hours,
                Minutes = time.Minutes
            };
        }
    }
}
