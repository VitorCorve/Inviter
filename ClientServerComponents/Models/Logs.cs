using System;
using System.ComponentModel.DataAnnotations;

namespace ClientServerComponents.Models
{
    public class Logs
    {
        [Key]
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
