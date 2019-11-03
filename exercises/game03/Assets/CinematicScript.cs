using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CinematicScript : MonoBehaviour
{
    public Text text;
    public string[] phrases;
    public Color[] colors;
    public float[] waitDelays;
    public string sceneName;

    public float waitTime;
    public float letterTime;

    string phrase;
    char[] typeText;
    float delay;
    float fulldelay;
    bool done = true;
    int count = 0;
    int phrasenum = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        //setPhrase(phrases[0]);
        delay = letterTime;
        fulldelay = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
        {
            runNextScene(sceneName);
        }


        if (!done)
        {
            if (count < phrase.Length)
            {
                if (delay <= 0)
                {
                    text.text += typeText[count];
                    count++;
                    delay = letterTime;
                }
                else
                {
                    delay -= Time.deltaTime;
                }
            }
            else
            {
                fulldelay = waitTime;
                phrasenum++;
                done = true;
            }
        }
        else
        {
            if (fulldelay <= 0)
            {
                text.text = "";
                if(phrasenum < phrases.Length)
                {
                    if (waitDelays[phrasenum] <= 0)
                    {
                        setPhrase(phrases[phrasenum]);
                        text.color = colors[phrasenum];
                    }
                    else
                    {
                        waitDelays[phrasenum] -= Time.deltaTime;
                    }
                }
                else
                {
                    runNextScene(sceneName);
                }
            }
            else
            {
                fulldelay -= Time.deltaTime;
            }
        }
    }

    void setPhrase(string code)
    {
        phrase = code;
        typeText = new char[phrase.Length];
        int count2 = 0;
        foreach (char t in phrase)
        {
            typeText[count2] = t;
            count2++;
        }
        text.text = "";

        done = false;
        delay = 0.1f;
        count = 0;
    }

    void runNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
