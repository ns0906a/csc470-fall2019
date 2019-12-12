using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour
{
    GameManager gm;

    public GameObject pauseMenu;            // Game's Main Pause Menu

    public GameObject[] menus;              // List of Menus
    public Image blackScreen;               // Black screen that fades in when game loads

    public Button[] selects;                // Buttons that are current Interactable
    Button selection;                       // Button the player is currently selecting
    int selectNum = 0;                      // Number of selected Button

    public AudioSource aud;                 // Audiosource
    public AudioClip select;                // Sound effect for Selecting Button
    public AudioClip move;                  // Sound effect for Moving to Button

    bool tick = true;                       // Bool for checking selection delay

    public bool splashScreen;               // Bool for checking when on "Controller Recommended" screen

    EventSystem es;

    // Start is called before the first frame update
    void Start()
    {
        if (!splashScreen)
        {
            StartCoroutine(fadeFromDark());
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
        if (splashScreen)
        {
            StartCoroutine(waitLoad(5, "Main Menu"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Pause") == 1 && !gm.endgame && !splashScreen)            // Enables/Disables Pause Menu
        {
            if (tick)
            {
                deSelect();
                StartCoroutine(selectWait());
                togglePause();
            }
        }
        if (selection != null)
        {
            
            if (tick)                                                               //  Wait for tick before doing choice 
            {
                if (Input.GetAxis("Jump") == 1)                                     // Select button
                {
                    selection.Select();
                }
                if (Input.GetAxis("NavV") == -1)                                    // Move button up
                {
                    if (selection.FindSelectableOnUp() != null)
                    {
                        selectButton((Button)selection.FindSelectableOnUp());
                    }
                    else
                    {
                        selectButton(selects[selects.Length - 1]);
                    }
                }
                if (Input.GetAxis("NavV") == 1)                                    // Move button down
                {
                    if (selection.FindSelectableOnDown() != null)
                    {
                        selectButton((Button)selection.FindSelectableOnDown());
                    }
                    else
                    {
                        selectButton(selects[0]);
                    }
                }
                if (Input.GetAxis("NavH") == -1)                                    // Move button left
                {
                    if (selection.FindSelectableOnLeft() != null)
                    {
                        selectButton((Button)selection.FindSelectableOnLeft());
                    }
                    else
                    {
                        selectButton(selects[selects.Length - 1]);
                    }
                }
                if (Input.GetAxis("NavH") == 1)                                   // Move button right
                {
                    if (selection.FindSelectableOnRight() != null)
                    {
                        selectButton((Button)selection.FindSelectableOnRight());
                    }
                    else
                    {
                        selectButton(selects[0]);
                    }
                }
            }

        }
        else
        {
            if (Input.GetAxis("NavV") != 0 || Input.GetAxis("NavH") != 0)          // Sets selected button to first button
            {
                selectButton(selects[0]);
            }
        }
    }

    public void togglePause()                                      // Disable/Enables the Pause Menu
    {
        gm.paused = !gm.paused;
        if(gm.paused)
        {
            changeInteraction(pauseMenu);
            pauseMenu.SetActive(true);
        }
        else
        {
            foreach (GameObject men in menus)
            {
                men.SetActive(false);
            }
        }
    }

    IEnumerator waitLoad(int seconds,string scene)                 // Loads scene of inputed seconds
    {
        yield return new WaitForSeconds(seconds);
        loadScene(scene);
    }

    IEnumerator selectWait()                                        // Wait before player can select again
    {
        tick = false;
        yield return new WaitForSeconds(0.2f);
        tick = true;
    }

    public void selectButton(Button but)                            // Sets button to selected Button
    {
        if (selection != null)
        {
            deSelect();
        }
        if (but != null)
        {
            selection = but;
            selection.GetComponent<Image>().color = Color.grey;
            aud.PlayOneShot(move);
        }
        StartCoroutine(selectWait());
    }

    public void deSelect()                                          // Deselects currently selected Button
    {
        if (selection != null)
        {
            selection.GetComponent<Image>().color = Color.white;
            es.SetSelectedGameObject(null);
            selection = null;

        }
    }    

    public void loadScene(string scene)                             // Loads after fading to black input scene
    {
        StartCoroutine(fadeToDark(scene));
        aud.PlayOneShot(select);
    }

    IEnumerator fadeToDark(string scene)                            // Fades screen to black
    {
        for (float fade = 0; fade <= 1.1; fade += 0.01f)
        {
            Color col = blackScreen.color;
            col.a = fade;
            blackScreen.color = col;
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }

    IEnumerator fadeFromDark()                                      // Fades screen from black
    {
        for (float fade = 1; fade >= 0; fade -= 0.01f)
        {
            Color col = blackScreen.color;
            col.a = fade;
            blackScreen.color = col;
            yield return null;
        }
    }

    public Button[] setInteractable(GameObject menu, bool value)    // Makes set of buttons interactable
    {
        List<Button> butts = new List<Button>();
        List<Button[]> buttonLists = new List<Button[]>();
        foreach (Transform child in menu.transform)
        {
            if (child.GetComponent<VerticalLayoutGroup>() != null)
            {
                buttonLists.Add(setInteractable(child.gameObject, value));
            }
            if (child.GetComponent<HorizontalLayoutGroup>() != null)
            {
                buttonLists.Add(setInteractable(child.gameObject, value));
            }
            if (child.GetComponent<Button>() != null)
            {
                butts.Add(child.GetComponent<Button>());
                child.GetComponent<Button>().interactable = value;
            }
        }
        int length = butts.Count;
        foreach (Button[] list in buttonLists)
        {
            length += list.Length;
        }

        Button[] addSelect = new Button[length];

        int count = 0;
        foreach (Button but in butts)
        {
            addSelect[count] = but;
            count++;
        }

        foreach (Button[] list in buttonLists)
        {
            foreach (Button but in list)
            {
                addSelect[count] = but;
                count++;
            }
        }
        return addSelect;
    }

    public void toggleMenu(GameObject menu)                     // Enables/Disables Menu 
    {
        menu.SetActive(!menu.activeSelf);
        aud.PlayOneShot(select);
    }

    public void changeInteraction(GameObject menu)              // Enables menu buttons and Disables all others 
    {
        foreach (GameObject men in menus)
        {
            setInteractable(men, false);
        }
        selects = setInteractable(menu, true);
        selectNum = 0;
        
    }

    public void togglemute(GameObject button)                   // Enables/Disables game sounds
    {
        AudioListener.pause = !AudioListener.pause;
        if(AudioListener.pause)
        {
            button.GetComponentInChildren<Text>().text = "Unmute";
        }
        else
        {
            button.GetComponentInChildren<Text>().text = "Mute";
        }
        deSelect();
    }
}
