using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class SuccessResult : Result
    { //İşlem başarılıysa döndereceği veriyi gösterecek - işlem sonucu dönderir
                //burası zaten success döneceğinden true yapabılırız
        public SuccessResult() : base(true)
        {
        }

        public SuccessResult(string message) : base(true, message)
        {
        }
    }
}
