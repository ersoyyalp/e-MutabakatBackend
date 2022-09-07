using Core.Entities.Concrete;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {   //Giriş servisi kullanıcı girişi, kullanıcı kaydı ve kontrol işlemleri imza kısmı
        IDataResult<UserCompanyDto> Register(UserForRegister userForRegister, string password, Company company);
        IDataResult<List<UserReletionshipDto>> RegisterSecondAccount(UserForRegister userForRegister, string password, int companyId, int adminUserId);
        IDataResult<User> Login(UserForLogin userForLogin);
        IDataResult<User> GetByMailConfirmValue(string value);
        IDataResult<User> GetById(int id);
        IDataResult<User> GetByEmail(string email);
        IResult UserExists(string email);
        IResult Update(User user);
        IResult ChangePassword(User user);
        //IResult sonuç döndürür
        IResult CompanyExists(Company company);
        IResult SendConfirmEmailAgain(User user);
        IDataResult<AccessToken> CreateAccessToken(User user, int companyId);
        IDataResult<UserCompany> GetCompany(int userId);
        IResult SendForgotPasswordEmail(User user, string value);

    }
}
