using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.JWT
{
    public interface ITokenHelper
    {//Authourize işlemleri için token oluşturdum
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims, int companyId, string companyName);
    }
}
