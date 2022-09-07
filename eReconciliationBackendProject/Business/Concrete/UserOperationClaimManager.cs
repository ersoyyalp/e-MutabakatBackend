using Business.Abstract;
using Business.BusinessAspects;
using Business.Constans;
using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        //[SecuredOperation("Admin,UserOperationClaim.Add")]
        public IResult Add(UserOperationClaim useroperationClaim)
        {
            _userOperationClaimDal.Add(useroperationClaim);
            return new SuccessResult(Messages.AddedUserOperationClaim);
        }

        [SecuredOperation("Admin,UserOperationClaim.Delete")]
        public IResult Delete(UserOperationClaim useroperationClaim)
        {
            _userOperationClaimDal.Delete(useroperationClaim);
            return new SuccessResult(Messages.DeletedUserOperationClaim);
        }

        [SecuredOperation("Admin,UserOperationClaim.Get")]
        public IDataResult<UserOperationClaim> GetById(int id)
        {
            return new SuccessDataResult<UserOperationClaim>(_userOperationClaimDal.Get(i=>i.Id ==id));
        }
      
        [SecuredOperation("Admin,UserOperationClaim.GetList")]
        public IDataResult<List<UserOperationClaim>> GetList(int userId, int companyId)
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetList(p => p.UserId == userId && p.CompanyId == companyId));
        }

        public IDataResult<List<UserOperationClaimDto>> GetListDto(int userId, int companyId)
        {
            return new SuccessDataResult<List<UserOperationClaimDto>>(_userOperationClaimDal.GetListDto(userId,companyId));
        }

        [SecuredOperation("Admin,UserOperationClaim.Update")]
        public IResult Update(UserOperationClaim useroperationClaim)
        {
            _userOperationClaimDal.Update(useroperationClaim);
            return new SuccessResult(Messages.UpdatedUserOperationClaim);
        }
    }
}
