using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SussyKart_Partie1.Models
{
    [Keyless]
    [Table("CompteBancaire", Schema = "Utilisateurs")]
    public partial class CompteBancaire
    {
        [StringLength(20)]
        public string? NoCompteBancaire { get; set; }
    }
}
