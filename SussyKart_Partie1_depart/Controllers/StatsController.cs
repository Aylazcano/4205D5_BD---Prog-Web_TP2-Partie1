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
            var participations = await _context.VwDetailsParticipations.FromSqlRaw("SELECT * FROM Courses.VW_DetailsParticipations").OrderByDescending(p => p.DateParticipation).Take(30).ToListAsync();

            return View(new FiltreParticipationVM
            {
                DetailsParticipation = participations
            });
        }

        public IActionResult ToutesParticipationsFiltre(FiltreParticipationVM fpvm)
        {
            // Obtenir les participations grâce à une vue SQL
            IQueryable<VwDetailsParticipation> participationsQuery = _context.VwDetailsParticipations;

            if (fpvm.Pseudo != null)
            {
                // ...
                participationsQuery = participationsQuery.Where(p => p.Joueur == fpvm.Pseudo);
            }

            if (fpvm.Course != "Toutes")
            {
                // ...
                participationsQuery = participationsQuery.Where(p => p.Course == fpvm.Course);
            }

            // Trier soit par date, soit par chrono (fpvm.Ordre) de manière croissante ou décroissante (fpvm.TypeOrdre)
            if (fpvm.Ordre == "Date")
            {
                if (fpvm.TypeOrdre == "ASC")
                {
                    participationsQuery = participationsQuery.OrderBy(p => p.DateParticipation);
                }
                else
                {
                    participationsQuery = participationsQuery.OrderByDescending(p => p.DateParticipation);
                }
            }
            else
            {
                if (fpvm.TypeOrdre == "ASC")
                {
                    participationsQuery = participationsQuery.OrderBy(p => p.Chrono);
                }
                else
                {
                    participationsQuery = participationsQuery.OrderByDescending(p => p.Chrono);
                }
            }
            // Sauter des paquets de 30 participations si la page est supérieure à 1
            int pageSize = 30; // nombre de participations par page
            int pageNumber = fpvm.Page;
            fpvm.DetailsParticipation = participationsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View("ToutesParticipations", fpvm);
        }

        // Section 2 : Compléter ParticipationsParCourse OU ChronoParCourseParTour
        public async Task<IActionResult> ParticipationsParCourseAsync()
        {
            // Obtenir les participations grâce à une vue SQL
            //var participations = await _context.VwDetailsParticipations.FromSqlRaw("SELECT Course, COUNT(*) AS nbParticipation FROM Courses.VW_DetailsParticipations")
            //    .ToListAsync();
            IEnumerable<VwDetailsParticipation> participations = await _context.VwDetailsParticipations.FromSqlRaw("SELECT * FROM Courses.VW_DetailsParticipations").ToListAsync();

            var participationsQuery = from p in participations
                                      group p.NbJoueurs by p.Course into result
                                      select new { Course = result.Key, NbJoueurs = result.Count() };

            List<ParticipationVM> courseAvecNbParticipations = new List<ParticipationVM>();

            foreach(var p in participationsQuery)
            {
                courseAvecNbParticipations.Add(new ParticipationVM(p.Course, p.NbJoueurs));
            }

            return View(courseAvecNbParticipations);
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
