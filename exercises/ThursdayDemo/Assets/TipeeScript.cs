using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TipeeScript : MonoBehaviour
{

    public Text winText;

    // Start is called before the first frame update
    void Start()
    {
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("TipMiniGame");
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EndZone1"))
        {
            winText.text = "Player 1 Wins!\n Press 'R' to Restart";
        }

        if (other.CompareTag("EndZone2"))
        {
            winText.text = "Player 2 Wins!\n Press 'R' to Restart";
        }
    }
}
