using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities.Messaging
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
