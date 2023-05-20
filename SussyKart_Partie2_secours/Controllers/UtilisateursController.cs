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
        readonly private TP2_SussyKartContext _context;

        public UtilisateursController(TP2_SussyKartContext context)
        { 
            _context = context; 
        }

        public IActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Inscription(InscriptionVM ivm)
        {
            // Pseudo est-il déjà pris ?
            bool existeDeja = await _context.Utilisateurs.AnyAsync(x => x.Pseudo == ivm.Pseudo);
            if (existeDeja)
            {
                ModelState.AddModelError("Pseudo", "Ce nom d'utilisateur est déjà pris.");
                return View(ivm);
            }

            // Préparation de l'appel de la procédure stockée
            string query = "EXEC Utilisateurs.USP_CreerUtilisateur @Pseudo, @Courriel, @MotDePasse";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Pseudo", Value = ivm.Pseudo},
                new SqlParameter{ParameterName = "@MotDePasse", Value = ivm.MotDePasse},
                new SqlParameter{ParameterName = "@Courriel", Value = ivm.Courriel}
            };

            // Appel de la procédure stockée
            try
            {
                await _context.Database.ExecuteSqlRawAsync(query, parameters.ToArray());
            }
            catch(Exception){
                ModelState.AddModelError("", "Une erreur est survenue. Veuillez réessayer plus tard.");
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
            if (utilisateur == null)
            {
                ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe invalide.");
                return View(cvm);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilisateur.UtilisateurId.ToString()),
                new Claim(ClaimTypes.Name, utilisateur.Pseudo)
            };

            ClaimsIdentity identite = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identite);
            await HttpContext.SignInAsync(principal);

            // Si la connexion réussit :
            return RedirectToAction("Index", "Jeu");
        }

        [Authorize]
        public async Task<IActionResult> Deconnexion() {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Jeu");
        }

        [Authorize]
        public async Task<IActionResult> Profil()
        {
            IIdentity? identite = HttpContext.User.Identity;
            if (identite != null && identite.IsAuthenticated)
            {
                string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);

                // (AYL) Nécessaire pour: Si l’utilisateur possède un avatar, l’envoyer à la vue via le ProfilVM.
                Avatar? avatar = await _context.Avatars.FirstOrDefaultAsync(x => x.UtilisateurId == utilisateur.UtilisateurId);

                if (utilisateur != null)
                {
                    ProfilVM profilVM = new ProfilVM()
                    {
                        // Si l’utilisateur possède un avatar, l’envoyer à la vue via le ProfilVM.
                        ImageUrl = avatar?.FichierAvatar != null ? $"data:image/png;base64,{Convert.ToBase64String(avatar.FichierAvatar)}" : null,

                        Pseudo = utilisateur.Pseudo,
                        DateInscription = utilisateur.DateInscription,
                        Courriel = utilisateur.Courriel,
                        NbrAmis = _context.Amities.Where(a => a.UtilisateurId == utilisateur.UtilisateurId).Count()
                    };

                    ViewData["UtilisateurID"] = utilisateur.UtilisateurId;
                    return View(profilVM);
                }
            }
            return NotFound();
        }
		
		// Action qui mène vers une vue qui permet de choisir un avatar pour son profil.
		[Authorize]
		public async Task<IActionResult> Avatar()
		{
            // Trouver l'utilisateur grâce à son cookie.
            IIdentity? identite = HttpContext.User.Identity;
            if(identite!= null && identite.IsAuthenticated)
            {
                string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
                
                //// (AYL) Alternative plus courte.
                //Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == identite.Name);

                // Si utilisateur trouvé, retourner la vue Avatar avec un ImageUploadVM qui contient le bon UtilisateurID.
                if (utilisateur != null)
                {
                    var imageUploadVM = new ImageUploadVM { UtilisateurID = utilisateur.UtilisateurId };
                    return View("Avatar",imageUploadVM);
                }
            }

            // Sinon, retourner la vue Connexion
            return View("Connexion");
		}

		// Action qui est appelée suite à l'envoi d'un formulaire et qui change l'avatar
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Avatar(ImageUploadVM iuvm)
		{
            // Trouver l'utilisateur grâce à son cookie
            IIdentity? identite = HttpContext.User.Identity;
            if (identite != null && identite.IsAuthenticated)
            {
                string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
                Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);
                Avatar? avatar = _context.Avatars.FirstOrDefault(x => x.UtilisateurId == utilisateur.UtilisateurId);

                // Si aucun utilisateur trouvé, retourner la vue Connexion
                if (utilisateur == null)
                    return View("Connexion");

                if(ModelState.IsValid)
                {
                    // Si le FormFile contient bel et bien un fichier, ajouter / remplacer 
                    // un avatar dans la BD et retourner au Profil.
                    
                    if (iuvm.FormFile != null && iuvm.FormFile.Length >= 0)
                    {
                        MemoryStream stream = new MemoryStream();
                        await iuvm.FormFile.CopyToAsync(stream);
                        byte[] fichierAvatar = stream.ToArray();

                        if (avatar == null)
                        {
                            await _context.Avatars.AddAsync(new Avatar { UtilisateurId = utilisateur.UtilisateurId, FichierAvatar = fichierAvatar});
                            await _context.SaveChangesAsync();
                        }
                        else 
                        {
                            avatar.FichierAvatar = fichierAvatar;
                            await _context.SaveChangesAsync();
                        }
                        return RedirectToAction("Profil");
                    }
                }
            }
            return RedirectToAction("Avatar");
        }

		// Action qui mène vers une vue qui affiche notre liste d'amis et permet d'en ajouter de nouveaux.
		[Authorize]
		public async Task<IActionResult> Amis()
		{
            // Trouver l'utilisateur grâce à son cookie
            IIdentity? identite = HttpContext.User.Identity;

            // Si aucun utilisateur trouvé, retourner la vue Connexion
            if (identite == null) 
                return View("Connexion");

            // Sinon, retourner la vue Amis en lui transmettant une liste d'AmiVM
            string pseudo = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            Utilisateur? utilisateur = await _context.Utilisateurs.FirstOrDefaultAsync(x => x.Pseudo == pseudo);

            List<Amitie> amis = _context.Amities.Where(a => a.UtilisateurId == utilisateur.UtilisateurId).ToList();
            List<AmiVM> amiVMs = new List<AmiVM>();
            foreach (var a in amis)
            {
                AmiVM amiVM = new AmiVM();
                Utilisateur? ami = _context.Utilisateurs.SingleOrDefault(u => u.UtilisateurId == a.UtilisateurIdAmi);
                Avatar? avatarAmi = await _context.Avatars.FirstOrDefaultAsync(x => x.UtilisateurId == ami.UtilisateurId);

                amiVM.AmiID = ami.UtilisateurId;
                amiVM.Pseudo = ami.Pseudo;
                amiVM.DateInscription = ami.DateInscription;

                amiVM.ImageUrl = avatarAmi != null ? $"data:image/png;base64,{Convert.ToBase64String(avatarAmi.FichierAvatar)}" : null;

                ParticipationCourse? derniereParticipation = _context.ParticipationCourses
                    .Where(pc => pc.UtilisateurId == ami.UtilisateurId)
                    .OrderByDescending(pc => pc.DateParticipation)
                    .FirstOrDefault();

                amiVM.DernierePartie = derniereParticipation?.DateParticipation ?? DateTime.MinValue;


                amiVMs.Add(amiVM);
            }

            // De plus, glisser dans ViewData["utilisateurID"] l'id de l'utilisateur qui a appelé l'action. (Car c'est utilisé dans Amis.cshtml)
            ViewData["utilisateurID"] = utilisateur?.UtilisateurId;

            return View(amiVMs);
        }

        // Action appelée lorsque le formulaire pour ajouter un ami est rempli
        [Authorize]
		[HttpPost]
		public async Task<IActionResult> AjouterAmi(int utilisateurID, string pseudoAmi)
		{
            // Trouver l'utilisateur qui a appelé l'action ET l'utilisateur qui sera ajouté en ami
            Utilisateur? utilisateur = await _context.Utilisateurs.FindAsync(utilisateurID);
            Utilisateur? ami = await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Pseudo == pseudoAmi);

            // Si l'utilisateur qui appelle l'action n'existe pas, retourner la vue Connexion.
            if (utilisateur == null)
                return View("Connexion");

            // Si l'ami à ajouter n'existe pas rediriger vers la vue Amis.
            if (ami == null)
                return RedirectToAction("Amis");

            // Si l'ami ne faisait pas déjà partie de la liste, créer une nouvelle amitié et l'ajouter dans la BD.
            bool amiExiste = await _context.Amities.AnyAsync(a => a.UtilisateurId == utilisateur.UtilisateurId && a.UtilisateurIdAmi == ami.UtilisateurId);

            if (!amiExiste)
            {
                // Créer une nouvelle amitié entre les deux utilisateurs
                Amitie nouvelleAmitie = new Amitie
                {
                    UtilisateurId = utilisateur.UtilisateurId,
                    UtilisateurIdAmi = ami.UtilisateurId
                };

                // Ajouter la nouvelle amitié dans la base de données
                _context.Amities.Add(nouvelleAmitie);
                await _context.SaveChangesAsync();
            }

            // Puis, dans tous les cas, rediriger vers la vue Amis.
            return RedirectToAction("Amis");
		}

		// Action qui est appelée lorsqu'on appuie sur le bouton qui supprime un ami
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> SupprimerAmi(int utilisateurID, int amiID)
		{
            // Trouver l'utilisateur qui a appelé l'action ET l'utilisateur qui sera retiré des amis
            Utilisateur? utilisateur = await _context.Utilisateurs.FindAsync(utilisateurID);
            Utilisateur? ami = await _context.Utilisateurs.FindAsync(amiID);

            // Si l'utilisateur qui appelle l'action n'existe pas, retourner la vue Connexion.
            if (utilisateur == null)
                return View("Connexion");
							
			// Si l'ami à ajouter n'existe pas rediriger vers la vue Amis.
            if (ami == null)
			    return RedirectToAction("Amis");

            // Supprimer l'amitié de la BD et redirigrer vers la vue Amis.
            Amitie? amitie = await _context.Amities.FirstOrDefaultAsync(a =>
                (a.UtilisateurId == utilisateur.UtilisateurId && a.UtilisateurIdAmi == ami.UtilisateurId) ||
                (a.UtilisateurId == ami.UtilisateurId && a.UtilisateurIdAmi == utilisateur.UtilisateurId));
            
            _context.Amities.Remove(amitie);
            await _context.SaveChangesAsync();

            return RedirectToAction("Amis");
		}

		// Action qui est appelée lorsqu'un utilisateur appuie sur le bouton qui supprime son compte
		[Authorize]
		[HttpPost]
        public async Task<IActionResult> DesactiverCompte(int utilisateurID)
        {
            // Trouver l'utilisateur avec l'id utilisateurID et s'il n'existe pas retourner la vue Connexion
            var utilisateur = await _context.Utilisateurs.FindAsync(utilisateurID);
            if (utilisateur == null)
                return View("Connexion");

            // " Suppimer " l'utilisateur de la BD. Votre déclencheur fera le reste.
            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();

            // await HttpContext.SignOutAsync(); Même si mettre cette ligne de code serait judicieux, ne pas le faire !
            return RedirectToAction("Index", "Jeu");
		}
    }
}
