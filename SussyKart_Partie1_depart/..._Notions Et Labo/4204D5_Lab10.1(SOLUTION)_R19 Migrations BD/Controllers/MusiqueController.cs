using _4204D5_labo10.Data;
using _4204D5_labo10.Models;
using _4204D5_labo10.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace _4204D5_labo10.Controllers
{
    public class MusiqueController : Controller
    {
        readonly Lab10Context _context;

        public MusiqueController(Lab10Context context)
        {
            _context = context;
        }

        public /*async Task<*/IActionResult/*>*/ Index()
        {
            // Manière habituelle de récupérer un utilisateur (Migration 1.4)
            /*ViewData["utilisateur"] = "visiteur";
            IIdentity? identite = HttpContext.User.Identity;
            if (identite != null && identite.IsAuthenticated)
            {
                string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
                if (utilisateur != null)
                {
                    // Pour dire "Bonjour X" sur l'index
                    ViewData["utilisateur"] = utilisateur.Pseudo;
                }
            }*/
            return View();
        }

        public async Task<IActionResult> ChanteursEtChansons()
        {
            // On va récupérer toutes les chansons et les chanteurs pour les envoyer à la vue
            ChanteursChansonsViewModel ccvm = new ChanteursChansonsViewModel(){
                Chanteurs = await _context.Chanteurs.ToListAsync(),
                Chansons = await _context.Chansons.ToListAsync()
            };
            return View(ccvm);
        }

        public async Task<IActionResult> Chanteurs()
        {
            // À cause du lazy loading, on charge les chansons de la BD :
            List<Chanson> chansons = await _context.Chansons.ToListAsync();

            // Au départ:
            // Ensuite on va chercher les chanteurs ET on compte leur nombre de chansons pour chacun
            //List<ChanteurEtNbChansonsViewModel> cencvm = await _context.Chanteurs
            //    .Select(x => new ChanteurEtNbChansonsViewModel() { 
            //        Chanteur = x,
            //        NbChansons = x.Chansons.Count
            //    }).ToListAsync();

            // Avec la migration 1.2 et la création de la vue dans la BD
            // return View(await _context.VwChanteurNbChansons.ToListAsync());
            return View(await _context.VwChanteurNbChansons.ToListAsync());
        }

        public async Task<IActionResult> UnChanteurEtSesChansons(string chanteurRecherche)
        {
            //  Trouver un chanteur par son nom. Pas sensible à la casse
            Chanteur? chanteur = await _context.Chanteurs.Where(x => x.Nom.ToUpper() == chanteurRecherche.ToUpper()).FirstOrDefaultAsync();
            if(chanteur == null)
            {
                ViewData["chanteurNonTrouve"] = "Cet artiste n'existe pas.";
                return RedirectToAction("Index", "Musique");
            }
            // Obtenir la liste des chansons du chanteur (Sera modifié à la migration 1.3)
            // La fouille est basée sur le titre de la chanson au lieu de son id...
            // Au départ: List<Chanson> chansons = await _context.Chansons.Where(x => x.NomChanteur == chanteur.Nom).ToListAsync();
            // Après la migration 1.0:
            //  List<Chanson> chansons = await _context.Chansons.Where(x => x.ChanteurId == chanteur.ChanteurId).ToListAsync();
            // Après la migration 1.3:
           

            string query = "EXEC Musique.USP_ChanteurChansons @ChanteurID";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@ChanteurID", Value = chanteur.ChanteurId}
            };

            List<Chanson> chansons = await _context.Chansons.FromSqlRaw(query, parameters.ToArray()).ToListAsync();

            return View(new ChanteurEtSesChansonsViewModel()
            {
                Chanteur = chanteur,
                Chansons = chansons
            });
        }

        [HttpPost]
        //[Authorize]
        public /*async Task<*/IActionResult/*>*/ AjouterFavori(int chanteurId)
        {
            // Manière standard de récupérer l'utilisateur
            /*IIdentity? identite = HttpContext.User.Identity;
            string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
            if (utilisateur != null)
            {
                // L'id du chanteur fourni existe-t-il ?
                Chanteur? chanteur = await _context.Chanteurs.FirstOrDefaultAsync(x => x.ChanteurId == chanteurId);
                if(chanteur != null)
                {
                    // Le chanteur est-il déjà dans les favoris de l'utilisateur ?
                    bool dejaFavori = await _context.ChanteurFavoris
                        .AnyAsync(x => x.ChanteurId == chanteur.ChanteurId && x.UtilisateurId == utilisateur.UtilisateurId);
                    if (!dejaFavori)
                    {
                        // Okay ! On construit une rangée dans la table de liaison entre utilisateur et chanteur
                        ChanteurFavori favori = new ChanteurFavori()
                        {
                            ChanteurId = chanteur.ChanteurId,
                            Chanteur = chanteur,
                            UtilisateurId = utilisateur.UtilisateurId,
                            Utilisateur = utilisateur
                        };
                        // On l'ajoute à la BD
                        _context.ChanteurFavoris.Add(favori);
                        await _context.SaveChangesAsync();
                    }                 
                }
            }*/
            // On retourne à la page où on était
            return RedirectToAction("Chanteurs", "Musique");
        }
    }
}
