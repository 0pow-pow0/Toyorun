using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyer : MonoBehaviour
{
    IEnumerator kill()
    {
        yield return new WaitForSeconds(6f);
        Application.Quit();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(kill());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
