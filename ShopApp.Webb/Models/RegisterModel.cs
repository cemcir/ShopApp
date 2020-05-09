using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Webb.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="isim ve soyisim girmelisiniz")]
        //[RegularExpression("([A-Za-z])+( [A-Za-z]+)", ErrorMessage = "isimle soyisim arasında bir karakter boşluk olmalı ve ismin ilk harfi büyük olmalıdır")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="kullanıcı adı girmelisiniz")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="şifre girmelisiniz")]
        [DataType(DataType.Password)]
        [StringLength(72,ErrorMessage ="şifre minimum 6 karakter olmalıdır",MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage ="şifreyi tekrar girmelisiniz")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="şifre tekrar ile şifre uyuşmuyor")]
        public string RePassword { get; set; }

        [Required(ErrorMessage ="e-posta girmelisiniz")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "email adresi doğrulanmadı")]
        public string Email { get; set; }
    }
}
