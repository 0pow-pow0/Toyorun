using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;
using TMPro;

public class Cartello : MonoBehaviour
{
    [SerializeField] CartelPopupPanel refScr;

    // Il testo scritto sul cartello
    [SerializeField] private string content;
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        refScr.Activate(content);
    }

    void OnTriggerExit(Collider other)
    {
        refScr.QueueDeactivate();
    }
}
