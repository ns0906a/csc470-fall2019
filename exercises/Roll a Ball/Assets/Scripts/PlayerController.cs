using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    public Text countText;
    public Text winText;
    public Text timeText;
    public float speed;
    public GameObject PickUp;
    static int PickUpNumber = 0;

    private Rigidbody rb;
    private int count;
    private AudioSource sound;
    

    void Start ()
    {
        PickUpNumber++;
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winText.text = "";
        timeText.text = "Time: " + Time.timeSinceLevelLoad.ToString("0.00");
        sound = GetComponent<AudioSource>();
        SetTable();

    }

    void SetTable() //Initiate PickUps
    {
        for (int i = 0; i < PickUpNumber; i++)
        {
            float rx = Random.Range(-8f, 8f);
            float rz = Random.Range(-8f, 8f);
            Instantiate(PickUp, new Vector3(rx, 0.5f, rz), Quaternion.identity);
        }
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    void Update ()
    {

        if(Input.GetKey(KeyCode.R))
        {
            PickUpNumber = 0;
            SceneManager.LoadScene("Minigame");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Minigame");
        }

            if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        timeText.text = "Time: " + Time.timeSinceLevelLoad.ToString("0.00");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
            sound.Play();
        }
    }


    void SetCountText ()
    {
        countText.text = "Count: " + count.ToString() + "/" + PickUpNumber.ToString();
        if (count >= PickUpNumber)
        {
            winText.text = "You Win!\n Press 'Space' to Continue\n Press 'R' to Restart\nTime: " + Time.timeSinceLevelLoad.ToString("0.00") + " seconds";
            timeText.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
        }
    }
}
