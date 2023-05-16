using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SussyKart_Partie1.Models
{
    [Table("Avatar", Schema = "Utilisateurs")]
    [Index("Identifiant", Name = "UC_Avatar_Identifiant", IsUnique = true)]
    public partial class Avatar
    {
        [Key]
        [Column("AvatarID")]
        public int AvatarId { get; set; }
        [Column("UtilisateurID")]
        public int UtilisateurId { get; set; }
        public Guid Identifiant { get; set; }
        public byte[]? FichierAvatar { get; set; }
    }
}
