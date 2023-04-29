using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4204D5_labo10.Models
{
    [Keyless]
    public partial class VwChanteurNbChanson
    {
        [Column("ChanteurID")]
        public int ChanteurId { get; set; }
        [StringLength(50)]
        public string Nom { get; set; } = null!;
        [Column("Date de naissance", TypeName = "date")]
        public DateTime DateDeNaissance { get; set; }
        [Column("Nombre de chansons")]
        public int? NombreDeChansons { get; set; }
    }
}
