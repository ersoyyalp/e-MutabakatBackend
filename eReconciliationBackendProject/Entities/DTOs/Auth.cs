using Core.Entities;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs
{
    public class Auth :IDto
    {
        public User user { get; set; }
        public List<OperationClaim> operationClaims { get; set; }
    }
}
