﻿using Business.Abstract;
using Business.BusinessAspects;
using Business.Constans;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Caching;
using Core.Aspects.Perfomance;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMailDto = Entities.DTOs.SendMailDto;

namespace Business.Concrete
{
    public class BaBsReconciliationManager : IBaBsReconciliationService
    {
        private readonly IBaBsReconciliationDal _baBsReconciliationDal;
        private readonly ICurrencyAccountService _currencyAccountService;
        private readonly IMailService _mailService;
        private readonly IMailTemplateService _mailTemplateService;
        private readonly IMailParameterService _mailParameterService;

        public BaBsReconciliationManager(IBaBsReconciliationDal baBsReconciliationDal, ICurrencyAccountService currencyAccountService, IMailService mailService, IMailTemplateService mailTemplateService, IMailParameterService mailParameterService)
        {
            _baBsReconciliationDal = baBsReconciliationDal;
            _currencyAccountService = currencyAccountService;
            _mailService = mailService;
            _mailTemplateService = mailTemplateService;
            _mailParameterService = mailParameterService;
        }

        [PerformanceAspect(3)]
        //[SecuredOperation("BaBsReconciliation.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Add(BaBsReconciliation babsReconciliation)
        {
            string guid = Guid.NewGuid().ToString();
            babsReconciliation.Guid = guid;
            _baBsReconciliationDal.Add(babsReconciliation);
            return new SuccessResult(Messages.AddedBaBsReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Add,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        [TransactionScopeAspect]
        public IResult AddToExcel(string filePath, int companyId)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        string code = reader.GetString(0);

                        if (code != "Cari Kodu" && code != null)
                        {
                            string type = reader.GetString(1);
                            double mounth = reader.GetDouble(2);
                            double year = reader.GetDouble(3);
                            double quantity = reader.GetDouble(4);
                            double total = reader.GetDouble(5);
                            string guid = Guid.NewGuid().ToString();
                            int currencyAccounId = _currencyAccountService.GetByCode(code, companyId).Data.Id;

                            BaBsReconciliation baBsReconciliation = new BaBsReconciliation()
                            {
                                CompanyId = companyId,
                                CurrencyAccountId = currencyAccounId,
                                Type = type,
                                Mounth = Convert.ToInt16(mounth),
                                Year = Convert.ToInt16(year),
                                Quantity = Convert.ToInt16(quantity),
                                Total = Convert.ToDecimal(total),
                                Guid = guid
                            };
                           _baBsReconciliationDal.Add(baBsReconciliation);
                        }
                    }
                }
            }
            File.Delete(filePath);
            return new SuccessResult(Messages.AddedAccountReconciliation);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Delete,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Delete(BaBsReconciliation babsReconciliation)
        {
            _baBsReconciliationDal.Delete(babsReconciliation);
            return new SuccessResult(Messages.DeletedBaBsReconciliation);
        }

        [PerformanceAspect(3)]
        //[SecuredOperation("BaBsReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliation> GetByCode(string code)
        {
            return new SuccessDataResult<BaBsReconciliation>(_baBsReconciliationDal.Get(p => p.Guid == code));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Get,Admin")]
        [CacheAspect(60)]
        public IDataResult<BaBsReconciliation> GetById(int id)
        {
            return new SuccessDataResult<BaBsReconciliation>(_baBsReconciliationDal.Get(p => p.Id == id));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliation>> GetList(int companyId)
        {
            return new SuccessDataResult<List<BaBsReconciliation>>(_baBsReconciliationDal.GetList(p => p.CompanyId == companyId));
        }


        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliation>> GetListByCurrencyAccountId(int currencyAccount)
        {
            return new SuccessDataResult<List<BaBsReconciliation>>(_baBsReconciliationDal.GetList(p => p.CurrencyAccountId == currencyAccount));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.GetList,Admin")]
        [CacheAspect(60)]
        public IDataResult<List<BaBsReconciliationDto>> GetListDto(int companyId)
        {
            return new SuccessDataResult<List<BaBsReconciliationDto>>(_baBsReconciliationDal.GetAllDto(companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.SendMail,Admin")]
        public IResult SendReconciliationMail(BaBsReconciliationDto baBsReconciliationDto)
        {
            string subject = "Mutabakat Maili";
            string body = $"Şirketimiz: {baBsReconciliationDto.CompanyName} <br /> " +
                $"Şirketimizin Vergi Dairesi: {baBsReconciliationDto.CompanyTaxDepartment} <br />" +
                $"Şirketimizin Vergi Numarası: {baBsReconciliationDto.CompanyTaxIdNumber} - {baBsReconciliationDto.CompanyIdentityNumber} <br /><hr>" +
                $"Sizin Şirketiniz: {baBsReconciliationDto.AccountName} <br />" +
                $"Sizin Şirketinizin Vergi Dairesi: {baBsReconciliationDto.AccountTaxDepartment} <br />" +
                $"Sizin Şirketinizin Vergi Numarası: {baBsReconciliationDto.AccountTaxIdNumber} - {baBsReconciliationDto.AccountIdentityNumber} <br /><hr>" +
                $"Ay / Yıl: {baBsReconciliationDto.Mounth} / {baBsReconciliationDto.Year}<br />" +
                $"Adet: {baBsReconciliationDto.Quantity}<br />" +
                $"Tutar: {baBsReconciliationDto.Total} {baBsReconciliationDto.CurrencyCode} <br />";

            //Guid benzersiz bir kod oluşturur guvenlik olarak daha uiyi
            string link = "https://localhost:7101/api/BaBsReconciliations/GetByCode?code=" + baBsReconciliationDto.Guid;
            string linkDescription = "Mutabakatı Cevaplamak İçin Tıklayınız.";

            var mailTemplate = _mailTemplateService.GetByTemplateName("Kayıt", 7);
            string templateBody = mailTemplate.Data.Value;
            templateBody = templateBody.Replace("{{title}}", subject);
            templateBody = templateBody.Replace("{{message}}", body);
            templateBody = templateBody.Replace("{{link}}", link);
            templateBody = templateBody.Replace("{{linkDescription}}", linkDescription);

            var mailParameter = _mailParameterService.Get(7);
            SendMailDto sendMailDto = new SendMailDto()
            {
                mailParameter = mailParameter.Data,
                email = baBsReconciliationDto.AccountEmail,
                subject = subject,
                body = templateBody

            };
            _mailService.SendMail(sendMailDto);

            return new SuccessResult(Messages.MailSendSuccessful);
        }

        [PerformanceAspect(3)]
        [SecuredOperation("BaBsReconciliation.Update,Admin")]
        [CacheRemoveAspect("IBaBsReconciliationService.Get")]
        public IResult Update(BaBsReconciliation babsReconciliation)
        {
            _baBsReconciliationDal.Update(babsReconciliation);
            return new SuccessResult(Messages.UpdatedBaBsReconciliation);
        }
    }
}
