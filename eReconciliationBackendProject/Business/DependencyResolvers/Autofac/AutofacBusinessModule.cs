﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {   //ICompanyService çağırdığımda CompanyManager a git
            builder.RegisterType<CompanyManager>().As<ICompanyService>();
            builder.RegisterType<EfCompanyDal>().As<ICompanyDal>();

            builder.RegisterType<AccountReconciliationDetailManager>().As<IAccountReconciliationDetailService>();
            builder.RegisterType<EfAccountReconciliationDetailDal>().As<IAccountReconciliationDetailDal>();
            
            builder.RegisterType<AccountReconciliationManager>().As<IAccountReconciliationService>();
            builder.RegisterType<EfAccountReconciliationDal>().As<IAccountReconciliationDal>();
            
            builder.RegisterType<BaBsReconciliationDetailManager>().As<IBaBsReconciliationDetailService>();
            builder.RegisterType<EfBaBsReconciliationDetailDal>().As<IBaBsReconciliationDetailDal>();

            builder.RegisterType<BaBsReconciliationManager>().As<IBaBsReconciliationService>();
            builder.RegisterType<EfBaBsReconciliationDal>().As<IBaBsReconciliationDal>();
            
            builder.RegisterType<CurrencyManager>().As<ICurrencyService>();
            builder.RegisterType<EfCurrencyDal>().As<ICurrencyDal>();

            builder.RegisterType<CurrencyAccountManager>().As<ICurrencyAccountService>();
            builder.RegisterType<EfCurrencyAccountDal>().As<ICurrencyAccountDal>();
            
            builder.RegisterType<MailParameterManager>().As<IMailParameterService>();
            builder.RegisterType<EfMailParameterDal>().As<IMailParameterDal>();

            builder.RegisterType<MailTemplateManager>().As<IMailTemplateService>();
            builder.RegisterType<EfMailTemplateDal>().As<IMailTemplateDal>();

            builder.RegisterType<MailManager>().As<IMailService>();
            builder.RegisterType<EfMailDal>().As<IMailDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<TermsandConditionManager>().As<ITermsandConditionService>();
            builder.RegisterType<EfTermsandConditionDal>().As<ITermsandConditionDal>();

            builder.RegisterType<OperationClaimManager>().As<IOperationClaimService>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();

            builder.RegisterType<UserReletionShipManager>().As<IUserReletionShipService>();
            builder.RegisterType<EfUserReletionShipDal>().As<IUserReletionshipDal>();

            builder.RegisterType<ForgotPasswordManager>().As<IForgotPasswordService>();
            builder.RegisterType<EfForgotPasswordDal>().As<IForgotPasswordDal>();

            builder.RegisterType<UserCompanyManager>().As<IUserCompanyService>();
            builder.RegisterType<EfUserCompanyDal>().As<IUserCompanyDal>();

            builder.RegisterType<UserOperationClaimManager>().As<IUserOperationClaimService>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();
           
        }
    }
}