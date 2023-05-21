-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Optimisation
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE NONCLUSTERED INDEX IX_Utilisateur_UtilisateurId ON Utilisateurs.Utilisateur(UtilisateurId); 
--Il y a beaucoup de recherches basées sur le identifiant d'utilisateur dans le programme. 
--Ceci réduira le temps de calcul pour ces recherches, améliorant ainsi les performances.

CREATE NONCLUSTERED INDEX IX_Utilisateur_Pseudo ON Utilisateurs.Utilisateur(Pseudo);
--Les utilisateurs de SussyKart peuvent et vont probablement avoir plusieurs amis, il y aura possiblement un grand nombre de recherches basées sur les pseudos. 
--Ceci réduira le temps de calcul pour ces recherches, améliorant ainsi les performances.

CREATE NONCLUSTERED INDEX IX_ParticipationCourse_ParticipationCourseID ON Courses.ParticipationCourse(ParticipationCourseID);
--On veux que les requêtes s'exécutent rapidement quand les utilisateurs effectuent des recherches statistiques. 
--Ceci permettra d'accélérer les recherches basées sur l'identifiant de participation à une course et améliorera les performances à l'accès aux statistiques de participation.





