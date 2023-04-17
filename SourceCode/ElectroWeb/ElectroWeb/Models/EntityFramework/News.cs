using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectroWeb.Models.EntityFramework
{
    [Table("news")]
    public class News : GeneralDataAbstract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề tin")]
        [StringLength(150)]
        public string Title { get; set; }
        public string Alias { get; set; }
        public int MenuID { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string Image { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public virtual Menu Menu { get; set; }
    }
}