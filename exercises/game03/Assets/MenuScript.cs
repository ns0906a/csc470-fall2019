using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public AudioSource au;
    public Image flicker;
    public GameObject credits;
    public Text title;

    public AudioClip buttonHover;
    public AudioClip buttonSelect;

    bool flick = false;

    // Start is called before the first frame update
    void Start()
    {
        title.text = "//" + "\n";
        foreach (char t in "Null Refference Error ")
        {
            if(t == ' ')
            {
                title.text += "\n";
                Debug.Log("Yo");
            }
            else
            {
                title.text += "<color=" + "#" + Random.Range(0, 16777215).ToString("x") + ">" + t + "</color>";
            }
        }
        title.text += "//";
    }

    // Update is called once per frame
    void Update()
    {
        if (!flicker.enabled)
        {
            if (!flick)
            {
                flickerPlay();
                flick = true;
            }
        }
        else
        {
            if (flick)
            {
                flickerPlay();
                flick = false;
            }
        }
    }

    public void flickerPlay()
    {
        au.Play();
    }

    public void showCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //public void 
}
