-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Optimisation
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE NONCLUSTERED INDEX IX_Utilisateur_UtilisateurId ON Utilisateurs.Utilisateur(UtilisateurId);
--Commentaires a ajouter
--

CREATE NONCLUSTERED INDEX IX_Utilisateur_Pseudo ON Utilisateurs.Utilisateur(Pseudo);
--Commentaires a ajouter
--

CREATE NONCLUSTERED INDEX IX_ParticipationCourse_ParticipationCourseID ON Courses.ParticipationCourse(ParticipationCourseID);
--Commentaires a ajouter
--