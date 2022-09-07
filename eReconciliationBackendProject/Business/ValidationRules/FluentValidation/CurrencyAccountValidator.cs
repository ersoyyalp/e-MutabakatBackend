using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules.FluentValidation
{
    public class CurrencyAccountValidator : AbstractValidator<CurrencyAccount>
    {
        public CurrencyAccountValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Firma adı boş bırakılamaz.");
            RuleFor(p => p.Name).MinimumLength(4).WithMessage("Firma adı en az 4 karakter olabilir.");
            RuleFor(p => p.Address).NotEmpty().WithMessage("Firma adresi boş bırakılamaz.");
            RuleFor(p => p.Address).MinimumLength(4).WithMessage("Firma adresi en az 4 karakter olabilir.");
        }
    }
}
