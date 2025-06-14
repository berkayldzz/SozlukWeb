using EntityLayer.Concrete;
using FluentValidation;

namespace BusinessLayer.ValidationRules
{
    public class WriterValidator : AbstractValidator<Writer>
    {
        // Validator sınıfları içersinde kullanıcağımız doğrulama kurallarını bir constructor metot içersinde yazıcaz.
        //public WriterValidator()
        //{
        //    RuleFor(x => x.WriterName).NotEmpty().WithMessage("Yazar Adını Boş Geçemezsiniz");
        //    RuleFor(x => x.WriterSurName).NotEmpty().WithMessage("Yazar Soyadını Boş Geçemezsiniz");
        //    RuleFor(x => x.WriterAbout).NotEmpty().WithMessage("Hakkımda Kısmını Boş Geçemezsiniz");
        //    RuleFor(x => x.WriterTitle).NotEmpty().WithMessage("Ünvan Kısmını Boş Geçemezsiniz");
        //    RuleFor(x => x.WriterSurName).MinimumLength(2).WithMessage("Lütfen en az 2 karakter girişi yapınız ");
        //    RuleFor(x => x.WriterSurName).MaximumLength(50).WithMessage("Lütfen 50 karakterden fazla değer girişi yapmayınız ");
        //}

        public WriterValidator()
        {
            // WriterName Validation
            RuleFor(x => x.WriterName)
                .NotEmpty().WithMessage("Yazar adını boş geçemezsiniz.")
                .MinimumLength(2).WithMessage("Yazar adı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Yazar adı en fazla 50 karakter olmalıdır.");

            // WriterSurName Validation
            RuleFor(x => x.WriterSurName)
                .NotEmpty().WithMessage("Yazar soyadını boş geçemezsiniz.")
                .MinimumLength(2).WithMessage("Yazar soyadı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Yazar soyadı en fazla 50 karakter olmalıdır.");

            // WriterAbout Validation
            RuleFor(x => x.WriterAbout)
                .NotEmpty().WithMessage("Hakkımda kısmını boş geçemezsiniz.")
                .MinimumLength(10).WithMessage("Hakkımda yazısı en az 10 karakter olmalıdır.") // More descriptive 'About'
                .MaximumLength(100).WithMessage("Hakkımda yazısı en fazla 100 karakter olmalıdır.");

            // WriterMail Validation
            RuleFor(x => x.WriterMail)
           .NotEmpty().WithMessage("E-posta adresini boş geçemezsiniz.")
           .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.") // Standart e-posta format kontrolü
           .MaximumLength(200).WithMessage("E-posta adresi en fazla 200 karakter olmalıdır.")
           // Yeni kural: Türkçe karakter kontrolü
           .Matches("^[a-zA-Z0-9@._\\-]*$").WithMessage("E-posta adresi Türkçe karakter (ş, ç, ı, ğ, ö, ü) veya özel işaretler içermemelidir.");

            // WriterPassword Validation
            RuleFor(x => x.WriterPassword)
                .NotEmpty().WithMessage("Şifreyi boş geçemezsiniz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .MaximumLength(200).WithMessage("Şifre en fazla 200 karakter olmalıdır.")
                // You can add more complex password rules like:
                // .Matches("[A-Z]").WithMessage("Şifreniz en az bir büyük harf içermelidir.")
                // .Matches("[a-z]").WithMessage("Şifreniz en az bir küçük harf içermelidir.")
                // .Matches("[0-9]").WithMessage("Şifreniz en az bir rakam içermelidir.")
                // .Matches("[^a-zA-Z0-9]").WithMessage("Şifreniz en az bir özel karakter içermelidir.");
                ;

            // WriterImage Validation
         
            // WriterTitle Validation
            RuleFor(x => x.WriterTitle)
                .NotEmpty().WithMessage("Ünvan kısmını boş geçemezsiniz.")
                .MinimumLength(3).WithMessage("Ünvan en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Ünvan en fazla 50 karakter olmalıdır.");
        }
    }
}
