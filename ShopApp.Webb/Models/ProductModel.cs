using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Webb.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="ürün adı alanı boş bırakılamaz")]
        [StringLength(60,MinimumLength =10,ErrorMessage ="Ürün ismi minimum 10 karakter ve maksimum 60 karakter olmalıdır")]
        //[DataType(DataType.EmailAddress)]
        //[EmailAddress(ErrorMessage = "Email adresi doğrulanmadı")]
        public string Name { get; set; }

        [Required(ErrorMessage ="resim url alanı boş bırakılamaz")]
        public string ImgeUrl { get; set; }

        [Required(ErrorMessage ="açıklama alanı boş bırakılmaz")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Fiyat belirtiniz")]
        [Range(1,10000,ErrorMessage ="1 ile 10000 arasında bir sayı girilmeli")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}
