using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CopySpawner : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] Transform startPos;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 newPos = startPos.position;
        for (int i = 0; i < 10; i++)
        {
            newPos += new Vector3(0, 0, 10);
            GameObject o = Instantiate(obj, newPos, startPos.localRotation);
            o.transform.localScale = obj.transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
