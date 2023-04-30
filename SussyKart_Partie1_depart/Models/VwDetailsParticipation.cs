using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SussyKart_Partie1.Models
{
    [Keyless]
    public partial class VwDetailsParticipation
    {
        [StringLength(30)]
        public string Joueur { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Course { get; set; } = null!;
        public int NbJoueurs { get; set; }
        public int Position { get; set; }
        public int Chrono { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateParticipation { get; set; }
    }
}
