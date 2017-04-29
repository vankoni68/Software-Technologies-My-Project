using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class Tag
    {
        private ICollection<Skizone> skizones;

        public Tag()
        {
            this.skizones = new HashSet<Skizone>();
        }

        [Key]
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        public virtual ICollection<Skizone> Skizones
        {
            get { return this.skizones; }
            set { this.skizones = value; }
        }
    }
}