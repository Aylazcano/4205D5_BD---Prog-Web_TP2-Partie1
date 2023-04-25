using _4204D5_labo10.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Security.Principal;
using _4204D5_labo10.Data;
using _4204D5_labo10.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace _4204D5_labo10.Controllers
{
    public class UtilisateursController : Controller
    {
        readonly Lab10Context _context;
        public UtilisateursController(Lab10Context context)
        {
            _context = context;
        }

        public IActionResult Inscription()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> Inscription(InscriptionViewModel ivm)
        {
            // Le pseudo est déjà pris ?
            bool existeDeja = await _context.Utilisateurs.AnyAsync(x => x.Pseudo == ivm.Pseudo);
            if (existeDeja)
            {
                ModelState.AddModelError("Pseudo", "Ce pseudonyme est déjà pris.");
                return View(ivm);
            }

            // On INSERT l'utilisateur avec une procédure stockée qui va s'occuper de
            // hacher le mot de passe, chiffrer la couleur ...
            string query = "EXEC Utilisateurs.USP_CreerUtilisateur @Pseudonyme, @MotDePasse, @Couleur";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudo", Value = ivm.Pseudo},
                new SqlParameter{ParameterName = "@MotDePasse", Value = ivm.MotDePasse},
                new SqlParameter{ParameterName = "@Couleur", Value = ivm.CouleurPrefere}
            };
            try
            {
                await _context.Database.ExecuteSqlRawAsync(query, parameters.ToArray());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Une erreur est survenue. Veuillez réessayez.");
                return View(ivm);
            }
            return RedirectToAction("Connexion", "Utilisateurs");
        }*/

        public IActionResult Connexion()
        {
            return View();
        }

        /*[HttpPost]
        public async Task<IActionResult> Connexion(ConnexionViewModel cvm)
        {
            // Procédure stockée qui compare le mot de passe fourni à celui dans la BD
            // Retourne juste l'utilisateur si le mot de passe est valide
            string query = "EXEC Utilisateurs.USP_AuthUtilisateur @Pseudo, @MotDePasse";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudo", Value = cvm.Pseudo},
                new SqlParameter{ParameterName = "@MotDePasse", Value = cvm.MotDePasse}
            };
            Utilisateur? utilisateur = (await _context.Utilisateurs.FromSqlRaw(query, parameters.ToArray()).ToListAsync()).FirstOrDefault();
            if (utilisateur == null)
            {
                ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe invalide");
                return View(cvm);
            }

            // Construction du cookie d'authentification 
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilisateur.UtilisateurId.ToString()),
                new Claim(ClaimTypes.Name, utilisateur.Pseudo)
            };

            ClaimsIdentity identite = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identite);

            // Cette ligne fournit le cookie à l'utilisateur
            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Musique");
        }*/

        [HttpGet]
        public async Task<IActionResult> Deconnexion()
        {
            // Cette ligne mange le cookie 🍪 Slurp
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Musique");
        }

        /*
        // JUSTE SI AUTHENTIFIÉ SVP
        public async Task<IActionResult> Profil()
        {
            // Manière habituelle de récupérer un utilisateur
            IIdentity? identite = HttpContext.User.Identity;
            string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
            if (utilisateur == null) // Utilisateur supprimé entre-temps ... ?
            {
                return RedirectToAction("Connexion", "Utilisateurs");
            }

            return View(utilisateur); // Remplacer cette ligne une fois à la version 1.5
        }*/

            // FIN alternative à l'action Profil() pour la migration 1.5
            /*
            // Récupérer les chanteurs favoris de l'utilisateur pour les afficher dans le profil
            List<ChanteurFavori> favoris = await _context.ChanteurFavoris.ToListAsync();
            List<Chanteur> chanteurs = await _context.Chanteurs
                .Where(x => x.ChanteurFavoris
                .Any(y => y.UtilisateurId == utilisateur.UtilisateurId)).ToListAsync();
            return View(new UtilisateurEtFavorisViewModel()
            {
                Utilisateur = utilisateur,
                ChanteursFavoris = chanteurs
            });
            */

        /*[HttpPost]
        [Authorize]
        public async Task<IActionResult> Couleur(string motDePasse)
        {
            // Méthode habituelle pour récupérer l'utilisateur qui fait la requête
            IIdentity? identite = HttpContext.User.Identity;
            string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
            if (utilisateur == null) // Utilisateur supprimé entre-temps ... ?
            {
                return RedirectToAction("Connexion", "Utilisateurs");
            }

            // Exécuter la procédure stockée pour récupérer la couleur déchiffrée
            string query = "EXEC Utilisateurs.USP_Couleur @Pseudo, @MotDePasse";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudo", Value = utilisateur.Pseudo},
                new SqlParameter{ParameterName = "@MotDePasse", Value = motDePasse}
            };
            Couleur? couleur = (await _context.Couleurs.FromSqlRaw(query, parameters.ToArray()).ToListAsync()).FirstOrDefault();
            if(couleur != null)
            {
                // On passe la couleur par le ViewData (ou le ViewBag)
                ViewData["couleur"] = couleur.Couleur1;
            }

            // Récupérer les chanteurs favoris. Un peu répétitif, ça aurait pu être dans une fonction vu
            // que c'est utilisé 2 fois.
            List<ChanteurFavori> favoris = await _context.ChanteurFavoris.ToListAsync();
            List<Chanteur> chanteurs = await _context.Chanteurs
                .Where(x => x.ChanteurFavoris
                .Any(y => y.UtilisateurId == utilisateur.UtilisateurId)).ToListAsync();

            // On retourne la vue profil (car c'est de là que l'utilisateur arrivait), sauf que
            // la couleur sera potentiellement affichée cette fois-ci.
            return View("Profil", new UtilisateurEtFavorisViewModel()
            {
                Utilisateur = utilisateur,
                ChanteursFavoris = chanteurs
            });
        }*/
    }
}
