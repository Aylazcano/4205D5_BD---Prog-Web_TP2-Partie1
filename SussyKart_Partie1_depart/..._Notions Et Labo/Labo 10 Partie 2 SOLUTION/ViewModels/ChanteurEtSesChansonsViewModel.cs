using _4204D5_labo10.Models;

namespace _4204D5_labo10.ViewModels
{
    public class ChanteurEtSesChansonsViewModel
    {
        public Chanteur Chanteur { get; set; } = null!;
        public List<Chanson> Chansons { get; set; } = null!;
    }
}
