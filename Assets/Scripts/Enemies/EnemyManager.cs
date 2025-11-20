using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
  

/// <summary>
/// E' un singleton singletonia
/// Gestice i nemici all'interno di ogni sezione di livello, dunque puo' gestire
/// solo una sezione alla volta.
/// </summary> 
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager mng; // Singleton
    //[SerializeField] GameObject enemyPrefab;
    public static EnemyManager instance()
    {
        if(mng == null)
        {
            mng = new EnemyManager();   
        }
        return mng;
    }
     // Lo script della sezione del livello attiva al momento (sara' sempre e solo una)
    [NonSerialized] private LevelSection activeLevelSection;

    // Praticamente utilizzo delle circonferenze come range intorno al giocatore per
    // selezionare il punto verso cui il nemico dovra' andare
    #region COSTANTI
    // COSTANTI CALCOLO DELTA
    public const int MAX_STOP_RADIUS_PATROLLING = 40;
    public const int MIN_STOP_RADIUS_PATROLLING = 20;
    public const int MAX_STOP_RADIUS_ATTACK = 40;
    public const int MIN_STOP_RADIUS_ATTACK = 20;
    public const int MAX_VIEW_ANGLE = 140;

    // La distanza minima che ci deve essere fra il delta di un nemico ed un altro 
    public const int MIN_DISTANCE_DELTAS = 20;


    // COSTANTI GENERICHE
    [NonSerialized] public const int MAX_DISTANCE_FROM_DELTA_WALKINGTO_STATE = 15;

    #endregion
    // Meglio avere un getter cosi' sono piu' conscio delle modifiche
    public void ChangeActiveLevelSection(LevelSection lvlSec)
    {
        activeLevelSection = lvlSec;
    }
    public EnemyManager() { }

    private void Awake()
    {
    }

    /// <summary>
    /// Letteralmente quanto dice, immaginando una circonferenza al centro di un piano cartesiano
    /// si prende un punto completamente a cazzo di questa circonferenza, che sia negativo o positivo, 
    /// all'interno di range determinati, il range in questione e' un'altra circonferenza ed una sezione
    /// circolare della data circonferenza.
    /// Guardare disegno per capire meglio. [l'ho fatto su carta xd]
    /// Funziona un po' malino, dovrei fixarlo ma ripperoni
    /// </summary>
    /// <param name="min">Inclusivo</param>
    /// <param name="max">Non inclusivo</param>
    Vector2 GetRandomPointInCircumferenceRange(Vector2 targetAlDiFuori, float min, float max)
    {
        // La formula e' molto semplice, unity gia' ci da una funzione che prende un valore casuale
        // di una circonferenza, MA noi vogliamo che sia compreso in un certo RANGE, quindi facciamo
        // qualche calcolo in piu'.
        // Considerato che il punto ritornato e' normalizzato, basta fare un'addizione ed una differenza.
        /*Vector2 randomPointNormalized = UnityEngine.Random.insideUnitCircle;
        Vector2 finalVec = new Vector2(
            ((max - min) * randomPointNormalized.x) + Mathf.Sign(randomPointNormalized.x) * min,

             ((max - min) * randomPointNormalized.y) + Mathf.Sign(randomPointNormalized.y) * min
            );*/
        // Traccia una linea che va dal centro della circonferenza al target

        // Adesso crea i lati del settore circolare ruotandoli di meta' dell'angolo massimo

        // Proviamo il seguente ragionamento, dato il settore circolare, 
        // calcola l'angolo tra i vettori che lo compongono,
        // dopodiche' scegli un numero randomico compreso fra 0 e ANGOLO
        // a questo ruotiamo il primo vettore di N gradi. Cosi' facendo
        // abbiamo ottenuto i segni del punto generato
        // infine, generiamo un numero casuale tra 0 e 1 che sara' usato come moltiplicando
        // per decretare il punto randomico ottenuto

        // Lo normalizziamo perche' non ci interessano le coordinate, ma piu' l'orientamento rispetto al centro.
        Vector2 targetAlDiFuoriNormalized = targetAlDiFuori.normalized;

        // normalizzarlo da valori da 0 a 1, il che non ci basta per sapere l'orientamento 
        // quindi moltiplichiamo per i segni che ci danno l'orientamento rispetto ad un punto al
        // centro del piano cartesiano
        targetAlDiFuoriNormalized = new Vector2(
            targetAlDiFuoriNormalized.x * Mathf.Sign(targetAlDiFuori.x),
            targetAlDiFuoriNormalized.y * MathF.Sign(targetAlDiFuori.y));

        // Generiamo il "campo visivo"
        // Ruotiamo dei vettori di unitari con le coordinate di un piano cartesiano
        Vector2 sxLat = new Vector2(0, 1); // lato sinitro del settore circolare [minore]
        Vector2 dxLat = new Vector2(1, 0); // lato destro del settore circolare [maggiore]

        float angoloFraMaxETarget = Vector2.Angle(sxLat, targetAlDiFuoriNormalized);
        sxLat = Quaternion.AngleAxis(angoloFraMaxETarget - (MAX_VIEW_ANGLE / 2), Vector3.forward) * sxLat;

        float angoloFraMinETarget = Vector2.Angle(dxLat, targetAlDiFuoriNormalized);
        dxLat = Quaternion.AngleAxis(angoloFraMinETarget + (MAX_VIEW_ANGLE / 2), Vector3.forward) * dxLat;

        //Debug.Log("sx: " + sxLat + " dx: " + dxLat);


        // Il punto randomico verra' utilizzato come una percentuale 
        float angoloDelSettore = Vector2.Angle(dxLat, sxLat);
        float angoloRandomicoInternoSettore = UnityEngine.Random.Range(0, angoloDelSettore);

        // Vector forward in questo caso e' il vettore uscente dal piano
        Vector2 vettoreDelSettoreRuotato = Quaternion.AngleAxis(angoloRandomicoInternoSettore, Vector3.forward) * new Vector2(sxLat.x, sxLat.y);
       // Debug.Log("Angolo settore: " + angoloDelSettore + " angolo random: " + angoloRandomicoInternoSettore);
        //Debug.Log("Vettore ruotato: " + vettoreDelSettoreRuotato);

        Vector2 vettoreRuotatoRandomizzato = new Vector2(
            vettoreDelSettoreRuotato.x * UnityEngine.Random.value,/* valore tra 0.0 e 1.0*/
            vettoreDelSettoreRuotato.y * UnityEngine.Random.value);

        //Debug.Log("vettore ruotato randomizzato: " + vettoreRuotatoRandomizzato);

        Vector2 randomPointInRange = new Vector2(
            (min * Mathf.Sign(vettoreRuotatoRandomizzato.x)) + ((max - min) * vettoreRuotatoRandomizzato.x),
            (min * Mathf.Sign(vettoreRuotatoRandomizzato.y)) + ((max - min) * vettoreRuotatoRandomizzato.y)
            );
        //Debug.Log(Vector2.Angle(minLat, sxLat));

        // Trovati i lati abbiamo risolto il problema e abbiamo fra le mani una disequazione semplice
        // Dunque, dobbiamo trovare solo i valori compresi all'interno dei due vettori
        //Debug.Log("RandPointNormalized: " + randomPointNormalized);
       // Debug.Log("RandPointInRange: " + randomPointInRange);

        return randomPointInRange;
    }


    /// <summary>
    /// Quello che fa e' trovare il delta rispettando i criteri di distanza che ci devono essere.
    /// Mi raccomando zio peride, resetta il delta prima di chiamare questa fottutissima funzione :).
    /// </summary>
    public Vector2 RequestEnemyDelta(Transform enemPos, Vector2 enemDelta)
    {
        // Esegui un massimo di 4 volte, se alla terza volta
        // non riesce comunque a trovare un punto disponibile, fottitene della distanza
        
        bool skipCheck = false;
        for (int i = 0; i < 4; i++) 
        {        
                if(i >= 3) { skipCheck = true; }
    
                // Randomizza il punto
                // Non sara' x e y ma x e z
                Vector2 newDelta = GetRandomPointInCircumferenceRange(
                new Vector2(enemPos.position.x, enemPos.position.z),
                MIN_STOP_RADIUS_PATROLLING, 
                MAX_STOP_RADIUS_PATROLLING);
                bool distantFromEveryone = true;
                for(int a = 0; a < activeLevelSection.simpleEnemies.Length; a++)
                {
                    SimpleEnemy enemScr = activeLevelSection.simpleEnemies[a].GetComponent<SimpleEnemy>();                
                    // se e' uguale significa che e' il delta del chiamante xd [tutti i delta sono diversi l'uno dall'altro]
                    if(enemDelta == enemScr.delta &&
                        enemDelta != Vector2.zero) // Altrimenti non setta una ciola quando non sono mai stati settati 
                    {
                        continue;
                    }

                    // Controlla se sia abbastanza distante da ogni punto
                    if (skipCheck || !(Mathf.Abs(Vector2.Distance(enemScr.delta, newDelta)) >= MIN_DISTANCE_DELTAS))
                    {
                        distantFromEveryone = false;
                    }
                }
                if(skipCheck) { Debug.LogWarning("SkipCheck True!"); }
                if (distantFromEveryone || skipCheck) { return newDelta; }

            }
        Debug.LogError("Delta non trovato!");
        return new Vector2(0, 0);
    }
    

}
