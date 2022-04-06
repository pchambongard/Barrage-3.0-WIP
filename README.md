# Barrage-3.0-WIP

 ## Documentation du projet:

### Le projet est composé de plusieurs services qui communique entre eux:

  1- L'API Auth LDAP_AD est appelée par le site web Seulement lorsqu'une tentative de login est effectuée depuis la partie Admin du site (admin.blazor)
  
  2- L'API Barrage est appelée par:
    -> Le Service FTP (3) toutes les 5 minutes pour inserer les nouvelles données
    -> Le Service Temp (4) toutes les 5 minutes pour inserer les nouvelles données
    -> Le site Web (5) à chaque chargement de page nécéssitant des données conservées en base
    
 3- Le Service FTP est chargé de traduire les données stockés sur le fichier .csv alimenté par le service de prévention des crues et mis à disposition sur
    un serveur FTP (Toutes les infos se trouvent dans le fichier InfosFTP.cs)
    
 4- Le Service Temp est chargé de recupérer les données depuis l'API Du logiciel ThermotrackWebserve via une API KEY trouvable dans le fichier worker.cs
 5- Le Site Web affiche les données et l'état des barrages et des capteurs présents dans la base de données.
 
### Comment ajouter un utilisateur

  - Le service Auth vérifie que l'utilisateur est membre de :"CN=BARRAGE,OU=Globaux-Fonctionnels,OU=Groupes-globaux,OU=CG30-Groupes,OU=CG30-Usr-Grp,DC=intra,DC=cg30,DC=fr".
    Il faut donc juste intégrer l'utilisateur dans le groupe Barrage dans AD pour lui donner les droits de création et de modification de données.
    
### Modifier des composants

  - Depuis la page admin, un utilisateur authentifié membre du groupe Barrage pourra modifier des barrages, capteurs ou cotes d'exploitation.
    Il est possible de modifier la plupart des caractéristiques des barrages, de rendre actif ou inactif un capteur, de modifier les seuils des cotes d'exploitations etc..
    
    /!\ Il faut que chaque composant est un nom unique: (pour les capteurs et les cotes, l'unicité sera vérifiée par barrage, sauf pour les capteurs de température0
        - Pour les capteurs, il ne peut pas y avoir 2 capteurs principaux;
        - Pour les cotes il faut que le barrage n'ai pas d'autres cotes avec un indice de criticité plus faible et un seuil plus haut, et vice-versa
        
### Ajouter des composants

  - Depuis la page admin, un utilisateur authentifié membre du groupe Barrage pourra ajouter des barrages, capteurs ou cotes d'exploitation.
    Il est possible de definir la plupart des caractéristiques des barrages, des capteur, et dess cotes d'exploitations.
    
    /!\ Lors de l'ajout d'un capteur, il doit être rattaché à un barrage, et il n'est PAS possible de changer un capteur de barrage.
    /!\ Si un capteur est ajouté, il peut être alimenté automatiquement SI ET SEULEMENT SI:
        -> Il y a un capteur qui porte le MEME nom sur le MEME barrage dans le fichier mis à disposition par le SPC
        -> Si c'est un capteur de température, il doit seulement avoir le meme nom qu'un capteur ThermotrackWebserve
           MAIS tout les capteurs venant de ThermoTrackWebserve doivent avoir un nom différent, QU'IMPORTE LES BARRAGES AUXQUELS ILS SONT RATTACHES
    
    
## Base de données

### Hiérarchie des tables

  barrages:  
        -> id  
        -> id_type_barrage  
          => Table types_barrage -> id  
        -> nom *unique*
        -> usage  
        ....  
        
 capteurs:  
        -> id  
        -> id_type_capteur  
          => types_capteur -> id  
        -> id_barrage *unique en pair avec le nom*  
          => barrages -> id  
        -> nom *unique en pair avec l'id du barrage*  
        -> est_actif  
        -> est_principal  
        
  cotes_exploitation:  
        -> id  
        -> id_type_cote_exploitation  
          => types_cote_exploitation -> id  
        -> id_barrage *unique en pair avec le libelle*  
          => barrages -> id  
        -> seuil  
        -> criticité  
        -> libellé *unique en pair avec l'id du barrage*  
        
   types_barrage:  
        -> id  
        -> libellé  
        
   types_capteur:  
        -> id  
        -> libellé  
        
   types_cote_exploitation:  
        -> id  
        -> libellé  
        
   mesures:
        -> id_barrage  
          => barrages -> id  
        -> id_capteur *unique en pair avec le gdh*  
          => capteurs -> id  
        -> gdh ####unique en pair avec l'id capteur  
        -> valeur  
        -> debit_sortant  
        -> debit_entrant  
        -> volume  
        
   mesures_temperature:  
        -> id_capteur *unique en paire avec le gdh*  
          => capteurs -> id  
        -> gdh *unique en paire avec l'id capteur*  
        -> valeur  
        
        
 ### Vues
 
   Il y a une vue calculée dans la base : mesures jour.
    Elle calcule automatiquement la pluviometrie journaliere de chaque capteur de type pluvio (id_type_capteur == 2)
