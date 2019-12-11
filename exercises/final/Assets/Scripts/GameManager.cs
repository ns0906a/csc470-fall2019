using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject camTarg;
    public GameObject endMenu;
    public GameObject intro;
    public GameObject arrow;
    public PlayerScript selectedPlayer;
    public PlayerScript[] players;
    public List<PlayerScript> activePlayers;
    public List<GameObject> arrows;
    List<PlayerScript> endPlayers;
    public MenuScript menu;

    public GameObject door;

    public GameObject save;

    public Text coinText;

    public int coinsCollected = 0;

    public int selectNum = 0;
    bool cameraPan = true;

    float camDistance = -15;
    public float camSpeed = 5;

    public bool paused;
    public bool endgame;

    GameObject target;

    float timer = 0.0f;
    public int currentLevel;
    int nxtLevel = 0;

    ParticleSystem part;

    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetAxis("Fire4") == 1)
        {
            foreach (GameObject arrw in GameObject.FindGameObjectsWithTag("arrow"))
            {
                Destroy(arrw);
            }
            foreach (GameObject coin in GameObject.FindGameObjectsWithTag("coin"))
            {
                Vector3 pos = coin.transform.position + Vector3.up * (2 + (camDistance / 24));
                pos = Camera.main.WorldToScreenPoint(pos);
                GameObject arrw = Instantiate(arrow);
                arrw.name = coin.name;
                RectTransform rct = arrw.GetComponent<RectTransform>();
                rct.position = pos;
                rct.localScale = new Vector3(0.5f, 0.5f, 0.5f);// + (new Vector3(0.5f, 0.5f, 0.5f) * (-camDistance / 48));
                arrw.transform.parent = GameObject.Find("Canvas").transform;
                Vector3 rctpos = rct.position;
                rct.eulerAngles = new Vector3(0f, 0f, 0f);
                if (rctpos.x > Screen.width-15)
                {
                    rctpos.x = Screen.width-15;
                    rct.eulerAngles = new Vector3(0f, 0f, 90f);                    
                }
                if (rctpos.x < 0 + 15)
                {
                    rctpos.x = 0 + 15;
                    rct.eulerAngles = new Vector3(0f, 0f, -90f);
                }
                if (rctpos.y > Screen.height-15)
                {
                    rctpos.y = Screen.height-15;
                    rct.eulerAngles = new Vector3(0f, 0f, 180f);
                }
                if (rctpos.y < 0 + 15)
                {
                    rctpos.y = 0 + 15;
                }
                if (rctpos.z < 0)
                {
                    arrw.SetActive(false);
                }
                rct.position = rctpos;
            }
        }
        else
        {
            foreach (GameObject arrw in GameObject.FindGameObjectsWithTag("arrow"))
            {
                Destroy(arrw);
            }
        }

        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("coin"))
        {
            coin.transform.Rotate(new Vector3(0f,1f,0f));
        }

        float cAxis = Input.GetAxis("CameraV");
        float tAxis = Input.GetAxis("CameraH");

        if (!cameraPan)
        {
            camDistance += cAxis * camSpeed * Time.deltaTime;
        } 
        

        if (camDistance > -6)
        {
            camDistance = -6;
        }
        if (camDistance < -30)
        {
            camDistance = -30;
        }

        Vector3 camPos = camTarg.transform.position + camTarg.transform.forward * (camDistance) + Vector3.up * (camDistance/-2);
        Camera.main.transform.position = camPos;
        Camera.main.transform.LookAt(camTarg.transform);

        if (!cameraPan)
        {
            camTarg.transform.Rotate(0f, 60 * tAxis * Time.deltaTime, 0f);
        }

        if (selectedPlayer != null)
        {

            if (cameraPan)
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
            else
            {
                camTarg.transform.position = target.transform.position;
                //Debug.Log("Beep");
            }

        }
        if (!paused)
        {
            if (Input.GetButtonDown("Fire3") || Input.GetButtonDown("Fire2"))
            {
                int currentSel = selectNum;
                if (Input.GetButtonDown("Fire3"))
                {
                    selectNum++;
                }
                else
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

    IEnumerator introText()
    {
        yield return new WaitForSeconds(10);
        intro.SetActive(false);
    }

    public void activatePlayer(PlayerScript player)
    {
        activePlayers.Add(player);
    }

    public void addEnd(PlayerScript player)
    {
        endPlayers.Add(player);
        if(endPlayers.Count == players.Length)
        {
            endLevel();
        }
        Debug.Log(endPlayers.Count + "," + players.Length);
    }

    public void removeEnd(PlayerScript player)
    {
        endPlayers.Remove(player);
        Debug.Log(endPlayers.Count + "," + players.Length);
    }

    public void endLevel()
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

    public void loadNextLevel()
    {
        if (nxtLevel == 0)
        {
            menu.loadScene("Credts");
        }
        else
        {
            menu.loadScene("level" + nxtLevel);
        }
    }

    public void increaseCoin()
    {
        coinsCollected++;
        coinText.text = "Coins Collected: " + coinsCollected + "\nCoins Left: " + (GameObject.FindGameObjectsWithTag("coin").Length-1);
        if (GameObject.FindGameObjectsWithTag("coin").Length - 1 <= 0)
        {
            openDoor();
        }
    }

    public void openDoor()
    {
        StartCoroutine(doorPan());    
    }

    public void startFocusCam(GameObject targ)
    {
        target = targ;
        StartCoroutine(focusCam());
    }

    IEnumerator focusCam()
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

    IEnumerator doorPan()
    {
        cameraPan = true;
        paused = true;
        camDistance = -15;
        target = door;
        camTarg.transform.LookAt(target.transform);
        camTarg.transform.localRotation = new Quaternion(0f, camTarg.transform.rotation.y, 0f, camTarg.transform.rotation.w);
        yield return cameraPan = false;
        door.GetComponent<Animator>().SetTrigger("Open");
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

    

    void selectPlayer(PlayerScript player)
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
