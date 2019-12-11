using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuScript : MonoBehaviour
{
    public GameObject levelSelect;

    public GameObject saveGame;
    GameObject save;
    public Button[] levels;
    public GameObject[] menus;
    public Image blackScreen;

    public AudioSource aud;
    public AudioClip select;
    public AudioClip move;

    public Button[] selects;
    Button selection;
    int selectNum = 0;

    EventSystem es;

    bool tick = true;

    public int unlockedLevels = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Save") == null)
        {
            GameObject sv = Instantiate(saveGame);
            sv.name = "Save";
        }

        save = GameObject.Find("Save");
        unlockedLevels = int.Parse(save.GetComponent<Text>().text);
        DontDestroyOnLoad(save);
        StartCoroutine(fadeFromDark());
        es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        changeInteraction(menus[0]);
    }

    // Update is called once per frame
    void Update()
    {
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
                if (Input.GetAxis("NavH") == 1)
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
                if (Input.GetAxis("NavH") == -1)
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

    IEnumerator waitLoad(int seconds, string scene)
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
        for (float fade = 1; fade >= 0; fade -= 0.1f)
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
        if (AudioListener.pause)
        {
            button.GetComponentInChildren<Text>().text = "Unmute";
        }
        else
        {
            button.GetComponentInChildren<Text>().text = "Mute";
        }
        aud.PlayOneShot(select);
        deSelect();

    }

    public void setLevels()
    {
        if (unlockedLevels > 0)
        {
            toggleMenu(levelSelect);
            changeInteraction(levelSelect);
            foreach (Button level in levels)
            {
                level.gameObject.SetActive(false);
            }
            for (int i = 0; i < unlockedLevels; i++)
            {
                levels[i].gameObject.SetActive(true);
            }
            levels[4].gameObject.SetActive(true);
        } else
        {
            loadScene("level1");
        }
    }
}
