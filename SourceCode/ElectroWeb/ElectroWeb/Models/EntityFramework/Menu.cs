using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ElectroWeb.Models.EntityFramework
{
    [Table("menu")]
    public class Menu : GeneralDataAbstract
    {
        public Menu()
        {
            this.News = new HashSet<News>();
            this.Posts = new HashSet<Post>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Tên menu không được để trống")]
        [StringLength(150)]
        public string Title { get; set; }

        public string Alias { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }
        public int Position { get; set; }
        public bool IsActive { get; set; }

        [StringLength(150)]
        public string SeoTitle { get; set; }

        [StringLength(250)]
        public string SeoDescription { get; set; }

        [StringLength(150)]
        public string SeoKeywords { get; set; }
        public ICollection<News> News { get; set; }
        public ICollection<Post> Posts { get; set; }

    }
}