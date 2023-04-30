using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SussyKart_Partie1.Data;
using SussyKart_Partie1.Models;
using SussyKart_Partie1.ViewModels;

namespace SussyKart_Partie1.Controllers
{
    public class StatsController : Controller
    {
        readonly TP2_SussyKartContext _context;
        public StatsController(TP2_SussyKartContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        // Section 1 : Compléter ToutesParticipations (Obligatoire)
        public async Task<IActionResult> ToutesParticipations()
        {
            // Obtenir les participations grâce à une vue SQL
            var participations = await _context.VwDetailsParticipations.FromSqlRaw("SELECT * FROM Courses.VW_DetailsParticipations").Take(30).ToListAsync();

            return View(new FiltreParticipationVM
            {
                DetailsParticipation = participations
            });
        }
        //public async Task<IActionResult> ToutesParticipations(int? page)
        //{
        //    int pageSize = 30; // nombre de participations par page
        //    int pageNumber = (page ?? 1); // numéro de la page à afficher (1 par défaut)

        //    // récupérer les participations grâce à une vue SQL qui gère les jointures
        //    var participations = await _context.VwDetailsParticipations.ToListAsync();

        //    // trier les participations par ordre décroissant de la date
        //    participations = participations.OrderByDescending(p => p.DateParticipation).ToList();

        //    // paginer les résultats
        //    var paginatedParticipations = participations.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        //    // stocker les participations paginées dans la liste ajoutée à FiltreParticipationVM
        //    var vm = new FiltreParticipationVM
        //    {
        //        DetailsParticipation = paginatedParticipations
        //    };

        //    return View(vm);
        //}


        public IActionResult ToutesParticipationsFiltre(FiltreParticipationVM fpvm)
        {
            // Obtenir les participations grâce à une vue SQL

            if(fpvm.Pseudo != null)
            {
                // ...
            }

            if(fpvm.Course != "Toutes")
            {
                // ...
            }

            // Trier soit par date, soit par chrono (fpvm.Ordre) de manière croissante ou décroissante (fpvm.TypeOrdre)

            // Sauter des paquets de 30 participations si la page est supérieure à 1

            return View("ToutesParticipations", fpvm);
        }

        // Section 2 : Compléter ParticipationsParCourse OU ChronoParCourseParTour
        public IActionResult ParticipationsParCourse()
        {
            return View();
        }

        public IActionResult ChronoParCourseParTour()
        {
            return View();
        }

        // Section 3 : Compléter MeilleursChronosSolo ou MeilleursChronosQuatre
        public IActionResult MeilleursChronosSolo()
        {
            return View();
        }

        public IActionResult MeilleursChronosQuatre()
        {
            return View();
        }
    }
}
