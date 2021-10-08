using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using shopapp.entity;

namespace shopapp.webui.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Display(Name="Name",Prompt ="Enter product name")]
        [Required(ErrorMessage ="Name alanı zorunludur.")]
        [StringLength(60,MinimumLength =5,ErrorMessage ="Name 5 ile 60 karakter arasında olmalıdır.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Url alanı zorunludur.")]
        public string Url { get; set; }

        [Required(ErrorMessage ="Price alanı zorunludur.")]
        [Range(1,10000,ErrorMessage ="Price 1 ile 10000 arasında olmalıdır.")]
        public decimal? Price { get; set; }

        [StringLength(100,MinimumLength =5,ErrorMessage ="Description 5 ile 100 karakter arasında olmalıdır.")]
        [Required(ErrorMessage ="Description alanı zorunludur.")]
        public string Description { get; set; }

        [Required(ErrorMessage ="ImageUrl alanı zorunludur.")]
        public string ImageUrl { get; set; }

        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public List<Category> SelectedCategories { get; set; }
        
    }
}