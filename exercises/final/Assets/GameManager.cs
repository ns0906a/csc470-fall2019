using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject camTarg;
    public PlayerScript selectedPlayer;
    public PlayerScript[] players;

    public Text coinText;

    public int coinsCollected = 0;

    int selectNum = 0;
    bool cameraPan;

    float camDistance = -6;
    public float camSpeed = 5;

    ParticleSystem part;

    // Start is called before the first frame update
    void Start()
    {
        coinText.text = "Coins Collected: " + coinsCollected + "\nCoins Left: " + GameObject.FindGameObjectsWithTag("coin").Length;
        part = GetComponent<ParticleSystem>();
        players = new PlayerScript[GameObject.FindGameObjectsWithTag("Player").Length];
        int count = 0;
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("Player"))
        {
            players[count] = b.GetComponent<PlayerScript>();
            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {

        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("coin"))
        {
            coin.transform.Rotate(new Vector3(0f, 1f, 0f));
        }

        float cAxis = Input.GetAxis("Camera");

        camDistance += cAxis * camSpeed * Time.deltaTime;

        if (camDistance > -3)
        {
            camDistance = -3;
        }
        if (camDistance < -20)
        {
            camDistance = -20;
        }

        Vector3 camPos = camTarg.transform.position + camTarg.transform.forward * (camDistance) + Vector3.up * (camDistance/-2);
        Camera.main.transform.position = camPos;
        Camera.main.transform.LookAt(camTarg.transform);

        if (selectedPlayer != null)
        {
            
            if (cameraPan)
            {
                Vector3 destination = selectedPlayer.transform.position;
                Transform targ = camTarg.transform;
                Vector3 targPos = destination;
                //targPos.y = 2.5f;
                Vector3 vecToDist = (targPos - targ.position).normalized;
                float dist = Vector3.Distance(targPos, targ.position);

                if (dist <= 0.1)
                {
                    //dist = 0.001f;
                    cameraPan = false;
                }
                targ.position += vecToDist * (camSpeed - camSpeed / (dist + 1)) * Time.deltaTime;
            }
            else
            {
                camTarg.transform.position = selectedPlayer.transform.position;
            }


        }
        
        if (Input.GetButtonDown("Fire3") || Input.GetButtonDown("Fire2"))
        {
            if (Input.GetButtonDown("Fire3"))
            {
                selectNum++;
            }
            else
            {
                selectNum--;
            }
            if (selectNum >= players.Length)
            {
                selectNum = 0;
            }
            if (selectNum < 0)
            {
                selectNum = players.Length-1;
            }
            if (players[selectNum] != selectedPlayer)
            {
                selectPlayer(players[selectNum]);
            }            
        }
    }

    public void increaseCoin()
    {
        coinsCollected++;
        coinText.text = "Coins Collected: " + coinsCollected + "\nCoins Left: " + (GameObject.FindGameObjectsWithTag("coin").Length-1);
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
            col.r = col.r - 0.5f;
            col.g = col.g - 0.5f;
            selectedPlayer.re.material.color = col;
            //part.transform.LookAt(selectedPlayer.transform.position + Vector3.up * 2);
            cameraPan = true;
            
            //part.Play();
        } else
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
