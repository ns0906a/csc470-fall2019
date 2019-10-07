using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    float randX = 0;            // Random X value holder
    float randZ = 0;            // Random Z value holder
    int side = 0;               // Random side value holder (for where the cat spawns)
    bool gameOver = false;      // Whether the game is over

    float waitTime = 0;         // Random Time holder for how long it takes for a cat to spawn
    float textTime = 5;         // Time it takes for beginning text to disapear

    public int maxScore;        // Max score for the game (20 for 1player, 10 for 2player)
    public Text winTxt;         // Text that displays at end of game

    public GameObject cheese;   // holder for Cheese prefab
    public GameObject cat;      // holder for Cat prefab
    public GameObject rat;      // Player 2
    public GameObject mouse;    // Player 1

    // Start is called before the first frame update
    void Start()
    {
        winTxt.text = "Collect Cheese \n Watch out for Cats!";  // Starting text
        CreateCheese();                                         // Creates first cheese
        waitTime = Random.Range(5f, 10f);                       // Sets cat spawn time
        gameOver = false;                                       // Makes sure the doesn't start over over
    }

    // Update is called once per frame
    void Update()
    {
        waitTime -= Time.deltaTime;
        if (waitTime < 0) // Calls create cat once waitTime reaches below 0
        {
            createCat();
            waitTime = Random.Range(10f, 20f);
        }

        if (textTime > 0) // Decreases textTime as long as it's above 0
        {
            textTime -= Time.deltaTime;
        }

        if (textTime < 0 && textTime > -1) // Clears starting text once textTime reaches below 0
        {
            winTxt.text = "";
            textTime = -1;
        }

        checkWin(); // Check if game over
    }

    public void ReloadScene() // Reloads the current scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu() // Returns to menu scene
    {
        SceneManager.LoadScene("Menu Scene");
    }

    public void CreateCheese() // Creates a Cheese randomly on the map
    {
        randX = Random.Range(-3.5f, 3.5f);
        randZ = Random.Range(-2.5f, 2.5f);
        Instantiate(cheese, new Vector3(randX, 0f, randZ), Quaternion.identity);
    }

    void createCat() // Creates a Cat randomly on the outsides of the map
    {
        side = Random.Range(1, 4);
        if(side == 1) // Top
        {
            randX = Random.Range(-7f, 7f);
            randZ = 5.30f;
        }

        if(side == 2) // Down
        {
            randX = Random.Range(-7f, 7f);
            randZ = -5.30f;
        }

        if(side == 3) // Left
        {
            randX = -7f;
            randZ = Random.Range(-5.30f, 5.30f);
        }

        if(side == 4) // Right
        {
            randX = 7f;
            randZ = Random.Range(-5.30f, 5.30f);
        }

        Instantiate(cat, new Vector3(randX, 0f, randZ), Quaternion.identity);
    }

    void checkWin() // Checks coniditions needed to end the game and declare a winner
    {
        if (!gameOver)
        {
            if (rat != null) // If it's a 2player game
            {
                if(rat.GetComponent<PlayerController>().getPoints() >= maxScore)
                {
                    winTxt.text = "Player 2 Wins!";
                    gameOver = true;
                }
                if (mouse.GetComponent<PlayerController>().getPoints() >= maxScore)
                {
                    winTxt.text = "Player 1 Wins!";
                    gameOver = true;
                }
            }
            else // If it's a 1player game
            {
                if (mouse.GetComponent<PlayerController>().getPoints() >= maxScore)
                {
                    winTxt.text = "You Win!";
                    gameOver = true;
                }
            }
        }
    }
}
