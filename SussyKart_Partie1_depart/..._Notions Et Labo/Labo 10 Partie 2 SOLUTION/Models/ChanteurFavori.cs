using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4204D5_labo10.Models
{
    [Table("ChanteurFavori", Schema = "Musique")]
    [Index("ChanteurId", "UtilisateurId", Name = "UC_ChanteurFavori_ChanteurUtilisateur", IsUnique = true)]
    public partial class ChanteurFavori
    {
        [Key]
        [Column("ChanteurFavoriID")]
        public int ChanteurFavoriId { get; set; }
        [Column("ChanteurID")]
        public int ChanteurId { get; set; }
        [Column("UtilisateurID")]
        public int UtilisateurId { get; set; }

        [ForeignKey("ChanteurId")]
        [InverseProperty("ChanteurFavoris")]
        public virtual Chanteur Chanteur { get; set; } = null!;
        [ForeignKey("UtilisateurId")]
        [InverseProperty("ChanteurFavoris")]
        public virtual Utilisateur Utilisateur { get; set; } = null!;
    }
}
