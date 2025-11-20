
# FAI COSE
- metti collisioni player
- comunica al player che e' invincibile
- cambia fsmplayer come fsmsimplenemy
- aggiungi animazion caduta scudo
- sistema outline nemico
- ritarda colldier player MEDIUM PRIORITY
- i mage enemy dovrebbero interagire fra di loro, coordinarsi.
- anima dodge in modo diverso, magari con un particellars

# BUGS
- sistema collision detection 
- sistema intelligenza artificiosa
- se io grabbo un nemico ma allo stesso tempo c'e' un nemico che attacca lui mi stunna, ma non deve andare cosi', se io grabbo non devo essere vulnerabile agli altri cristiani duratne il grab, magari tolgo lo stun? ✅✅✅
- Magari metto che devo comunque uccidere i tizi mini? SI!!!
- Se colpisco mentre nemici caricano attacco?
- Se dodgio dentro attacco nemico?
- Ogni tanto il dodge si rompe, e non cambia stato ma si trigghera lo stato
- Controlla che le collisioni coincidano (algoritmo checkbox genera boxcollider correttamente)
- Ogni tanto non parte le animazioni del grab ✅
- la testa rimaneva girata in alcune animazioni ✅✅✅
- I nemici possono uscire dalla zona di combattimento

FIRST MILESTONE
- Termina modelli 3D
- Programma in modo grezzo il player 
*Linka file canva*
## PROGRAMMING TODOS

#### Fundamentals
- Termina IA nemici
- Termina collisioni
- sistema rotator nemici.

#### Major
- Sistema GetCircumnferencePointInRange, rendilo piu' preciso.
- Instanzia particellari in ogni attacco
- Fai cutscenes
- Potrebbe esserci un problema quando una levelSection viene distrutta: activeLevelSection in EnemyManager deve essere settata a nullptr appena la levelSection viene distrutta; ma se venisse distrutta con delay e nel frattempo una nuova levelSection viene settata? activeLevelSection dovrebbe essere settata a nullptr SOLO SE sta puntando alla scena che sta venendo distrutta, per ora questo problema non si pone ma e' importante che io scriva sta roba se mai dovessi fare una modifica del genere. 
- non fare camminare il player mentre e' per terra
###### Enemies
- Istanzia particellari attacco
- Gestisci collisioni
- Gestisci problema dell'intelligenza artificiale: se il nemico dovesse avere un punto al di fuori del terreno percorribile?
#### Minor
- Aggiungi particellari nemici.
- Negli Stati del player evita che sia necessario assegnare una reference a player ogni frame
- Aggiungi un GOOD JOB a fine levelSection
- tutorial animati 

##### Problemi futuri
- isInvincible viene settato ogni frame, significa che tutto cio' che modifichera' questa variabile, non avra' effetto
## ART TODOS

#### Fundamentals
- Finire: trono, camino, tavolo, tende, alberi, giocattoli, SE RIESCI ecc...  
- finire modellazione mappa di gioco
- finire disegno troni
- finire disegno mappa
- particellari
- modello bambino
- modello nemico base
- hud
#### Major
- Trova narratore
- Determina come fare le cutscenes
#### Minor
- Aggiungi particellare appena spawnano i nemici
- Aggiusta animazione di camminata
## BUGS 
- MITIGATO DAL FATTO CHE SIA UNA SMOOTH ROTATION 😊
  il player dovrebbe ruotare meglio. ES: se premo "w" ed "d" contemporaneamente e rilascio, anche una minima differenza nel rilascio fa ruotare il player verso o la "d" o la "w", quando in realta' si avrebbe voluto ruotare verso il senso di "w" e "d". 




## Fixacci veloci
- Negli Stati del player evita che sia necessario assegnare una reference a player ogni frame

# Idee
- Parte in cui devi saltare su dei blocchi di lava
- Parte in cui devi inseguire coniglio per ottenere chiave entrata castello 
- Piccolo enigma all'uscita del villaggio
- PArte finale remix discoteca
- boss finale
- power up semplici che fanno pi\ danno oppure cambiano le combo oppure aggiungono potenziamenti alle armi.


# Cose esterne prese
- ToonShader
- OutlineScript/OutlineShader
- Gli assets della UI sono stati scaricati da internet, MA animati e modificati da me ( (aggiunta titolo gioco, animazione svolazzante, outline, ecc...). Ps. li ho comprati
- Castello SOLO ESTERNO
- font selezionati e scaricati da internet
- Se mi e' sfuggito qualcosa non mi sparate
- Tutorial seguiti:
	- https://youtu.be/RQd44qSaqww?si=htmUgq0gSTZEjHnC
	- https://youtu.be/Vt8aZDPzRjI?si=xZ3DtYjUuUPzh6m6
	- https://www.youtube.com/watch?v=Ms10GAMAydA&t=12s&pp=ygUOdW5pdHkgY3V0c2NlbmXSBwkJjQkBhyohjO8%3D
	- https://www.youtube.com/watch?v=EMhTROG0nAw&pp=ygUPdW5pdHkgbW92ZW1lbnQg
- Tutto il resto documentazione unity/Documentazione ufficiali csharp microsoft o simili.