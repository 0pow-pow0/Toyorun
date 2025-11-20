using EntitiesCostants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilityShit;

public class Cuore : MonoBehaviour
{
    [SerializeField] Player plr;
    [SerializeField] bool isGold;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PowUtility.CheckSphere(transform.gameObject.GetComponent<SphereCollider>(), LayerMaskCostants.instance().playerBody))
        {

            if (!isGold)
            {
                if (Player.GetCurrentHp() < Player.GetMaxHp())
                {
                    plr.GainHp();
                    Destroy(transform.gameObject);
                }
            }
            else
            {
                plr.AddHp();
                Destroy(transform.gameObject);
            }
        }
    }
    

}
