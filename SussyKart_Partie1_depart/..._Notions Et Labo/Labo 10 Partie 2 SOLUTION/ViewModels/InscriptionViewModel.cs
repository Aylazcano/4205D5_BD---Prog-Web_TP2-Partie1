using System.ComponentModel.DataAnnotations;

namespace _4204D5_labo10.ViewModels
{
    public class InscriptionViewModel
    {
        [Required(ErrorMessage = "Un nom d'utilisateur est requis.")]
        public string Pseudo { get; set; } = null!;

        [Required(ErrorMessage = "Un mot de passe est requis.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Le mot de passe doit avoir entre 6 et 50 caractères.")]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez confirmer le mot de passe.")]
        [DataType(DataType.Password)]
        [Compare(nameof(MotDePasse), ErrorMessage = "Les deux mots de passe sont différents.")]
        public string MotDePasseConfirmation { get; set; } = null!;

        [Required(ErrorMessage = "La couleur préférée est obligatoire.")]
        [StringLength(30, ErrorMessage = "Il doit bien y avoir une couleur de moins de 30 caractères que vous aimez ?")]
        public string CouleurPrefere { get; set; } = null!;
    }
}
