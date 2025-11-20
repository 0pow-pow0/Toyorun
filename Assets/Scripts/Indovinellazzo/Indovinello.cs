using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// L'indovinello e' un classico indovinello ad interruttore
/// Ogni pressureplate attiva o disattiva degli interruttori
/// Per vincere dobbiamo aprire il passaggio facendo abbassare il cancello (spegnere tutti gli interruttori).
/// Utilizziamo la logica dello XOR nell'applicare le "key" delle pressure plates
/// </summary>
public class Indovinello : MonoBehaviour
{
    [SerializeField] PressurePlate[] pressurePlates;
    // YOOO Bitmask, si procedera' in binario
    //[SerializeField] public int combinationLenghtMask; 
    [SerializeField] GameObject colliderSoluzione;
    [SerializeField] GameObject[] cancelli;
    
    // La password che dobbiamo trovare
    [SerializeField] public int password;
    // Il risultato dovuto alla pressione delle pressure plates.
    private int insertedPassword;

    // Appena e' true, l'indovinello non potra' piu' essere modificato
    // Disattiveremo gli animator, cosi' le pressure plates rimarrano
    // nello stato della combinazione corretta e non potranno essere cambiate.
    // Disabilitiamo anche lo script per evitare dispiacevoli inconvenienti xd.
    bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Controlla se tutte le pressure plates sono premute,
    // se sono premute attiva/disattiva gli interruttori corrispondenti.
    void CheckAndApplyKeys()
    {
        for (int i = 0; i < pressurePlates.Length; i++)
        {
            if (pressurePlates[i].isPressed)
            {
                insertedPassword = insertedPassword ^ pressurePlates[i].key;
            }
        }
        int pow = 2;
        // Abbassa / Alza cancelli
        for (int i = 0; i < cancelli.Length; i++)
        {
            // Se il bit e' attivo alza
            if(i == 0) { pow = 1; }
            else { pow = 2 * i; }
            if((insertedPassword & pow) != 0)
            {                
                cancelli[i].GetComponent<Animator>().SetBool("CancelloSu", false);
                cancelli[i].GetComponent<Animator>().SetBool("CancelloGiu", true);
            }
            else
            {
                cancelli[i].GetComponent<Animator>().SetBool("CancelloGiu", false);
                cancelli[i].GetComponent<Animator>().SetBool("CancelloSu", true);
            }
        }   
    }

    

    IEnumerator TurnOffAnimatorsAndScripts()
    {
        yield return new WaitForSeconds(2f);
        for(int i = 0;i < pressurePlates.Length;i++)
        {
            pressurePlates[i].anim.enabled = false;
            pressurePlates[i].enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (completed) { return; }
 
        // Resettiamo cosi' prendiamo solo le chiavi attive
        insertedPassword = 0;
        CheckAndApplyKeys();


        if(insertedPassword == password)
        {
            completed = true;
            colliderSoluzione.SetActive(false);
            StartCoroutine(TurnOffAnimatorsAndScripts());
            Debug.Log("COMPLETED");
        }
    }
}
