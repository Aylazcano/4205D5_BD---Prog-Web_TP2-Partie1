using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4204D5_labo10.Models
{
    [Table("Chanteur", Schema = "Musique")]
    public partial class Chanteur
    {
        public Chanteur()
        {
            Chansons = new HashSet<Chanson>();
        }

        [StringLength(50)]
        public string Nom { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime DateNaissance { get; set; }
        [Key]
        [Column("ChanteurID")]
        public int ChanteurId { get; set; }

        [InverseProperty("Chanteur")]
        public virtual ICollection<Chanson> Chansons { get; set; }
    }
}
