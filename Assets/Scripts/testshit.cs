using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testshit : MonoBehaviour
{
    //Vector2 GetRandomPointInCircumferenceRange(Vector2 targetAlDifuori, int min, int max, int maxAngle)
    //{
    //    // La formula e' molto semplice, unity gia' ci da una funzione che prende un valore casuale
    //    // di una circonferenza, MA noi vogliamo che sia compreso in un certo RANGE, quindi facciamo
    //    // qualche calcolo in piu'.
    //    // Considerato che il punto ritornato e' normalizzato, basta fare un'addizione ed una differenza.
    //    /*Vector2 randomPointNormalized = UnityEngine.Random.insideUnitCircle;
    //    Vector2 finalVec = new Vector2(
    //        ((max - min) * randomPointNormalized.x) + Mathf.Sign(randomPointNormalized.x) * min,

    //         ((max - min) * randomPointNormalized.y) + Mathf.Sign(randomPointNormalized.y) * min
    //        );*/
    //    // Traccia una linea che va dal centro della circonferenza al target

    //    // Adesso crea i lati del settore circolare ruotandoli di meta' dell'angolo massimo

    //    // Proviamo il seguente ragionamento, dato il settore circolare, 
    //    // calcola l'angolo tra i vettori che lo compongono,
    //    // dopodiche' scegli un numero randomico compreso fra 0 e ANGOLO
    //    // a questo ruotiamo il primo vettore di N gradi. Cosi' facendo
    //    // abbiamo ottenuto i segni del punto generato
    //    // infine, generiamo un numero casuale tra 0 e 1 che sara' usato come moltiplicando
    //    // per decretare il punto randomico ottenuto

    //    // Lo normalizziamo perche' non ci interessano le coordinate, ma piu' l'orientamento rispetto al centro.
    //    Vector2 targetAlDiFuoriNormalized = targetAlDifuori.normalized;

    //    // Generiamo il "campo visivo"
    //    // Ruotiamo dei vettori di unitari con le coordinate di un piano cartesiano
    //    Vector2 sxLat = new Vector2(0, 1); // lato sinitro del settore circolare [minore]
    //    Vector2 dxLat = new Vector2(1, 0); // lato destro del settore circolare [maggiore]

    //    float angoloFraMaxETarget = Vector2.Angle(sxLat, targetAlDiFuoriNormalized);
    //    sxLat = Quaternion.AngleAxis(angoloFraMaxETarget - (maxAngle/2), Vector3.forward) * sxLat;

    //    float angoloFraMinETarget = Vector2.Angle(dxLat, targetAlDiFuoriNormalized);
    //    dxLat = Quaternion.AngleAxis(angoloFraMinETarget + (maxAngle/2), Vector3.forward) * dxLat;




    //    // Il punto randomico verra' utilizzato come una percentuale 
    //    float angoloDelSettore = Vector2.Angle(dxLat, sxLat);
    //    float angoloRandomicoInternoSettore = UnityEngine.Random.Range(0, angoloDelSettore);

    //    // Vector forward in questo caso e' il vettore uscente dal piano
    //    Vector2 vettoreDelSettoreRuotato = Quaternion.AngleAxis(angoloRandomicoInternoSettore, Vector3.forward) * new Vector2(0, 1);
    //    Debug.Log("Angolo settore: " + angoloDelSettore + " angolo random: " + angoloRandomicoInternoSettore);
    //    Debug.Log("Vettore ruotato: " + vettoreDelSettoreRuotato);

    //    Vector2 vettoreRuotatoRandomizzato = new Vector2(
    //        vettoreDelSettoreRuotato.x * UnityEngine.Random.value,/* valore tra 0.0 e 1.0*/
    //        vettoreDelSettoreRuotato.y * UnityEngine.Random.value);

    //    Debug.Log("vettore ruotato randomizzato: " + vettoreRuotatoRandomizzato);

    //    Vector2 randomPointInRange = new Vector2(
    //        (min * Mathf.Sign(vettoreRuotatoRandomizzato.x)) + ((max - min) * vettoreRuotatoRandomizzato.x),
    //        (min * Mathf.Sign(vettoreRuotatoRandomizzato.y)) + ((max - min) * vettoreRuotatoRandomizzato.y)
    //        );
    //    //Debug.Log(Vector2.Angle(minLat, sxLat));

    //    // Trovati i lati abbiamo risolto il problema e abbiamo fra le mani una disequazione semplice
    //    // Dunque, dobbiamo trovare solo i valori compresi all'interno dei due vettori
    //    //Debug.Log("RandPointNormalized: " + randomPointNormalized);
    //    Debug.Log("RandPointInRange: " + randomPointInRange);

    //    return randomPointInRange;
    //}

    // Start is called before the first frame update

    Vector2 GetRandomPointInCircumferenceRange(Vector2 targetAlDifuori, float min, float max, float maxAngle)
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
        Vector2 targetAlDiFuoriNormalized = targetAlDifuori.normalized;

        // Generiamo il "campo visivo"
        // Ruotiamo dei vettori di unitari con le coordinate di un piano cartesiano
        Vector2 sxLat = new Vector2(0, 1); // lato sinitro del settore circolare [minore]
        Vector2 dxLat = new Vector2(1, 0); // lato destro del settore circolare [maggiore]
        Debug.Log("targetNormalized: " + targetAlDiFuoriNormalized);
        float angoloFraMaxETarget = Vector2.Angle(sxLat, targetAlDiFuoriNormalized);
        sxLat = Quaternion.AngleAxis(angoloFraMaxETarget - (maxAngle / 2), Vector3.forward) * sxLat;

        float angoloFraMinETarget = Vector2.Angle(dxLat, targetAlDiFuoriNormalized);
        dxLat = Quaternion.AngleAxis(angoloFraMinETarget + (maxAngle / 2), Vector3.forward) * dxLat;



        Debug.Log("sx: " + sxLat + " dx: " + dxLat);
        // Il punto randomico verra' utilizzato come una percentuale 
        float angoloDelSettore = Vector2.Angle(dxLat, sxLat);
        float angoloRandomicoInternoSettore = UnityEngine.Random.Range(0, angoloDelSettore);

        // Vector forward in questo caso e' il vettore uscente dal piano
        Vector2 vettoreDelSettoreRuotato = Quaternion.AngleAxis(angoloRandomicoInternoSettore, Vector3.forward) * new Vector2(sxLat.x, sxLat.y);
        Debug.Log("Angolo settore: " + angoloDelSettore + " angolo random: " + angoloRandomicoInternoSettore);
        Debug.Log("Vettore ruotato: " + vettoreDelSettoreRuotato);

        Vector2 vettoreRuotatoRandomizzato = new Vector2(
            vettoreDelSettoreRuotato.x * UnityEngine.Random.value,/* valore tra 0.0 e 1.0*/
            vettoreDelSettoreRuotato.y * UnityEngine.Random.value);

        Debug.Log("vettore ruotato randomizzato: " + vettoreRuotatoRandomizzato);

        Vector2 randomPointInRange = new Vector2(
            (min * Mathf.Sign(vettoreRuotatoRandomizzato.x)) + ((max - min) * vettoreRuotatoRandomizzato.x),
            (min * Mathf.Sign(vettoreRuotatoRandomizzato.y)) + ((max - min) * vettoreRuotatoRandomizzato.y)
            );
        //Debug.Log(Vector2.Angle(minLat, sxLat));

        // Trovati i lati abbiamo risolto il problema e abbiamo fra le mani una disequazione semplice
        // Dunque, dobbiamo trovare solo i valori compresi all'interno dei due vettori
        //Debug.Log("RandPointNormalized: " + randomPointNormalized);
        Debug.Log("RandPointInRange: " + randomPointInRange);

        return randomPointInRange;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log(GetRandomPointInCircumferenceRange(new Vector2(-20,-20), 40, 100, 140));
        }
    }
}
