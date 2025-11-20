using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Semplice contenitori di reference.
/// Non ruotare, grz.
/// </summary>
public class Barrel : MonoBehaviour
{
    /// <summary>
    /// La z dei pivot punta alla direzione verso cui spingere il barile
    /// </summary>
    [SerializeField] public GameObject[] pivots;
    [SerializeField] public Rigidbody rb;    
}
