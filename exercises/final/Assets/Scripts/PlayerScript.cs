using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    GameObject camTarg;

    public float moveSpeed = 5f;
    float setSpeed = 5f;
    float rotateSpeed = 60f;
    float jumpForce = 0.38f;

    bool inWater = false;

    public bool selected;
    public bool selected2;

    public bool canJump;
    bool inJumpZone = false;

    public bool hooked;

    public GameObject hat;

    public PlayerScript bottom;
    public PlayerScript holding;

    public Color playerColor;

    public int type;

    public float gravMod = 0.15f;

    public float yVel;

    bool prevGrounded;

    float hAxis;
    float vAxis;
    float tAxis;

    Vector3 destination;

    CharacterController cc;
    Animator ani;
    public Renderer re;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        inJumpZone = false;
        camTarg = Camera.main.gameObject;//GameObject.Find("CameraTarget");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (type == 1)
        {
            playerColor = Color.blue;
        } else if (type == 2)
        {
            playerColor = Color.yellow;
            jumpForce *= 1.5f;

        } else
        {
            playerColor = Color.red;
        }
        cc = GetComponent<CharacterController>();
        ani = GetComponent<Animator>();
        re.material.color = playerColor;

        prevGrounded = cc.isGrounded;
        canJump = true;
        
        
        hooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (selected)
        //{
            //Debug.Log(name + ": " + canJump);
        //}
        
        if (selected && !gm.paused)
        {
            hAxis = Input.GetAxis("Horizontal");
            vAxis = Input.GetAxis("Vertical");
            
        } else
        {
            hAxis = 0;
            vAxis = 0;

        }
        tAxis = Mathf.Abs(hAxis) + Mathf.Abs(vAxis);

        if (cc.isGrounded || (type == 1 && inWater))
        {
            if (yVel < -0.10)
            {
                yVel = 0;
            }

            if (Input.GetAxis("Jump") == 1 && selected && canJump && !gm.paused)
            {
                yVel = jumpForce;
            }
            

        } else
        {
            if (!hooked || selected)
            {
                yVel += Physics.gravity.y * gravMod * Time.deltaTime;
            }else if (hooked)
            {
                //if (bottom != null)
                //{
                    //Vector3 fall = transform.position; 
                    //fall.y = bottom.transform.position.y + 1.5f;
                //}
            }

            

            if (yVel < -gravMod && inWater)
            {
                yVel = -gravMod;
            }

            if (Input.GetAxis("Jump") == 0 && yVel > 0 && selected && canJump && !gm.paused)
            {
                yVel = 0;
            }
        }

        if (tAxis > 1)
        {
            tAxis = 1;
        }

        if (selected && !gm.paused)
        {
            //Vector3 destination = new Vector3(transform.position.x + (hAxis), transform.position.y, transform.position.z + (vAxis));
            Vector3 destination = transform.position + (camTarg.transform.right * (hAxis)) + (camTarg.transform.forward * (vAxis));
            destination.y = transform.position.y;
            Vector3 vecToDist = (destination - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, rotateSpeed * tAxis * Time.deltaTime, 1 * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        Vector3 amountToMove = transform.forward * tAxis * moveSpeed * Time.deltaTime;   
        amountToMove.y = yVel;
        
        if ((!hooked || selected) && !gm.paused)
        {
            cc.Move(amountToMove);
        }
        



        if (tAxis > 0 && (ani.GetCurrentAnimatorStateInfo(0).IsName("Walk") || ani.GetCurrentAnimatorStateInfo(0).IsName("Swim")))
        {
            ani.speed = tAxis;
        } else
        {
            ani.speed = 1;
        }
        if (selected)
        {
            setSize(findSize(0));
        } else
        {
            setSize(0);
        }
        
        ani.SetFloat("speed", tAxis);
        ani.SetFloat("jump", yVel);
        ani.SetBool("grounded", cc.isGrounded);
        ani.SetBool("swim", inWater);

        prevGrounded = cc.isGrounded;

        if (selected)
        {
            //Debug.Log(name + ": " + yVel + ", " + prevGrounded + ", " + cc.isGrounded);
        }

        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("block"))
        {
            Vector3 move = transform.forward * Time.deltaTime;
            move.y = hit.gameObject.transform.position.y;
            hit.gameObject.GetComponent<CharacterController>().Move(move);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("hat") && other.gameObject != hat)
        {
            PlayerScript sat = other.GetComponentInParent<PlayerScript>();
            if (bottom == null && sat != bottom && sat.holding == null && sat.holding != this)
            {
                sat.canJump = false;
                hooked = true;
                transform.parent = other.transform.parent;
                if (sat.holding == null)
                {
                    sat.holding = this;
                }
                bottom = sat;
            }
        }

        if (other.CompareTag("end"))
        {
            gm.addEnd(this);
        }

        if (other.CompareTag("water"))
        {
            yVel = 0;
            inWater = true;
            if (type != 1)
            {
                gravMod = 0.075f;
            }
            else
            {
                gravMod = 0f;
            }
        }

        if (other.CompareTag("no-jump"))
        {
            canJump = false;
            //inJumpZone = true;
        }
        if (other.CompareTag("coin"))
        {
            gm.increaseCoin();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("player2") && selected)
        {
            gm.startFocusCam(GameObject.Find("Player 2"));
            gm.activatePlayer(GameObject.Find("Player 2").GetComponent<PlayerScript>());
            Destroy(other.gameObject);
        }

        if (other.CompareTag("player3") && selected)
        {
            gm.startFocusCam(GameObject.Find("Player 3"));
            gm.activatePlayer(GameObject.Find("Player 3").GetComponent<PlayerScript>());
            Destroy(other.gameObject);
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hat") && other.gameObject != hat)
        {
            PlayerScript sat = other.GetComponentInParent<PlayerScript>();
            if (sat == bottom)
            {
                Debug.Log(name + ": Exit-Check");
                if (!inJumpZone)
                {
                    bottom.canJump = true;
                }
                
               
                hooked = false;
                transform.parent = null;
                if (sat.holding == this)
                {
                    sat.holding = null;
                }
                bottom = null;
            }            
        }

        if (other.CompareTag("end"))
        {
            gm.removeEnd(this);
        }

        if (other.CompareTag("water"))
        {
            inWater = false;
            gravMod = 0.15f;
        }

        if (other.CompareTag("no-jump"))
        {
            if (holding == null)
            {
                canJump = true;
            }
            inJumpZone = false;
        }
    }

    void setSize(int num)
    {
        cc.height = 1.38f + (1.50f * num);
        cc.center = new Vector3(0f, -0.2f, 0f) + (new Vector3(0f, 0.7f,0f) * num);
        if (type != 0)
        {
            moveSpeed = setSpeed * (1f / (num + 1f));
        }
        
    }

    public int findSize(int num)
    {
        if (holding == this)
        {
            //Debug.Log("Here!");
            holding = null;
            return 0;
        }
        else if (holding != null)
        {
            //Debug.Log(num + ": " + holding.name);
            return holding.findSize(num + 1);
        }
        else
        {
            return num;
        }
    }
}
