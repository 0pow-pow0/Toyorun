using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] public Animator anim;
    [SerializeField] BoxCollider activationCollider;
    // La chiave logica che attiva/disattiva gli interruttori.
    [SerializeField] public int key;
    public bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Se collide
        if(PowUtility.CheckBox(activationCollider, LayerMaskCostants.instance().barrelsCollider))
        {
            isPressed = true;
            anim.SetBool("isPressed", true);
        }
        // Se non collide
        else
        {
            isPressed = false;
            anim.SetBool("isPressed", false);
        }
    }
}
