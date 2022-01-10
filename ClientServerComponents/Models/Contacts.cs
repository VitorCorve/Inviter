using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClientServerComponents.Models
{
    public class Contacts
    {
        public Contacts()
        {
            Users = new HashSet<User>();
        }
        public Guid UserId { get; set; }
        [Key]
        public Guid LinkedWith { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
