using System.ComponentModel.DataAnnotations;

namespace _4204D5_labo10.ViewModels
{
    public class ConnexionViewModel
    {
        [Required(ErrorMessage = "Veuillez préciser un nom d'utilisateur.")]
        public string Pseudo { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez entrer un mot de passe.")]
        public string MotDePasse { get; set; } = null!;
    }
}
