using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject HeartPrefab;
    [SerializeField] private int heartsDistanceX; // Distanza fra i cuori nell'hud
    [SerializeField] private List<GameObject> heartsArray;
    //int indexOfRightMostHeart = 1; // Indice del cuore piu' a destra della barra PIENOs
    // Non si puo' instanziare
    private HealthBar() { }

    public void AddHeart()
    {
        GameObject newHeart = Instantiate(HeartPrefab, transform);
        RectTransform newHeartRectTransform = newHeart.GetComponent<RectTransform>();
        RectTransform nearHeart = heartsArray[heartsArray.Count-1].GetComponent<RectTransform>();
        // Prende la X dell'ultimo cuore inserito per traslarlo correttamente
        //Debug.Log(nearHeart.localPosition.x);
        newHeartRectTransform.localPosition = new Vector3(
            nearHeart.localPosition.x + heartsDistanceX,
            newHeartRectTransform.localPosition.y,
            newHeartRectTransform.localPosition.z);

        newHeart.GetComponent<Animator>().SetTrigger("AddHeartTrigger");
        heartsArray.Add(newHeart);
    }

    /// <summary>
    /// Rimuove un cuore dalla barra degli hp.
    /// In pratica toglie l'ultimo cuore dalla lista heartsArray.
    /// UN PO' UNSAFE LO SO.
    /// </summary>
    /// <param name="indexToDestroy">In questo modo posso distruggere piu' cuori contemporaneamente</param>
    public IEnumerator RemoveHeart(int indexToDestroy)
    {
        heartsArray[indexToDestroy].GetComponent<Animator>().SetTrigger("RemoveHeartTrigger");
        yield return new WaitForSeconds(4.5f);
        Destroy(heartsArray[indexToDestroy]);
        heartsArray.RemoveAt(indexToDestroy);
    }

    public void LoseHeart(int indexHeart)
    {
        if(indexHeart >= heartsArray.Count || indexHeart < 0) { Debug.LogWarning("Out of bounds"); return; }

        heartsArray[indexHeart].GetComponent<Animator>().SetTrigger("LoseHeartTrigger");
    }

    public void GainHeart(int indexHeart)
    {
        if (indexHeart >= heartsArray.Count || indexHeart < 0) { Debug.LogWarning("Out of bounds"); return; }
        heartsArray[indexHeart].GetComponent<Animator>().SetTrigger("GainHeartTrigger");
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
