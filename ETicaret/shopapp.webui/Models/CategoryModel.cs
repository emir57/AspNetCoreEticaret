using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Kategori zorunludur")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="Kategori için 5 ile 100 arasında bir değer giriniz.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Url zorunludur")]
        [StringLength(100,MinimumLength =5,ErrorMessage ="Url 5 ile 100 karakter arasında bir değer olmaldırı.")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}