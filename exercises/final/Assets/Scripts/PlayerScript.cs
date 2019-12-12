using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    AudioSource aud;                        // Audiosource
    public AudioClip jump;                  // Sound for jumping

    GameObject camTarg;                     // Camera Target (used to determine movement)

    public float moveSpeed = 5f;            // Movement speed of Character
    float setSpeed = 5f;                    // Default speed of Character
    float rotateSpeed = 60f;                // Rotate speed of Character
    float jumpForce = 0.38f;                // Force at which CHaracter Jumps

    public bool inWater = false;            // Bool for when Character in Water

    public bool selected;                   // Bool for when Selected

    public bool canJump;                    // Bool for whether Character can Jump
    bool inJumpZone = false;                // Bool for whether Character is in a Zone where they cannot Jump

    public bool hooked;                     // Bool for when Character is Riding another Character

    public GameObject hat;                  // Trigger that checks if Character is ontop of Character

    public PlayerScript bottom;             // Character the Character is Standing on
    public PlayerScript holding;            // Character the CHaracter is Holding

    public Color playerColor;                      // Character's Color (Red - Type 0, Blue - Type 1, Yellow - Type 2)

    public int type;                        // Character's Type (Type 0 - Red, Type 1 - Blue, Type 2 - Yellow)

    public float gravMod = 0.15f;           // The Multiplier for Gravities effect on the Character

    public float yVel;                      // Value of Character's 'Falling' movement

    bool prevGrounded;                      // Checks if previously Grounded

    float hAxis;                            // Horizontal Movement Axis 
    float vAxis;                            // Vertical Movement Axis 
    float tAxis;                            // Horizontal + Vertical Movement Axis 

    Vector3 destination;                    // Where the character looks before moving 

    public CharacterController cc;          // Character's Character Controller
    Animator ani;                           // Character's Animator
    public Renderer re;                     // Character's Renderer
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        inJumpZone = false;
        camTarg = Camera.main.gameObject;
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
        if (selected && !gm.paused)
        {
            hAxis = Input.GetAxis("Horizontal");                        // Sets hAxis to Horizontal Axis
            vAxis = Input.GetAxis("Vertical");                          // Sets vAxis to Horizontal Axis

        }
        else                                                            // Sets hAxis and vAxis to 0 when not Selected
        {
            hAxis = 0;
            vAxis = 0;

        }

        tAxis = Mathf.Abs(hAxis) + Mathf.Abs(vAxis);                   // Sets tAxis to vAxis + hAxis for Animation Speed

        if (cc.isGrounded || (type == 1 && inWater))                   // Determine if Character can fall/Jump
        {
            if (yVel < -0.10)
            {
                yVel = 0;
            }

            if (Input.GetAxis("Jump") == 1 && selected && canJump && !gm.paused)
            {
                yVel = jumpForce;
                aud.PlayOneShot(jump);
            }
            

        } else
        {
            if (!hooked || selected)
            {
                yVel += Physics.gravity.y * gravMod * Time.deltaTime;
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

        if (selected && !gm.paused)                   // Turn to look at Walking Direction
        {
            Vector3 destination = transform.position + (camTarg.transform.right * (hAxis)) + (camTarg.transform.forward * (vAxis));
            destination.y = transform.position.y;
            Vector3 vecToDist = (destination - transform.position).normalized;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, rotateSpeed * tAxis * Time.deltaTime, 1 * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        Vector3 amountToMove = transform.forward * tAxis * moveSpeed * Time.deltaTime;        // Move when tAxis > 0
        amountToMove.y = yVel;                                                                // Fall yVel

        if ((!hooked || selected) && !gm.paused)                                              // Do full Movement
        {
            cc.Move(amountToMove);
        }

        if (tAxis > 0 && (ani.GetCurrentAnimatorStateInfo(0).IsName("Walk") || ani.GetCurrentAnimatorStateInfo(0).IsName("Swim")))       // Run animation Speed based on tAxis
        {
            ani.speed = tAxis;
        } else
        {
            ani.speed = 1;
        }

        if (selected)                   // Set Character Collider size and Character Speed Based on Amount of Characters stacked
        {
            cc.enabled = true; 
            setSize(findSize(0));
        } else
        {
            setSize(0);
            cc.enabled = false;
           
        }
        
        ani.SetFloat("speed", tAxis);
        ani.SetFloat("jump", yVel);
        ani.SetBool("grounded", cc.isGrounded);
        ani.SetBool("swim", inWater);

        prevGrounded = cc.isGrounded;
        
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("hat") && other.gameObject != hat)                                 // Sets Character stacked on another Character
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
            Debug.Log(name + ": Hat Enter");
        }

        if (other.CompareTag("end"))                                                        // Checks if Character in End Area
        {
            gm.addEnd(this);
        }

        if (other.CompareTag("water"))                                                      // Checks if Character in Water
        {
            Debug.Log(name + ": Water");
            yVel = 0;
            inWater = true;
            if (type != 1)
            {
                gravMod = 0.075f;
            }
            else
            {
                gravMod = 0f;
                moveSpeed = 5f;
            }
        }

        if (other.CompareTag("no-jump"))                                                      // Checks if Character in No-Jump Area
        {
            canJump = false;
            //inJumpZone = true;
        }
        if (other.CompareTag("coin"))
        {
            gm.increaseCoin();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("player2") && selected)                                          // Checks if Character in Player1 Area (To Activate Player1)
        {
            gm.startFocusCam(GameObject.Find("Player 2"));
            gm.activatePlayer(GameObject.Find("Player 2").GetComponent<PlayerScript>());
            Destroy(other.gameObject);
        }

        if (other.CompareTag("player3") && selected)                                          // Checks if Character in Player2 Area (To Activate Player2)
        {
            gm.startFocusCam(GameObject.Find("Player 3"));
            gm.activatePlayer(GameObject.Find("Player 3").GetComponent<PlayerScript>());
            Destroy(other.gameObject);
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("hat") && other.gameObject != hat && selected)                    // Sets Character no longer stacked on another Character
        {
            PlayerScript sat = other.GetComponentInParent<PlayerScript>();
            if (sat == bottom)
            {
                
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

        if (other.CompareTag("end"))                                                        // Checks if Character no longer in End Area
        {
            gm.removeEnd(this);
        }

        if (other.CompareTag("water"))                                                      // Checks if Character no longer in Water
        {
            inWater = false;
            gravMod = 0.15f;
        }

        if (other.CompareTag("no-jump"))                                                    // Checks if Character no longer in No-Jump Zone
        {
            if (holding == null)
            {
                canJump = true;
            }
            inJumpZone = false;
        }
    }

    void setSize(int num)                                     // Sets the size of Character's Character Controller and Character Speed
    {
        cc.height = 1.38f + (1.50f * num);
        cc.center = new Vector3(0f, -0.2f, 0f) + (new Vector3(0f, 0.7f,0f) * num);
        if ((type == 1 && !inWater) || (type == 2))
        {
            moveSpeed = setSpeed * (1f / (num + 1f));
        }
        
    }

    public int findSize(int num)                              // Code used to Determine Character Controller Size
    {
        if (holding != null && bottom != null && holding == bottom)
        {
            holding.holding = null;
            if (!inJumpZone)
            {
                holding.canJump = true;
            }
            bottom = null;
        }

        if (holding == this)
        {
            //Debug.Log("Here!");
            holding = null;
            return 0;
        }
        else if (holding != null)
        {
            //Debug.Log(num + ": " + holding.name);
            if(num > 3)
            {
                return 3;
            }
            return holding.findSize(num + 1);
        }
        else
        {
            return num;
        }
    }
}
