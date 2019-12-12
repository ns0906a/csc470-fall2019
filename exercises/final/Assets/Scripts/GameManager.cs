using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject camTarg;                      // Object for Camera to Follow
    public GameObject endMenu;                      // Menu for when Level complete
    public GameObject intro;                        // Text that appears at start of Level
    public GameObject arrow;                        // Prefab for Arrow that show up for Coin Search
    public PlayerScript selectedPlayer;             // Currently Selected Player
    public PlayerScript[] players;                  // List of Players in Level
    public List<PlayerScript> activePlayers;        // List of Players the Player can select and Controll

    List<PlayerScript> endPlayers;                  // List of Players in EndGoal (used to determine Level Complete)

    public MenuScript menu;                         // Menu Script          

    public GameObject door;                         // Level Exit Door

    public GameObject save;                         // Object that holds info on unlocked levels

    public Text coinText;                           // Text displaying how many Coins are left for the Player to collect

    public int coinsCollected = 0;                  // Total amount of coins collected

    public int selectNum = 0;                       // Number of Selected Cubi (character the player I control)
    bool cameraPan = true;                          // Bool for when Camera is panning from Target to Target

    float camDistance = -15;                        // Camera's Distance to player (Camera Controls)
    float camSpeed = 15;                            // Camera's movement speed (Camera controls)

    public bool paused;                             // Bool for when game is paused
    public bool endgame;                            // Bool for when Level is completed

    GameObject target;                              // Target for Camera to Follow/Pan to

    public int currentLevel;                        // Number for the Current Level
    int nxtLevel = 0;                               // Number for the Next Level to be Loaded (Loads Main Menu if set to 0)

    AudioSource aud;                                // AudioSource
    public AudioClip coinCollect;                   // Noise for Coin Pickup
    public AudioClip doorJingle;                    // Noise for Door Opening

    ParticleSystem part;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        save = GameObject.Find("Save");
        endPlayers = new List<PlayerScript>();
        if (currentLevel <= 2 && currentLevel >= 1)
        {
            nxtLevel = currentLevel + 1;
        }
        else
        {
            nxtLevel = 0;
        }
        
        StartCoroutine(introText());
        Debug.Log(activePlayers.Count);
        coinText.text = "Coins Collected: " + coinsCollected + "\nCoins Left: " + GameObject.FindGameObjectsWithTag("coin").Length;
        part = GetComponent<ParticleSystem>();
        players = new PlayerScript[GameObject.FindGameObjectsWithTag("Player").Length];
        int count = 0;
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("Player"))
        {
            players[count] = b.GetComponent<PlayerScript>();
            count++;
        }
        cameraPan = true;
        selectPlayer(activePlayers[0]);
        //endLevel();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire4") == 1)                                                            // Show Arrows over Coins (to help find more Hidden ones)
        {
            foreach (GameObject arrw in GameObject.FindGameObjectsWithTag("arrow")) // Destroy Previous Arrows
            {
                Destroy(arrw);
            }
            foreach (GameObject coin in GameObject.FindGameObjectsWithTag("coin")) // Create Arrow over Each Coin
            {
                Vector3 pos = coin.transform.position + Vector3.up * (2 + (camDistance / 24));
                pos = Camera.main.WorldToScreenPoint(pos);
                GameObject arrw = Instantiate(arrow);
                arrw.name = coin.name;
                RectTransform rct = arrw.GetComponent<RectTransform>();
                rct.position = pos;
                rct.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                arrw.transform.parent = GameObject.Find("Canvas").transform;
                Vector3 rctpos = rct.position;
                rct.eulerAngles = new Vector3(0f, 0f, 0f);
                if (rctpos.x > Screen.width-15)                                   // Change rotation of Arrow when off screen
                {
                    rctpos.x = Screen.width-15;
                    rct.eulerAngles = new Vector3(0f, 0f, 90f);                    
                }
                if (rctpos.x < 0 + 15)                                            // Change rotation of Arrow when off screen
                {
                    rctpos.x = 0 + 15;
                    rct.eulerAngles = new Vector3(0f, 0f, -90f);
                }
                if (rctpos.y > Screen.height-15)                                  // Change rotation of Arrow when off screen
                {
                    rctpos.y = Screen.height-15;
                    rct.eulerAngles = new Vector3(0f, 0f, 180f);
                }
                if (rctpos.y < 0 + 15)                                            // Change rotation of Arrow when off screen
                {
                    rctpos.y = 0 + 15;
                }
                if (rctpos.z < 0)                                                // Arrows Disapear when player not Facing Arrow Direction
                {
                    arrw.SetActive(false);
                }
                rct.position = rctpos;
            }
        }
        else
        {
            foreach (GameObject arrw in GameObject.FindGameObjectsWithTag("arrow")) // Destroy Arrows when Coin Search not Pressed
            {
                Destroy(arrw);
            }
        }

        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("coin"))     // Spin coins
        {
            coin.transform.Rotate(new Vector3(0f,1f,0f));
        }

        float cAxis = Input.GetAxis("CameraV");                 // Set Camera Axis
        float tAxis = Input.GetAxis("CameraH");                 // Set Camera Axis

        if (!cameraPan)                                         // Set Camera Distance from Player
        {
            camDistance += cAxis * camSpeed * Time.deltaTime;
        } 
        

        if (camDistance > -6)                                   // Set Camera Distance Restraints
        {
            camDistance = -6;
        }
        if (camDistance < -30)                                  // Set Camera Distance Restraints
        {
            camDistance = -30;
        }

        Vector3 camPos = camTarg.transform.position + camTarg.transform.forward * (camDistance) + Vector3.up * (camDistance/-2);
        Camera.main.transform.position = camPos;
        Camera.main.transform.LookAt(camTarg.transform);

        if (!cameraPan)                                         // Set Camera Rotation
        {
            camTarg.transform.Rotate(0f, 60 * tAxis * Time.deltaTime, 0f);
        }

        if (selectedPlayer != null)
        {

            if (cameraPan)                                      // Camera Pan controls
            {
                Vector3 destination = target.transform.position;
                Transform targ = camTarg.transform;
                Vector3 targPos = destination;
                //targPos.y = 2.5f;
                Vector3 vecToDist = (targPos - targ.position).normalized;
                float dist = Vector3.Distance(targPos, targ.position);

                if (dist <= 0.5)
                {
                    //dist = 0.001f;
                    cameraPan = false;
                }
                targ.position += vecToDist * (camSpeed - camSpeed / (dist + 1)) * Time.deltaTime;
            }
            else                                                // Follow target if not panning
            {
                camTarg.transform.position = target.transform.position;
                //Debug.Log("Beep");
            }

        }
        if (!paused)
        {
            if ((Input.GetButtonDown("Fire3") || Input.GetButtonDown("Fire2")) && (selectedPlayer.cc.isGrounded || (selectedPlayer.type == 1 && selectedPlayer.inWater)))
            {
                int currentSel = selectNum;
                if (Input.GetButtonDown("Fire3"))             // Select Next Character
                {
                    selectNum++;
                }
                else                                         // Select Previous Character
                {
                    selectNum--;
                }
                if (selectNum >= activePlayers.Count)
                {
                    selectNum = 0;
                }
                if (selectNum < 0)
                {
                    selectNum = activePlayers.Count - 1;
                }
                if (activePlayers[selectNum] != selectedPlayer)
                {
                    if (activePlayers[selectNum])
                    {
                        selectPlayer(activePlayers[selectNum]);
                    }
                    else
                    {
                        selectNum = currentSel;
                    }
                }
            }
        }
    }

    IEnumerator introText()                               // Show Introduction Text then disable it
    {
        yield return new WaitForSeconds(10);
        intro.SetActive(false);
    }

    public void activatePlayer(PlayerScript player)      // Allow player to control character
    {
        activePlayers.Add(player);
    }

    public void addEnd(PlayerScript player)              // Add character to endList and check if Level over
    {
        endPlayers.Add(player);
        if(endPlayers.Count == players.Length)
        {
            endLevel();
        }
        Debug.Log(endPlayers.Count + "," + players.Length);
    }

    public void removeEnd(PlayerScript player)          // Remove character from endList
    {
        endPlayers.Remove(player);
        Debug.Log(endPlayers.Count + "," + players.Length);
    }

    public void endLevel()                             // End the Level
    {
        Text txt = save.GetComponent<Text>();
        int unlockedLevels = int.Parse(txt.text);
        if (nxtLevel > unlockedLevels)
        {
            unlockedLevels = nxtLevel;
        }
        txt.text = unlockedLevels.ToString();
        door.GetComponent<Animator>().SetTrigger("Close");
        paused = true;
        endgame = true;
        menu.toggleMenu(endMenu);
        menu.changeInteraction(endMenu);
    }

    public void loadNextLevel()                        // Load next Level (or Main Menu if nxtLevel = 0)
    {
        if (nxtLevel == 0)
        {
            menu.loadScene("Main Menu");
        }
        else
        {
            menu.loadScene("level" + nxtLevel);
        }
    }

    public void increaseCoin()                        // Increase coin counter and check if All coins collected and Open Door
    {
        aud.PlayOneShot(coinCollect);
        coinsCollected++;
        coinText.text = "Coins Collected: " + coinsCollected + "\nCoins Left: " + (GameObject.FindGameObjectsWithTag("coin").Length-1);
        if (GameObject.FindGameObjectsWithTag("coin").Length - 1 <= 0)
        {
            openDoor();
        }
    }

    public void openDoor()                            // Starts Animation to Open Door
    {
        StartCoroutine(doorPan());    
    }

    public void startFocusCam(GameObject targ)       // Runs FocusCam Coroutine
    {
        target = targ;
        StartCoroutine(focusCam());
    }

    IEnumerator focusCam()                          // Focuses camera on target for a few Moments, then returns to player
    {
        cameraPan = true;
        paused = true;
        camDistance = -15;
        camTarg.transform.LookAt(target.transform);
        camTarg.transform.localRotation = new Quaternion(0f, camTarg.transform.rotation.y, 0f, camTarg.transform.rotation.w);
        yield return cameraPan = false;
        yield return new WaitForSeconds(3f);
        cameraPan = true;
        paused = false;
        if (selectedPlayer != null)
        {
            target = selectedPlayer.gameObject;
        }
        else
        {
            selectPlayer(activePlayers[0]);
        }
    }

    IEnumerator doorPan()                          // Focuses camera on Door (specifically) for a few Moments, then returns to player
    {
        cameraPan = true;
        paused = true;
        camDistance = -15;
        target = door;
        camTarg.transform.LookAt(target.transform);
        camTarg.transform.localRotation = new Quaternion(0f, camTarg.transform.rotation.y, 0f, camTarg.transform.rotation.w);
        yield return cameraPan = false;
        door.GetComponent<Animator>().SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        aud.PlayOneShot(doorJingle);
        yield return new WaitForSeconds(3f);
        cameraPan = true;
        paused = false;
        if (selectedPlayer != null)
        {
            target = selectedPlayer.gameObject;
        } else
        {
            selectPlayer(activePlayers[0]);
        }
    }

    

    void selectPlayer(PlayerScript player)      // Sets inputed character as current character being controlled
    {
        if (player != null)
        {
            if (selectedPlayer != null)
            {
                selectedPlayer.re.material.color = selectedPlayer.playerColor;
                //part.transform.position = selectedPlayer.transform.position + Vector3.up * 2;
                selectedPlayer.selected = false;
            }
            selectedPlayer = player;
            selectedPlayer.selected = true;
            Color col = selectedPlayer.playerColor;
            col.b = col.b - 0.5f;
            col.g = col.g - 0.5f;
            col.r = col.r - 0.5f;
            selectedPlayer.re.material.color = col;
            //part.transform.LookAt(selectedPlayer.transform.position + Vector3.up * 2);
            target = selectedPlayer.gameObject;
            cameraPan = true;

            //part.Play();
        }
        else
        {
            if (selectedPlayer != null)
            {
                selectedPlayer.selected = false;
                selectedPlayer.re.material.color = selectedPlayer.playerColor;
                selectedPlayer = null;
            }
        }   
    }
}
