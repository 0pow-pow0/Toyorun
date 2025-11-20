using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class StyleBar : MonoBehaviour
{
    // Numero di colpi dati in successione dal player.
    [NonSerialized] public int hits;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] Slider slider;


    // Non si puo' instanziare
    private StyleBar() { }

    public void AddHit()
    {
        hits++;

        //if(hits == 10)
        //if(hits == 20)
        //ecc....
        // TODO: Check del numero con rispettivo aggiustamento
    }
    void Start()
    {
        
    }


}
