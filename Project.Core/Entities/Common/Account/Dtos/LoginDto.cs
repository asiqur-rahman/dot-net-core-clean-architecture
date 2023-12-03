using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Entities.Common.Account.Dtos
{
    public record LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
