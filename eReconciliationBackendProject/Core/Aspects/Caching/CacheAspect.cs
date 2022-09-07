using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {//TİP : $ içerisine değişken gönderebilmemizi sağlar

            //Cache kontrolunu yaptım
           var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}");
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(", ", arguments.Select(x => x?.ToString() ?? "<Null>"))})";
            //eğer sistemde buna ait daha once bir cache varsa onu getirir
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);
                return;
            }
            //eğer ilk defa çağırıyosak ve cache yoksa hafızasında kaydeder sonra veriri getirir.
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);

        }
    }
}
