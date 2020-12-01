using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using YoYo_Web_App.BusinessLogic.Managers.Contracts;
using YoYo_Web_App.DAL.Services.Contracts;
using YoYo_Web_App.Domain.Configurations;
using YoYo_Web_App.Domain.EventsArgs;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.BusinessLogic.Managers
{
    public class TestManager : ITestManager
    {
        private readonly IFitnessRatingService _fitnessRatingService;
        private readonly IDummyDataService _dummyDataService;
        private readonly ILogger<TestManager> _logger;
        private readonly AppConfiguration _appConfig;

        private Test _test;
        private Dictionary<int, List<FitnessRating>> ratingsBySpeedLevelForProcessingTest;
        private Dictionary<int, List<FitnessRating>> ratingsBySpeedLevelForValidatingResults;
        private Timer _timer;
        private int _currentSpeedLevel = 0;

        public event EventHandler<TestStatusChangedEventArgs> OnStatusChange;
        public TestManager(IFitnessRatingService fitnessRatingService,
            IDummyDataService dummyDataService, IOptions<AppConfiguration> appConfig,
            ILogger<TestManager> logger)
        {
            _fitnessRatingService = fitnessRatingService;
            _dummyDataService = dummyDataService;
            _logger = logger;
            _appConfig = appConfig.Value;

            _test = dummyDataService.GetTestObjWithInitialData();
            _ = LoadInitialData();
        }

        public Task<Test> GetTest()
        {
            return Task.FromResult(_test);
        }
        public async Task<Test> StartTest()
        {
            _logger.LogInformation("About to start a Test.");

            if (_test.TestInfo.RunStatus == TestRunningStatus.Stopped ||
                _test.TestInfo.RunStatus == TestRunningStatus.Completed)
            {
                _test = _dummyDataService.GetTestObjWithInitialData();
                await LoadInitialData();
                RaiseStatusChange();
            }
            _test.TestInfo.RunStatus = TestRunningStatus.Started;
            foreach (var participant in _test.Participants)
            {
                participant.StartTime = DateTime.Now;
                participant.Started = true;
            }

            // look up for a valid speed level
            foreach (var speedLevel in ratingsBySpeedLevelForProcessingTest.Keys)
            {
                if (ratingsBySpeedLevelForProcessingTest[speedLevel].Count > 0)
                {
                    _currentSpeedLevel = speedLevel;
                    break;
                }
            }

            _timer = new Timer(_appConfig.TimeIntervalInSeconds * 1000);
            _timer.Elapsed += Interval_Elapsed;

            _test.TestInfo.StartTime = DateTime.Now;
            _timer.Start();

            _logger.LogInformation(string.Format("The Test {0}, started.", _test.ID));

            return await Task.FromResult(_test);
        }
        public async Task<Test> StopTest()
        {
            _logger.LogInformation("About to stop current test.");
            _timer.Stop();
            _timer.Dispose();

            _test.TestInfo.EndTime = DateTime.Now;
            _test.TestInfo.RunStatus = TestRunningStatus.Stopped;
            _test.TestInfo.NextShuttle = 0;
            _test.TestInfo.Speed = 0;
            _test.TestInfo.SpeedLevel = 0;
            _test.TestInfo.ShuttleNo = 0;
            _test.TestInfo.TotalElapsedTime = 0;
            _test.TestInfo.TotalDistance = 0;
            _test.TestInfo.LevelShuttleTime = 0;

            _logger.LogInformation(string.Format("The Test {0}, stopped.", _test.ID));

            return await Task.FromResult(_test);
        }
        public Task<Test> MarkCompleted()
        {
            _test.TestInfo.RunStatus = TestRunningStatus.Completed;
            foreach (var participant in _test.Participants)
            {
                if (!participant.Stopped && participant.Started)
                    participant.Completed = true;
            }

            _logger.LogInformation(string.Format("The Test {0}, maked as completed.", _test.ID));
            return Task.FromResult(_test);
        }
        public Task<Time> GetElapsedTime()
        {
            if (_test.TestInfo.StartTime == null)
                return Task.FromResult(new Time());

            if (_test.TestInfo.EndTime == null)
            {
                var timediff = DateTime.Now.Subtract(_test.TestInfo.StartTime.Value);
                return Task.FromResult(new Time(timediff.Hours, timediff.Minutes));
            }
            else
            {
                var timediff = _test.TestInfo.EndTime.Value.Subtract(_test.TestInfo.StartTime.Value);
                return Task.FromResult(new Time(timediff.Hours, timediff.Minutes));
            }
        }

        public async Task<TestParticipantInfo> WarnParticipant(TestParticipantInfo participantInfo)
        {
            _logger.LogInformation(string.Format("The Participant {0}, warned.", participantInfo.Memeber.FirstName));
            participantInfo.Warned = true;

            return await Task.FromResult(participantInfo);
        }
        public async Task<TestResult> StopParticipantAndGetResult(TestParticipantInfo participantInfo)
        {
            _logger.LogInformation(string.Format("The Participant {0}, stopped.", participantInfo.Memeber.FirstName));

            participantInfo.Stopped = true;
            participantInfo.EndTime = DateTime.Now;
            var totalTimeTaken = participantInfo.EndTime.Value.Subtract(participantInfo.StartTime.Value);
            participantInfo.TimeTaken = new Time(totalTimeTaken.Hours, totalTimeTaken.Minutes);

            if (_test.Participants.All(z => z.Stopped))
            {
                await StopTest();
            }

            participantInfo.Result = await GetTestResult();
            return participantInfo.Result;
        }

        public async Task<List<TestResult>> GetParticipantResult(TestParticipantInfo participantInfo)
        {
            return await Task.FromResult(new List<TestResult>());
        }

        private async Task LoadInitialData()
        {
            _logger.LogInformation("Loading initial data.");

            var ratings = await _fitnessRatingService.GetAllFitnessRatingsFromCache();

            var ratingsGroupedBySpeedLevel = ratings.OrderBy(x => x.SpeedLevel).GroupBy(x => x.SpeedLevel);

            ratingsBySpeedLevelForProcessingTest = ratingsGroupedBySpeedLevel.ToDictionary(x => x.Key,
            y => y.OrderBy(x => x.ShuttleNo).ToList());

            ratingsBySpeedLevelForValidatingResults = ratingsGroupedBySpeedLevel.ToDictionary(x => x.Key,
            y => y.OrderBy(x => x.ShuttleNo).ToList());
            _logger.LogInformation("Done loading initial data.");
        }
        private List<FitnessRating> GetShuttelsAndUpdateSpeedLevelIfNeeded()
        {
            var shuttles = ratingsBySpeedLevelForProcessingTest[_currentSpeedLevel];
            if (shuttles.Count > 0)
            {
                return ratingsBySpeedLevelForProcessingTest[_currentSpeedLevel];
            }
            else// increase the speed level
            {
                var nextSpeedLevel = ratingsBySpeedLevelForProcessingTest.Keys.FirstOrDefault(x => x > _currentSpeedLevel);
                if (nextSpeedLevel == 0) return new List<FitnessRating>();

                _currentSpeedLevel = nextSpeedLevel;

                return ratingsBySpeedLevelForProcessingTest[_currentSpeedLevel];
            }
        }

        private async Task<TestResult> GetTestResult()
        {
            _logger.LogInformation("Test result is requested.");

            var allPossibleResults = new HashSet<FitnessRating>(ratingsBySpeedLevelForValidatingResults[_currentSpeedLevel]);
            var currentShuttleNo = _test.TestInfo.ShuttleNo;
            var previousShuttleNo = currentShuttleNo - 1;
            // putting up -1 here to get the last one
            var result = allPossibleResults.FirstOrDefault(x => x.ShuttleNo == previousShuttleNo);
            if (result == null)
            {
                var previousSpeedLevel = ratingsBySpeedLevelForValidatingResults.Keys.TakeWhile(x => x < _currentSpeedLevel).LastOrDefault();
                if (previousShuttleNo < 1 && ratingsBySpeedLevelForValidatingResults.Keys.Contains(previousSpeedLevel))
                {
                    allPossibleResults = new HashSet<FitnessRating>(ratingsBySpeedLevelForValidatingResults[previousSpeedLevel]);
                    result = allPossibleResults.LastOrDefault();
                }
                else
                {
                    result = allPossibleResults.FirstOrDefault();
                }
            }

            var keyToIndexKeyMap = ratingsBySpeedLevelForValidatingResults.Keys.ToList();
            var currentKeyIndex = keyToIndexKeyMap.IndexOf(_currentSpeedLevel);

            if (currentKeyIndex >= 1)
            {
                ratingsBySpeedLevelForValidatingResults[keyToIndexKeyMap[currentKeyIndex - 1]].ForEach(possibleResult =>
                allPossibleResults.Add(possibleResult));
            }
            if (currentKeyIndex < keyToIndexKeyMap.Count - 1)
            {
                ratingsBySpeedLevelForValidatingResults[keyToIndexKeyMap[currentKeyIndex + 1]].ForEach(possibleResult =>
                allPossibleResults.Add(possibleResult));
            }

            if (result == null) result = allPossibleResults.FirstOrDefault();

            allPossibleResults.Remove(result);

            _logger.LogInformation("Test result is generated.");
            return await Task.FromResult(new TestResult(result, allPossibleResults.ToList()));
        }

        private void UpdateTestState(int speedLevel, FitnessRating shuttelInfo, double timeIntervalInSeconds)
        {
            if (_test.TestInfo.SpeedLevel != speedLevel)
            {
                _test.TestInfo.ElapsedShuttleTime = 0;
                _test.TestInfo.NextShuttle = (int)shuttelInfo.LevelTime + 1;
                _test.TestInfo.SpeedLevel = speedLevel;
                _test.TestInfo.ShuttleNo = shuttelInfo.ShuttleNo;
                _test.TestInfo.LevelShuttleTime = shuttelInfo.LevelTime;
                _test.TestInfo.Speed = shuttelInfo.Speed;
            }
            else if (_test.TestInfo.ShuttleNo != shuttelInfo.ShuttleNo)
            {
                _test.TestInfo.ElapsedShuttleTime = 0;
                _test.TestInfo.ShuttleNo = shuttelInfo.ShuttleNo;
                _test.TestInfo.NextShuttle = (int)shuttelInfo.LevelTime + 1;
            }
            else
            {
                _test.TestInfo.NextShuttle -= (int)timeIntervalInSeconds;
                _test.TestInfo.ElapsedShuttleTime += timeIntervalInSeconds;

                if (_test.TestInfo.NextShuttle < 0)
                    _test.TestInfo.NextShuttle = 0;
            }

            _test.TestInfo.TotalElapsedTime += timeIntervalInSeconds;
            _test.TestInfo.TotalDistance += shuttelInfo.Speed * _appConfig.KmPerHrToMPersConversationFactor * timeIntervalInSeconds;

            RaiseStatusChange();
        }

        private async void Interval_Elapsed(object sender, ElapsedEventArgs e)
        {
            _logger.LogInformation("Interval elapsed. About to update all info.");
            if (!(sender is Timer)) return;
            var timeIntervalInSeconds = _timer.Interval / 1000;
            if (ratingsBySpeedLevelForProcessingTest.ContainsKey(_currentSpeedLevel))
            {
                var shuttles = GetShuttelsAndUpdateSpeedLevelIfNeeded();
                if (shuttles.Count == 0)
                {
                    await StopTest();
                    await MarkCompleted();
                    return;
                }

                var shuttle = shuttles[0];

                if (_test.TestInfo.ShuttleNo == shuttle.ShuttleNo &&
                    (_test.TestInfo.ElapsedShuttleTime + timeIntervalInSeconds) >= shuttle.LevelTime)
                {
                    shuttles.Remove(shuttle);
                    if (shuttles.Count > 0)
                    {
                        shuttle = shuttles[0];
                    }
                    else
                    {
                        shuttles = GetShuttelsAndUpdateSpeedLevelIfNeeded();
                        if (shuttles.Count == 0)
                        {
                            await StopTest();
                            await MarkCompleted();
                            return;
                        }
                        shuttle = shuttles[0];
                    }
                }
                UpdateTestState(_currentSpeedLevel, shuttle, timeIntervalInSeconds);
            }
            else
            {
                List<FitnessRating> firstSpeedLevelShuttels = null;

                // look up for a valid speed level
                foreach (var speedLevel in ratingsBySpeedLevelForProcessingTest.Keys)
                {
                    firstSpeedLevelShuttels = ratingsBySpeedLevelForProcessingTest[speedLevel];
                    if (firstSpeedLevelShuttels.Count > 0)
                    {
                        _currentSpeedLevel = speedLevel;
                        break;
                    }
                }

                if (firstSpeedLevelShuttels == null) throw new Exception("Invalid Fitness ratings dataset.");

                if (firstSpeedLevelShuttels.Count > 0)
                {
                    UpdateTestState(_currentSpeedLevel, firstSpeedLevelShuttels.First(), timeIntervalInSeconds);
                }
            }
        }

        private void RaiseStatusChange()
        {
            OnStatusChange?.Invoke(this, new TestStatusChangedEventArgs(_test));
        }
    }
}
