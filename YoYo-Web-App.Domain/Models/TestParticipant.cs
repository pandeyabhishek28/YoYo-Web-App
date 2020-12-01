using System;
using System.ComponentModel.DataAnnotations;

namespace YoYo_Web_App.Domain.Models
{
    public abstract class TestParticipant
    {
        [Key]
        public Guid ID { get; set; }
    }
}
