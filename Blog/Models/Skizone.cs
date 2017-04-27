using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Skizone
    {
        public Skizone()
        {

        }

        public Skizone(string authorId, string name, string elevationInfo, int categoryId, double slopes, double liftTicket, string contentInfo, string imagePath)
        {
            this.AuthorId = authorId;
            this.Name = name;
            this.ElevationInfo = elevationInfo;
            this.ContentInfo = contentInfo;
            this.CategoryId = categoryId;
            this.Slopes = slopes;
            this.LiftTicket = liftTicket;
            this.ImagePath = imagePath;

        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Elevation info (min m - max m)")]
        public string ElevationInfo { get; set; }

        [Required]
        [Range(0.0, 999.9)]
        [Display(Name = "Slopes in km.")]
        public double Slopes { get; set; }

        [Required]
        [Range(0.0, 99.9)]
        [Display(Name = "Lift ticket in €")]
        public double LiftTicket { get; set; }

        [Required]
        [Display(Name = "Info")]
        public string ContentInfo { get; set; }

        public string ImagePath { get; set; }

        public string AuthorId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public bool IsAuthor(string authorId)
        {
            return this.AuthorId == authorId;
        }
    }


}