using System;
using System.Collections.Generic;
using YoYo_Web_App.DAL.Services.Contracts;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.DAL.Services
{
    public class DummyDataService : IDummyDataService
    {
        public DummyDataService()
        {

        }
        public List<Athlete> GetAthletes()
        {
            return new List<Athlete>
            {
                new Athlete
                {
                    ID=Guid.NewGuid(),
                    FirstName="Ashton",
                    LastName="Eaton"
                },
                 new Athlete
                {
                    ID=Guid.NewGuid(),
                    FirstName="Bryan",
                    LastName="Clay"
                },
                  new Athlete
                {
                    ID=Guid.NewGuid(),
                    FirstName="Dean",
                    LastName="Karnazes"
                },
                   new Athlete
                {
                    ID=Guid.NewGuid(),
                    FirstName="Usain",
                    LastName="Bolt"
                },
                    new Athlete
                {
                    ID=Guid.NewGuid(),
                    FirstName="Milkha",
                    LastName="Singh"
                },
            };
        }

        public List<TestParticipantInfo> GetTestParticipants()
        {
            var testParticipantInfos = new List<TestParticipantInfo>();
            foreach (var athlete in GetAthletes())
            {
                testParticipantInfos.Add(new TestParticipantInfo
                {
                    ID = Guid.NewGuid(),
                    Memeber = athlete,
                    Completed = false,
                    Stopped = false,
                    Warned = false,
                    StartTime = null,
                    EndTime = null,
                    TimeTaken = null,
                    Result = new TestResult()
                });
            }
            return testParticipantInfos;
        }

        public Test GetTestObjWithInitialData()
        {
            return new Test
            {
                ID = Guid.NewGuid(),
                TestInfo = GetTestInfo(),
                Participants = GetTestParticipants()
            };
        }

        private TestInfo GetTestInfo()
        {
            return new TestInfo
            {
                StartTime = null,
                EndTime = null,
                NextShuttle = 0,
                RunStatus = TestRunningStatus.None,
                ShuttleNo = 0,
                Speed = 0,
                SpeedLevel = 0,
                TotalDistance = 0,
                TotalElapsedTime = 0d
            };
        }
    }
}
