using ClientServerComponents.Infrastructure.ClientCommands;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClientServerComponents.Models
{
    [Serializable]
    [Table("Messages")]
    public class Messages
    {
        [Key]
        public Guid MessageId { get; set; }
        public ClientRequest RequestType { get; set; }
        public Guid SenderId { get; set; }
        public Guid TargetId { get; set; }
        public DateTime SendedTime { get; set; }
        public DateTime DeliveredToServerTime { get; set; }
        public DateTime DeliveredToTargetTime { get; set; }
        public MessageStatus Status { get; set; }
        public string Text { get; set; }
        [JsonIgnore]
        [NotMapped]
        public User Sender { get; set; }
    }
}
