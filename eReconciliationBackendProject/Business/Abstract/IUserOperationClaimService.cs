using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserOperationClaimService
    {
        IResult Add(UserOperationClaim useroperationClaim);
        IResult Update(UserOperationClaim useroperationClaim);
        IResult Delete(UserOperationClaim useroperationClaim);
        IDataResult<UserOperationClaim> GetById(int id);
        IDataResult<List<UserOperationClaim>> GetList(int userId, int companyId);
        IDataResult<List<UserOperationClaimDto>> GetListDto(int userId, int companyId);
    }
}
