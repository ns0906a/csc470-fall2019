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

    EndTriggerScript endTrigger;
    public GameObject[] botPhases;
    

    // Start is called before the first frame update
    void Start()
    {
        //setPhrase(phrases[0]);
        delay = letterTime;
        fulldelay = waitTime;
        if (SceneManager.GetActiveScene().name.Equals("EndCinematic"))
        {
            int average = 0;
            int thres = 0;
            if (GameObject.Find("EndTrigger"))
            {
                endTrigger = GameObject.Find("EndTrigger").GetComponent<EndTriggerScript>();
                average = endTrigger.totalBotCount / 4;
                thres = endTrigger.botCount;
            }
            else
            {
                average = 17 / 4;
                thres = 17;
            }
            
            Debug.Log(average + "," + average * 2 + "," + average * 3 + "," + average * 4);
            for (int i = 0;i<=botPhases.Length;i++)
            {
                if(thres > average * i+1)
                {
                    botPhases[i].SetActive(true);
                }
            }
        }
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
