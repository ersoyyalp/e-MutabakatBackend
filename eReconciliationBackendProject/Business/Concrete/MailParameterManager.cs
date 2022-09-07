﻿using Business.Abstract;
using Business.BusinessAspects;
using Business.Constans;
using Core.Aspects.Caching;
using Core.Aspects.Perfomance;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class MailParameterManager : IMailParameterService
    {
        private readonly IMailParameterDal _mailParameterDal;
        private readonly IMailService _mailService;
        public MailParameterManager(IMailParameterDal mailParameterDal,IMailService mailService)
        {
            _mailParameterDal = mailParameterDal;
            _mailService = mailService;
        }

        public IResult ConnectionTest(int companyId)
        {
            var result = _mailParameterDal.Get(p => p.CompanyId == companyId);

            Entities.DTOs.SendMailDto sendMailDto = new Entities.DTOs.SendMailDto
            {
                subject = "Bağlantı Test Maili",
                email = result.Email,
                mailParameter = result,
                body = "Bu bir bağlantı test mailidir. Eğer maili görüyorsanız mail bağlantınız başarılı demektir."
            };

            _mailService.SendMail(sendMailDto);

            return new SuccessResult(Messages.MailSendSucessful);
        }

        [CacheAspect(60)]
        public IDataResult<MailParameter> Get(int companyId)
        {
            return new SuccessDataResult<MailParameter>(_mailParameterDal.Get(m => m.CompanyId == companyId));
        }

        [PerformanceAspect(3)]
        [SecuredOperation("MailParameter.Update,Admin")]
        [CacheRemoveAspect("IMailParameterService.Get")]
        public IResult Update(MailParameter mailParameter)
        {
            var result = Get(mailParameter.CompanyId);
            if (result.Data  == null)
            {
                _mailParameterDal.Add(mailParameter);
            }
            else
            {
              
                _mailParameterDal.Update(mailParameter);
            }
            return new SuccessResult(Messages.MailParameterUpdated);
        }
    }
}
