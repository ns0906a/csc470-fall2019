using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public string player = "1"; // Decides which player is controlling

    float speed = 5f;           // Speed of player movement
    float turnSpeed = 160f;     // Speed of player turning

    bool stunned = false;       // Whether the player is stunned
    float stuntime = 2;         // How long the player stays stunned

    public GameObject game;     // GameController

    AudioSource hit;            // AudioSource for hit noise
    public Image score;         // Image/UI for showing score value

    int points = 0;             // Current points
    int maxPoints;              // Max points (set by GameController)

    // Start is called before the first frame update
    void Start()
    {
        points = 0;                                                     // Makes sure points start at 0
        score.fillAmount = 0;                                           // Makes sure score UI starts cleared
        stunned = false;                                                // Makes sure the player does not start stunned
        hit = GetComponent<AudioSource>();                              // Sets hit to AudioSource component

        maxPoints = game.GetComponent<GameControllerScript>().maxScore; // Sets maxPoints to the maxPoints determined by GameController
    }

    // Update is called once per frame
    void Update()
    {
        if (stunned)            // Player spins when stunned
        {
            stuntime -= Time.deltaTime;
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            if(stuntime < 0)
            {
                stuntime = 2;
                stunned = false;
            }
        }
        else                    // Player's movements when not stunned
        {

            float xAxis = Input.GetAxis("Horizontal Player " + player);

            float yAxis = Input.GetAxis("Vertical Player " + player);

            transform.Rotate(0, xAxis * turnSpeed * Time.deltaTime, 0);

            transform.Translate(transform.forward * yAxis * speed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("boundaryUp")) // Touches northern boundary
        {
            transform.position = new Vector3(transform.position.x, 0f, -4f);
        }
        if (other.gameObject.CompareTag("boundaryDown")) // Touches southern boundary
        {
            transform.position = new Vector3(transform.position.x, 0f, 4f);
        }
        if (other.gameObject.CompareTag("boundaryLeft")) // Touches western boundary
        {
            transform.position = new Vector3(5.20f, 0f, transform.position.z);
        }
        if (other.gameObject.CompareTag("boundaryRight")) // Touches eastern boundary
        {
            transform.position = new Vector3(-5.20f, 0f, transform.position.z);
        }
        if (other.gameObject.CompareTag("cheese")) // Pickup cheese and Increase points
        {
            points++;
            Destroy(other.gameObject);
            game.GetComponent<GameControllerScript>().CreateCheese();
            score.fillAmount = points * (1f / maxPoints);
        }

        if (other.gameObject.CompareTag("cat")) // Hit cat, become stunned and loose 2 points
        {
            if (!stunned)
            {
                points -= 2;
                if(points < 0)
                {
                    points = 0;
                }
                hit.Play();
                score.fillAmount = points * (1f / maxPoints);
                stunned = true;
            }
        }
    }

    public int getPoints() // Called by GameController to get player's current points
    {
        return points;
    }

}
