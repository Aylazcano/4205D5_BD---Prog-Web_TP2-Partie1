using _4204D5_labo10.Models;

namespace _4204D5_labo10.ViewModels
{
    public class UtilisateurEtFavorisViewModel
    {
        // Nécessaire à partir de la migration 1.5
        //public Utilisateur Utilisateur { get; set; } = null!;

        public List<Chanteur> ChanteursFavoris { get; set; } = null!;
    }
}
