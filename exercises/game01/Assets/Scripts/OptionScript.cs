using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionScript : MonoBehaviour
{

    float turnSpeed = 80f;      // Speed at which the model turns

    public string scene = "";   // Name of the scene the model loads when clicked

    void OnMouseOver()          // Model spins when moused over
    {
        transform.Rotate(0,turnSpeed * Time.deltaTime, 0);
        if(Input.GetMouseButtonUp(0)) // Load scene when mouse clicked
        {
            SceneManager.LoadScene(scene);
        }
    }

    void OnMouseExit() // Set Model back to original rotation once mouse no longer over it
    {
        transform.rotation = Quaternion.identity;
    }
}
