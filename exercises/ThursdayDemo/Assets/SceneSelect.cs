using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("TipMiniGame");
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("KnockDownGame");
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("KnockDownGameVersus");
        }
    }
}
