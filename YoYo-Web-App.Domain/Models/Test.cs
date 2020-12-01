using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YoYo_Web_App.Domain.Models
{
    public class Test
    {
        [Key]
        public Guid ID { get; set; }
        public TestInfo TestInfo { get; set; }
        public List<TestParticipantInfo> Participants { get; set; }
    }
}
