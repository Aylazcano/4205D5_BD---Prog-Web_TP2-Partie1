-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•
--				Optimisation
-- •○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•○•

USE TP2_SussyKart;
GO

CREATE NONCLUSTERED INDEX IX_Utilisateur_UtilisateurId ON Utilisateurs.Utilisateur(UtilisateurId);
--Cet index est créé sur la colonne "Pseudo" de la table "Utilisateur" pour optimiser les recherches d'utilisateurs en fonction de leur pseudo. 
--Étant donné que les utilisateurs de SussyKart sont susceptibles d'avoir de nombreux amis, il est possible qu'il y ait un grand nombre de recherches basées sur les pseudos. 
--Cet index permettra de réduire le temps de calcul nécessaire pour ces recherches, améliorant ainsi les performances.

CREATE NONCLUSTERED INDEX IX_Utilisateur_Pseudo ON Utilisateurs.Utilisateur(Pseudo);
--Cet index est créé sur la colonne "UtilisateurId" de la table "Utilisateur". 
--Bien que les recherches d'utilisateurs ne soient pas toujours effectuées en utilisant leur pseudo, il peut arriver qu'elles soient basées sur leur identifiant d'utilisateur. 
--Cet index permet d'accélérer les recherches basées sur l'identifiant d'utilisateur, offrant ainsi des performances améliorées dans de telles situations.

CREATE NONCLUSTERED INDEX IX_ParticipationCourse_ParticipationCourseID ON Courses.ParticipationCourse(ParticipationCourseID);
--Cet index est créé sur la colonne "ParticipationCourseID" de la table "ParticipationCourse" dans le cadre des statistiques de participation aux courses. 
--Lorsque les utilisateurs effectuent des recherches dans ces statistiques, il est préférable que les requêtes s'exécutent rapidement. 
--Cet index permettra d'accélérer les recherches basées sur l'identifiant de participation à une course, ce qui améliorera les performances lors de l'accès aux statistiques de participation.


--En ajoutant ces index, vous optimisez les performances des requêtes fréquentes de votre application, réduisant ainsi le temps nécessaire pour récupérer les données et offrant une meilleure expérience utilisateur.






