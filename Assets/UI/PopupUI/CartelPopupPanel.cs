using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Il gameobject non si disattivera', ma i suoi figlioletti si'
/// </summary>
public class CartelPopupPanel : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject imageObj;
    [SerializeField] private GameObject textObj;


    public void Activate(string newText)
    {
        textObj.SetActive(true);
        textObj.GetComponent<TextMeshProUGUI>().text = newText;
        imageObj.SetActive(true);
        
        anim.SetTrigger("Activate");
        
    }

    /// <summary>
    /// Attiva l'animazione che al suo termine chiamera' Deactivate()
    /// </summary>
    public void QueueDeactivate()
    {
        anim.SetTrigger("Deactivate");
    }
    
    /// <summary>
    /// Da non chiamare direttamente.
    /// 
    /// Chiamata da un animation event.
    /// </summary>
    public void Deactivate()
    {
        textObj.SetActive(false);
        textObj.GetComponent<TextMeshProUGUI>().text = null;
        imageObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
