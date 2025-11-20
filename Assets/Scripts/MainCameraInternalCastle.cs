using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraInternalCastle : MonoBehaviour
{
    [SerializeField] Transform player;
    /*[SerializeField]*/ public Vector3 cameraOffset;
    [SerializeField] float CAMERA_SPEED;
    Vector3 cameraDest = Vector3.zero;


    // Utlizzero solo una delle assi di ognuno di questi gameObject
    // al fine di limitare il movimento della telecamera
    public GameObject costraintMinX;
    public GameObject costraintMaxX;
    public GameObject costraintMinZ;
    public GameObject costraintMaxZ;


    void Awake()
    {
        //cameraOffset = transform.position;
    }


    void Update()
    {
        // L'asse X la locko
        transform.position = new Vector3(
            cameraOffset.x,
            cameraOffset.y,
            cameraOffset.z + player.position.z);

        // ---------- Asse X
        if(transform.position.x < costraintMinX.transform.position.x)
        {
            transform.position = new Vector3(
                costraintMinX.transform.position.x,
                transform.position.y,
                transform.position.z);
        }

        if (transform.position.x > costraintMaxX.transform.position.x)
        {
            transform.position = new Vector3(
                costraintMaxX.transform.position.x,
                transform.position.y,
                transform.position.z);
        }

        // ------- ASSE Z
        if (transform.position.z < costraintMinZ.transform.position.z)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                costraintMinZ.transform.position.z);
        }
        if (transform.position.z > costraintMaxZ.transform.position.z)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                costraintMaxZ.transform.position.z);
        }
        //transform.position += Vector3.Lerp(transform.position, player.position, Time.deltaTime);
        //transform.position = new Vector3(transform.position.x, cameraOffset.y, transform.position.z);
    }
}
