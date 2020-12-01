using System.Collections.Generic;
using YoYo_Web_App.Domain.Models;

namespace YoYo_Web_App.DAL.Services.Contracts
{
    public interface IDummyDataService
    {
        List<Athlete> GetAthletes();
        List<TestParticipantInfo> GetTestParticipants();
        Test GetTestObjWithInitialData();
    }
}
