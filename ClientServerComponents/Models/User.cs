using ClientServerComponents.Infrastructure.ClientCommands;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClientServerComponents.Models
{
    [Serializable]
    [Table("Users")]
    public class User
    {
        /*        public User()
                {
                    Messages = new HashSet<Message>();
                    Contacts = new HashSet<User>();
                }*/
        [Key]
        public Guid? Id { get; set; }
        public string Nickname { get; set; }
        public string LastSeen { get; set; }
        public DateTime DateCreated { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ClientStatus Status { get; set; }
        /*        [JsonIgnore]
                public ICollection<Message> Messages { get; set; }
                [JsonIgnore]
                public ICollection<User> Contacts { get; set; }*/
    }
}
