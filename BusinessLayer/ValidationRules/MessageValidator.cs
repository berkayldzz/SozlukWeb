﻿using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class MessageValidator : AbstractValidator<Message>
    {
        public MessageValidator()
        {
            RuleFor(x => x.ReceiverMail).NotEmpty().WithMessage("Alıcı Adresini Boş Geçemezsiniz");
            RuleFor(x => x.Subject).NotEmpty().WithMessage("Konuyu Boş Geçemezsiniz");
            RuleFor(x => x.MessageContent).NotEmpty().WithMessage("Mesajı Boş Geçemezsiniz");
            RuleFor(x => x.Subject).MinimumLength(3).WithMessage("Lütfen en az 3 karakter girişi yapınız ");
            RuleFor(x => x.Subject).MaximumLength(100).WithMessage("Lütfen 100 karakterden fazla değer girişi yapmayın ");

        }

    }
}
