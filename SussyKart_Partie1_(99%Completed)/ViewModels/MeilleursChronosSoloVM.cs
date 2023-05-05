using Microsoft.AspNetCore.Mvc;

namespace SussyKart_Partie1.Models
{
    public class MeilleursChronosSoloVM
    {
        public string Joueur { get; set; } = null!;
        public string Course { get; set; } = null!;
        public int Position { get; set; }
        public int Chrono { get; set; }
        public DateTime DateParticipation { get; set; }
    }
}
