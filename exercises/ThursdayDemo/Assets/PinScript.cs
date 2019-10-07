using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PinScript : MonoBehaviour
{

    static int p1points;
    static int p2points;
    public int goal = 2;
    public string scene = "KnockDownGame";

    public Text winText;

    // Start is called before the first frame update
    void Start()
    {
        winText.text = "";
        p2points = 0;
        p1points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(scene);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EndZone1"))
        {
            p2points = p2points + 1;
            Debug.Log("EndZone1");
        }

        if(other.CompareTag("EndZone2"))
        {
            p1points = p1points + 1;
            Debug.Log("EndZone2");
        }

        checkWin();
    }

    void checkWin()
    {
        Debug.Log("P1: " + p1points.ToString() + ". P2: " + p2points.ToString());
        if(p1points >= goal)
        {
            winText.text = "Player 1 Wins!\n Press 'R' to Restart";
            Debug.Log("Point1");
        }

        if(p2points >= goal)
        {
            winText.text = "Player 2 Wins!\n Press 'R' to Restart";
            Debug.Log("Point2");
        }
    }
}
