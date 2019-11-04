using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTriggerScript : MonoBehaviour
{

    GameManager gm;
    bool end = false;
    float endTimer = 3f;

    public int totalBotCount;
    public int botCount;


    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            totalBotCount = GameObject.FindGameObjectsWithTag("Bot").Length + GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            if (end)
            {
                if (endTimer <= 0)
                {
                    
                    DontDestroyOnLoad(this.gameObject);
                    botCount = GameObject.FindGameObjectsWithTag("Bot").Length;
                    SceneManager.LoadScene("EndCinematic");
                }
                else
                {
                    endTimer-= Time.deltaTime;
                    gm.aud.volume -= 0.25f * Time.deltaTime;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            if (!end)
            {
                end = true;
                endTimer = 3f;
                gm.endGame();
            }
        }
    }
}
