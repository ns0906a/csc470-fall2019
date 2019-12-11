using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuScript : MonoBehaviour
{
    GameManager gm;

    public GameObject pauseMenu;

    //public 
    //public Button[] menuButtons;
    public GameObject[] menus;
    public Image blackScreen;

    public Button[] selects;
    Button selection;
    int selectNum = 0;

    public AudioSource aud;
    public AudioClip select;
    public AudioClip move;

    bool tick = true;
    bool pause = true;

    public bool splashScreen;

    EventSystem es;

    // Start is called before the first frame update
    void Start()
    {
        if (!splashScreen)
        {
            StartCoroutine(fadeFromDark());
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            //selects = menuButtons;
        }
        if (splashScreen)
        {
            StartCoroutine(waitLoad(5, "Main Menu"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Pause") == 1 && !gm.endgame && !splashScreen)
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
            if (Input.GetAxis("Jump") == 1)
            {
                selection.Select();
            }
            if (tick)
            {
                if (Input.GetAxis("NavV") == -1)
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
                if (Input.GetAxis("NavV") == 1)
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
                if (Input.GetAxis("NavH") == -1)
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
                if (Input.GetAxis("NavH") == 1)
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
            if (Input.GetAxis("NavV") != 0 || Input.GetAxis("NavH") != 0)
            {
                selectButton(selects[0]);
            }
        }
    }

    public void togglePause()
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

    IEnumerator waitLoad(int seconds,string scene)
    {
        yield return new WaitForSeconds(seconds);
        loadScene(scene);
    }

    IEnumerator selectWait()
    {
        tick = false;
        yield return new WaitForSeconds(0.2f);
        tick = true;
    }

    public void selectButton(Button but)
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

    public void deSelect()
    {
        if (selection != null)
        {
            selection.GetComponent<Image>().color = Color.white;
            es.SetSelectedGameObject(null);
            selection = null;

        }
    }    

    public void loadScene(string scene)
    {
        StartCoroutine(fadeToDark(scene));
        aud.PlayOneShot(select);
    }

    IEnumerator fadeToDark(string scene)
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

    IEnumerator fadeFromDark()
    {
        for (float fade = 1; fade >= 0; fade -= 0.01f)
        {
            Color col = blackScreen.color;
            col.a = fade;
            blackScreen.color = col;
            yield return null;
        }
    }

    public Button[] setInteractable(GameObject menu, bool value)
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

    public void toggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        aud.PlayOneShot(select);
    }

    public void changeInteraction(GameObject menu)
    {
        foreach (GameObject men in menus)
        {
            setInteractable(men, false);
        }
        selects = setInteractable(menu, true);
        selectNum = 0;
        
    }

    public void togglemute(GameObject button)
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
        aud.PlayOneShot(select);
    }
}
