using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YoYo_Web_App.Domain.EventsArgs;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.BusinessLogic.Managers.Contracts
{
    public interface ITestManager
    {
        event EventHandler<TestStatusChangedEventArgs> OnStatusChange;

        Task<Test> GetTest();
        Task<Test> StartTest();
        Task<Test> StopTest();
        Task<Test> MarkCompleted();
        Task<Time> GetElapsedTime();
        Task<TestParticipantInfo> WarnParticipant(TestParticipantInfo participantInfo);
        Task<TestResult> StopParticipantAndGetResult(TestParticipantInfo participantInfo);
        Task<List<TestResult>> GetParticipantResult(TestParticipantInfo participantInfo);
    }
}
