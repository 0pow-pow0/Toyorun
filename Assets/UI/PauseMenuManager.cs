using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UtilityShit;

/// <summary>
/// Il bookBackground e' padre di tutti, questo perche' in questo modo il suo animator
/// puo' disattivare tutti gli oggetti temporaneamnente per poter fare le sue animazioni
/// senza sovrapporsi al testo 
/// 
/// - I Panels sono in pratica le categorie del menu'
/// [Menu iniziale di pausa, Menu del tutorial di pausa]
/// 
/// - Le pageViews sarebbero i contenuti di ogni categoria nel caso in cui fosse possibile sfogliare
/// [Il Menu del tutorial di pausa avra': prima pagina, seconda pagina, ecc....]
/// </summary>
public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject bookBackground;
    [SerializeField] Animator bookAnim;
    // Per evitare che compaiano le scritte durante l'animazione di flip
    // non sapevo come fare in altro modo
    [SerializeField] float flipAnimationDuration;
    [SerializeField] GameObject mainPanel;

    // ----- Tutorial shit
    [SerializeField] GameObject tutorialPanel;
    // - Views ---- Sarebbero i wrapper delle pagine che compaiono che si possono sfogliare
    [SerializeField] GameObject[] tutorialPageViews;
    // Sono un perfezionista figlio di buttana
    [SerializeField] GameObject[] tutorialPageButtons;

    int pageIndex;

    private void Awake()
    {
        SetAllPanels(false);
    }

    void SetAllPanels(bool b)
    {
        bookBackground.SetActive(b);
        mainPanel.SetActive(b);
        tutorialPanel.SetActive(b);
        PowUtility.SetActiveObjs(tutorialPageViews, b);
    }

    void BeginPause()
    {
        GameManager.isGamePaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        bookBackground.SetActive(true);
        mainPanel.SetActive(true);
    }
    
    void EndPause()
    {
        GameManager.isGamePaused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        SetAllPanels(false);
    }

    #region MAIN PANEL BUTTONS ACTIONS
    public void MainPanelButtonRiprendi()
    {
        EndPause();
    }

    public void MainPanelButtonTutorial()
    {
        // Setta alla prima pagina
        pageIndex = 0;
        Debug.Log("BRUH");
        mainPanel.SetActive(false);
        // Attiva la prima pagina
        bookAnim.SetTrigger("FlipPage");
        
        // Aspetta la fine del flip dell'animazione
        PowUtility.DelayInstruction(this, () => {
            tutorialPanel.SetActive(true);
            PowUtility.SetActiveObjs(tutorialPageButtons, true);
            tutorialPageViews[pageIndex].SetActive(true); },
        flipAnimationDuration);
    }

    public void MainPanelButtonEsci()
    {
        EndPause();
        SceneManager.LoadScene("MainMenu");
    }
    #endregion

    #region TUTORIAL PANEL
    // ------------------ Logic


    // ------------------ Bottoni



    // Esci dalla schermata del tutorial
    public void TutorialPanelButtonQuit()
    {
        // Disattiva tutte le page views
        PowUtility.SetActiveObjs(tutorialPageViews, false);
        
        
        tutorialPanel.SetActive(false);
        // Ritorna alla schermata principale del menu
        PowUtility.DelayInstruction(this, () =>
        {

            mainPanel.SetActive(true);
        }, 
        flipAnimationDuration);

        bookAnim.SetTrigger("FlipPage");
    }

    public void TutorialPanelButtonNextPage()
    {
        // Disattiva la pagina aperta
        tutorialPageViews[pageIndex].SetActive(false);

        pageIndex++;

        // Ritorna all'inizio se sei all'ultima pagina
        if (pageIndex > tutorialPageViews.Length - 1)
        {
            pageIndex = 0;
        }

        // Per evitare che si triggheri prima dell'inizio dell'animazione del libro
        // non so perche' ma parte un po' in ritardo l'animazione del libro.

        PowUtility.SetActiveObjs(tutorialPageButtons, false);
        PowUtility.DelayInstruction(this,
            () => 
            { 
                tutorialPageViews[pageIndex].SetActive(true);
                PowUtility.SetActiveObjs(tutorialPageButtons, true);
            }, 
            flipAnimationDuration);

        bookAnim.SetTrigger("FlipPage");
    }

    public void TutorialPanelButtonPreaviousPage()
    {
        // Disattiva la pagina aperta
        tutorialPageViews[pageIndex].SetActive(false);

        pageIndex--;

        // Ritorna alla fine se sei all'inizio
        if (pageIndex < 0)
        {
            pageIndex = tutorialPageViews.Length - 1;
        }

        PowUtility.SetActiveObjs(tutorialPageButtons, false);
        PowUtility.DelayInstruction(this,
            () => 
            {
                tutorialPageViews[pageIndex].SetActive(true);
                PowUtility.SetActiveObjs(tutorialPageButtons, true);
            },
            flipAnimationDuration);

        bookAnim.SetTrigger("FlipPage");
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!GameManager.isGamePaused)
            {
                BeginPause();
            }
            else
            {
                EndPause();
            }
        }
    }


}
