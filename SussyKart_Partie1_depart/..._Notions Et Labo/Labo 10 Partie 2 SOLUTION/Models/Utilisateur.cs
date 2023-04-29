using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace _4204D5_labo10.Models
{
    [Table("Utilisateur", Schema = "Utilisateurs")]
    [Index("Pseudo", Name = "UC_Utilisateur_Pseudo", IsUnique = true)]
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            ChanteurFavoris = new HashSet<ChanteurFavori>();
        }

        [Key]
        [Column("UtilisateurID")]
        public int UtilisateurId { get; set; }
        [StringLength(50)]
        public string Pseudo { get; set; } = null!;
        [MaxLength(32)]
        public byte[] MotDePasseHache { get; set; } = null!;
        [MaxLength(16)]
        public byte[] Sel { get; set; } = null!;
        public byte[] CouleurPrefere { get; set; } = null!;

        [InverseProperty("Utilisateur")]
        public virtual ICollection<ChanteurFavori> ChanteurFavoris { get; set; }
    }
}
