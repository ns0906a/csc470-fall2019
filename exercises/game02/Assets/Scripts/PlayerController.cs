using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc; // CharacterController Instance
    public GameObject gc; //Game Controller
    GameManager gameCode;

    public AudioClip death;
    public AudioClip select;
    public Sprite deathpic;

    AudioSource au;

    float moveSpeed = 10;
    float yVel = 0;
    float jumpForce = 2.5f;
    float gravityMod = 0.03f;
    float camDistance = 10;
    float maxDis = 30;
    float minDis = 10;
    float camSpeed = 5;

    bool prevIsGround;
    public bool alive = true;
    public bool camUp = false;
    bool click = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        yVel = 0;
        gravityMod = 0.03f;
        prevIsGround = cc.isGrounded;
        alive = false;
        gameCode = gc.GetComponent<GameManager>();
        au = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        float tAxis = Input.GetAxis("TurnHorizontal");
        float cAxis = Input.GetAxis("camVertical");
        float jump = Input.GetAxis("Jump");
        float camBut = Input.GetAxis("Fire3");
        Vector3 amountToMove;
        CheckDead();
        if (alive)
        {
            transform.Rotate(0f, tAxis * Time.deltaTime * (moveSpeed * 10), 0f);
            amountToMove = ((transform.right * hAxis) + (transform.forward * vAxis)) * (moveSpeed * Time.deltaTime);

            if (cc.isGrounded)
            {
                if (!prevIsGround && cc.isGrounded)
                {
                    yVel = 0;
                }
                if (Input.GetAxis("Jump") > 0)
                {
                    yVel = jumpForce;
                }
            }
            else
            {
                yVel += Physics.gravity.y * gravityMod;
            }

            if(camBut > 0)
            {
                click = true;
            }
            if(camBut <= 0 && click)
            {
                camUp = !camUp;
                click = false;
            }

            amountToMove.y = yVel;
            cc.Move(amountToMove);

            prevIsGround = cc.isGrounded;

            camDistance += cAxis * camSpeed * Time.deltaTime;

            if(camDistance > maxDis)
            {
                camDistance = maxDis;
            }
            if(camDistance < minDis)
            {
                camDistance = minDis;
            }


            if(camUp)
            {
                Vector3 camPos = new Vector3(transform.position.x, camDistance, transform.position.z);
                Camera.main.transform.position = camPos;
                camPos.y = 0f;
                Camera.main.transform.LookAt(camPos);
            }
            else
            {
                Vector3 camPos = transform.position + transform.forward * (camDistance/-2) + Vector3.up * 3;
                Camera.main.transform.position = camPos;
                Camera.main.transform.LookAt(transform);
            }

        }
        else
        {
            if(gameCode.getSimulate())
            {
                GetComponentInChildren<SpriteRenderer>().sprite = deathpic;
                gravityMod = 0.0001f;
                Camera.main.transform.rotation = new Quaternion(0f,0f,0f,0f);
                gameCode.gameOver();
                au.PlayOneShot(death);
            }
            yVel += Physics.gravity.y * gravityMod;
            amountToMove = new Vector3(0f, yVel, 0f);
            transform.Rotate(5f,5f,5f);
            Camera.main.transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
            Camera.main.transform.LookAt(transform);
            cc.Move(amountToMove);
        }
        


    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        if(other.gameObject.CompareTag("cell"))
        {
            other.gameObject.GetComponent<CellScript>().map.color = Color.red;
            float fire = Input.GetAxis("Fire1");

            if (fire > 0f)
            {
                if (!other.gameObject.GetComponent<CellScript>().selected)
                {
                    other.gameObject.GetComponent<CellScript>().selected = true;
                    au.PlayOneShot(select);
                }
            }
            
        }        
    }
    
    void CheckDead()
    {
        if(transform.position.y < 0 && alive)
        {
            alive = false;
        }
    } 

    public void setUp(float x, float y)
    {

        transform.position = new Vector3(x, 2.8f, y);
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        alive = true;
        yVel = 0;
        gravityMod = 0.03f;
        prevIsGround = cc.isGrounded;
        gameObject.SetActive(true);
    }
}
