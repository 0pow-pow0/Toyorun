using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UtilityShit;

public class MainMenu : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] PlayableAsset startCutscene;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        director.stopped += OnCutsceneEnded;
    }

    bool activated = false;
    // Update is called once per frame
    void Update()
    {

        if(!activated)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
            else if(Input.anyKey)
            {
                director.Play(startCutscene);
                activated = true;
            }
        }   
    }

    void OnCutsceneEnded(PlayableDirector p)
    {
        SceneManager.LoadScene("mainScene"); 
    }
}
