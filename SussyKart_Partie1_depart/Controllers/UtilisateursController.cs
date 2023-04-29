using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SussyKart_Partie1.Data;
using SussyKart_Partie1.Models;
using SussyKart_Partie1.ViewModels;
using System.Security.Claims;
using System.Security.Principal;

namespace SussyKart_Partie1.Controllers
{
    public class UtilisateursController : Controller
    {
        readonly TP2_SussyKartContext _context;
        public UtilisateursController(TP2_SussyKartContext context)
        {
            _context = context;
        }

        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InscriptionAsync(InscriptionVM ivm)
        {
            // Création d'un nouvel utilisateur

            // Le pseudo est déjà pris ?
            bool existeDeja = await _context.Utilisateurs.AnyAsync(x => x.Pseudo == ivm.Pseudo);
            if (existeDeja)
            {
                ModelState.AddModelError("Pseudo", "Ce pseudonyme est déjà pris.");
                return View(ivm);
            }

            // On INSERT l'utilisateur avec une procédure stockée qui va s'occuper de
            // hacher le mot de passe, chiffrer la NoCompteBancaire ...
            string query = "EXEC Utilisateurs.USP_CreerUtilisateur @Pseudo, @Courriel, @NoCompteBancaire, @MotDePasse";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudo", Value = ivm.Pseudo},
                new SqlParameter{ParameterName = "@Courriel", Value = ivm.Courriel},
                new SqlParameter{ParameterName = "@NoCompteBancaire", Value = ivm.NoBancaire},
                new SqlParameter{ParameterName = "@MotDePasse", Value = ivm.MotDePasse}
                
                // Autre syntaxe possible? Quelle est la différence? :
                //new SqlParameter("@Pseudo", ivm.Pseudo),
                //new SqlParameter("@Courriel", ivm.Courriel),
                //new SqlParameter("@NoCompteBancaire", ivm.NoBancaire),
                //new SqlParameter("@MotDePasse", ivm.MotDePasse)
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
            // Si l'inscription réussit :
            return RedirectToAction("Connexion", "Utilisateurs");
        }

        public IActionResult Connexion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Connexion(ConnexionVM cvm)
        {
            // Authentification d'un utilisateur
            string query = "EXEC Utilisateurs.USP_AuthUtilisateur @Pseudo, @MotDePasse";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudo", Value = cvm.Pseudo},
                new SqlParameter{ParameterName = "@MotDePasse", Value = cvm.MotDePasse}
            };
            Utilisateur? utilisateur = (await _context.Utilisateurs.FromSqlRaw(query, parameters.ToArray()).ToListAsync()).FirstOrDefault();
            if(utilisateur == null)
            {
                ModelState.AddModelError("", "Le pseudonyme ou le mot de passe est invalide.");
                return View(cvm);
            }

            // Si la connexion réussit :
            // On crée un cookie d'authentification
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilisateur.UtilisateurId.ToString()),
                new Claim(ClaimTypes.Name, utilisateur.Pseudo),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);

            return RedirectToAction("Index", "Jeu");
        }

        [HttpGet]
        public async Task<IActionResult> Deconnexion() 
        {
            // Cette ligne mange le cookie 🍪 Slurp
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Jeu");
        }

        // JUSTE SI AUTHENTIFIÉ SVP
        [Authorize]
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

            // Dans tous les cas, on doit envoyer un ProfilVM à la vue.
            return View(new ProfilVM() {
                Pseudo = utilisateur.Pseudo,
                DateInscription = utilisateur.DateInscription,
                Courriel = utilisateur.Courriel,
                // TODO:
                // 1.Creer une procédure pour déchiffrer le NoCompteBancaireChiffre dans Sql_Scripts (Semaine10Partie2.pptx, page 14);
                // 2.l'utiliser pour afficher le numero de compte bancaire.
                NoBancaire = "123456789"
            });
        }
    }
}
