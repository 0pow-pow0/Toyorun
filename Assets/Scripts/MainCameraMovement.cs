using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float CAMERA_SPEED;
    // Dove viene posizionata la telecamera alla morte
    //[SerializeField] Transform deathTransform;
    //Vector3 cameraDest = Vector3.zero;


    void Start()
    {
    }
    void Update()
    {

        transform.position =  player.position + cameraOffset;

    }
}
