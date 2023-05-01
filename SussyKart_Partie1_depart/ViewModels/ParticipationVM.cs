using SussyKart_Partie1.Models;
using System.ComponentModel.DataAnnotations;

namespace SussyKart_Partie1.ViewModels
{
    public class ParticipationVM
    {
        [Required]
        [Range(1, 4)]
        public int Position { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Chrono { get; set; }

        [Required]
        [Range(1, 4)]
        public int NbJoueurs { get; set; }

        [Required]
        public string NomCourse { get; set; } = null!;

        public List<ParticipationVM> ParticipationsCoursesVM { get; set; } = new List<ParticipationVM>();
        public ParticipationVM(string course, int nbJoueurs)
        {
            NomCourse = course;
            NbJoueurs = nbJoueurs;
        }

    }
}
