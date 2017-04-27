using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class SkizoneViewModel
    {
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

        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}