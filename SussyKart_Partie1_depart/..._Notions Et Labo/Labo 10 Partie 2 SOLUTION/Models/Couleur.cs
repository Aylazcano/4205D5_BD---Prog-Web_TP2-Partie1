using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4204D5_labo10.Models
{
    [Keyless]
    [Table("Couleur", Schema = "Utilisateurs")]
    public partial class Couleur
    {
        [Column("Couleur")]
        [StringLength(30)]
        public string? Couleur1 { get; set; }
    }
}
