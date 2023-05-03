using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _4204D5_labo9_mvc.Data;
using _4204D5_labo9_mvc.Models;
using _4204D5_labo9_mvc.ViewModels;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace _4204D5_labo9_mvc.Controllers
{
    public class ArtistesController : Controller
    {
        private readonly Lab09_EmployesContext _context;

        public ArtistesController(Lab09_EmployesContext context)
        {
            _context = context;
        }

        // GET: Artistes
        public async Task<IActionResult> Index()
        {
            var lab09_EmployesContext = _context.Artistes.Include(a => a.Employe);
            return View(await lab09_EmployesContext.ToListAsync());
        }

        public async Task<IActionResult> Index2()
        {
            IEnumerable<VwListeArtiste> artistes = await _context.VwListeArtistes.ToListAsync();
            return View(artistes);
        }

        // GET: Artistes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Artistes == null)
            {
                return NotFound();
            }

            var artiste = await _context.Artistes
                .Include(a => a.Employe)
                .FirstOrDefaultAsync(m => m.ArtisteId == id);
            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        // GET: Artistes/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Artiste, Employe")] ArtisteEmployeViewModel artisteEmploye)
        {
            string query = "EXEC Employes.USP_AjouterArtiste @Prenom, @Nom, @NoTel, @Courriel, @Specialite";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ ParameterName = "@Prenom", Value = artisteEmploye.Employe.Prenom},
                new SqlParameter{ ParameterName = "@Nom", Value = artisteEmploye.Employe.Nom},
                new SqlParameter{ ParameterName = "@NoTel", Value = artisteEmploye.Employe.NoTel},
                new SqlParameter{ ParameterName = "@Courriel", Value = artisteEmploye.Employe.Courriel},
                new SqlParameter{ ParameterName = "@Specialite", Value = artisteEmploye.Artiste.Specialite}
            };
            await _context.Database.ExecuteSqlRawAsync(query, parameters.ToArray());

            await _context.SaveChangesAsync();
            return View(artisteEmploye);
        }

        // GET: Artistes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Artistes == null)
            {
                return NotFound();
            }

            // On récupère l'artiste à modifier
            Artiste? artiste = await _context.Artistes.FindAsync(id);
            if (artiste == null)
            {
                return NotFound();
            }
            // On récupère aussi ses données d'employés pour pouvoir construire un ArtisteEmployeViewModel avec
            Employe? employe = await _context.Employes.FindAsync(artiste.EmployeId);
            if (employe == null)
            {
                return NotFound();
            }

            // On envoie à la vue toutes les données, qui seront affichées dans un formulaire
            return View(new ArtisteEmployeViewModel() { Artiste = artiste, Employe = employe });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Artiste, Employe")] ArtisteEmployeViewModel artisteEmploye)
        {
            // Ceci a été adapté un peu
            if (id != artisteEmploye.Artiste.ArtisteId)
            {
                return NotFound();
            }

            // ModelState ne fonctionne pas très bien avec le ViewModel, alors voici une alternative (2 lignes)
            // Si vous avez juste retiré l'usage de ModelState, c'est acceptable pour cet exercice !
            artisteEmploye.Artiste.Employe = artisteEmploye.Employe;
            bool isValid = Validator.TryValidateObject(artisteEmploye,
                new ValidationContext(artisteEmploye),
                new List<ValidationResult>(), true);

            if (isValid)
            {
                try
                {
                    // Ici, on a UPDATE séparément la partie Employe et la partie Artiste
                    _context.Artistes.Update(artisteEmploye.Artiste);
                    _context.Employes.Update(artisteEmploye.Employe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtisteExists(artisteEmploye.Artiste.ArtisteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // On a retiré la SelectList ici ! Et on retourne un ArtisteEmployeViewModel.
            return View(artisteEmploye);
        }

        // GET: Artistes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Artistes == null)
            {
                return NotFound();
            }

            var artiste = await _context.Artistes
                .Include(a => a.Employe)
                .FirstOrDefaultAsync(m => m.ArtisteId == id);
            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        // POST: Artistes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Artistes == null)
            {
                return Problem("Entity set 'Lab09_EmployesContext.Artistes'  is null.");
            }
            var artiste = await _context.Artistes.FindAsync(id);
            if (artiste != null)
            {
                _context.Artistes.Remove(artiste);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtisteExists(int id)
        {
            return (_context.Artistes?.Any(e => e.ArtisteId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Query1()
        {
            // Données des employés embauchés en 2023 (Utilisez VwListeArtiste)
            IEnumerable<VwListeArtiste> artistes = await _context.VwListeArtistes.Where(a => a.DateEmbauche.Year == 2023).ToListAsync();
            return View(artistes);
        }

        public async Task<IActionResult> Query2()
        {
            // Données des employés avec la spécialité "modélisation 3D" (Utilisez VwListeArtiste)
            IEnumerable<VwListeArtiste> artistes = await _context.VwListeArtistes.Where(a => a.Specialite == "modélisation 3D").ToListAsync();
            return View(artistes);
        }

        public async Task<IActionResult> Query3()
        {
            // Prénom et nom de tous les employés, classés par prénom ascendant
            IEnumerable<string> employes = await _context.Employes
                .OrderBy(e => e.Prenom)
                .Select(x => x.Prenom + " " + x.Nom).ToListAsync();
            return View(employes);
        }

        public async Task<IActionResult> Query4()
        {
            // Toutes les données des employés artistes (Sans VwListeArtiste)

            // Nécessaire à moins d'utiliser EFCore.Proxies
            IEnumerable<Employe> employes = await _context.Employes.ToListAsync();

            IEnumerable<ArtisteEmployeViewModel> artistes = await _context.Artistes
                .Select(x => new ArtisteEmployeViewModel { Artiste = x, Employe = x.Employe })
                .ToListAsync();
            return View(artistes);
        }

        public async Task<IActionResult> Query5()
        {
			
	        // Combien d'artistes par spécialité ?
			
			//On va chercher tous les artistes
            IEnumerable<Artiste> artistes = await _context.Artistes.ToListAsync();

            // .Distinct() permet de garder une seule copie de chaque spécialité !
            IEnumerable<string> specialites = artistes.Select(x => x.Specialite).Distinct().ToList();

            // Avec Select(), on construit une liste de NbSpecialiteViewModel en comptant combien 
            // il y a d'artistes pour chaque spécialité
            IEnumerable<NbSpecialiteViewModel> nbSpecialitesVM = specialites
                .Select(x => new NbSpecialiteViewModel { Specialite = x, Nb = artistes.Count(a => a.Specialite == x) });

            return View(nbSpecialitesVM);
        }

        public async Task<IActionResult> Query6()
        {
			//>Deux spécialités avec des longs prénoms
			
            // On fait une jointure d'artiste et d'employé
            IEnumerable<ArtisteEmployeViewModel> artistes = await _context.Artistes
                .Select(x => new ArtisteEmployeViewModel { Artiste = x, Employe = x.Employe })
                .ToListAsync();

            // .Distinct() permet de garder une seule copie de chaque spécialité !
            IEnumerable<string> specialites = artistes.Select(x => x.Artiste.Specialite).Distinct().ToList();

            // On construit les NbSpecialiteViewModel, mais cette fois on regarde les tailles moyennes des prénoms  (Nb n'est plus un count mais plus une longueur)
            IEnumerable<NbSpecialiteViewModel> nbSpecialites = specialites
                .Select(x => new NbSpecialiteViewModel { Specialite = x, Nb = (int?)Math.Round(artistes.Where(a => a.Artiste.Specialite == x).Average(a => a.Employe.Prenom.Length)) })
                .OrderByDescending(x => x.Nb)
                .Take(2);

            return View(nbSpecialites);
        }

      

    }
}
