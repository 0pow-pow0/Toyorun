using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ContinuaButton()
    {
        Debug.Log("Mhanz");
        SceneManager.LoadScene("mainScene");
    }

    public void EsciButton()
    {
        Debug.Log("Mhanz");
        Application.Quit();
    }
}
