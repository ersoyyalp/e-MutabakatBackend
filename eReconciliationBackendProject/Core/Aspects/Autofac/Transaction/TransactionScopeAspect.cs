using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspect: MethodInterception
    {//Transaction Aspecti
        public override void Intercept(IInvocation invocation)
        {
           using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    //Eğer işlemde hata yoksa işlemi tamamla
                    invocation.Proceed();
                    transactionScope.Complete();
                }
                catch (Exception)
                { //Eğer işlemde hata varsa yapılan tüm işlemleri geri al
                    transactionScope.Dispose();
                    throw;
                }
            }
        }
    }
}
