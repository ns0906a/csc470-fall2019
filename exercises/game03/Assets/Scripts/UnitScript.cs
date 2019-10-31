using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    // Personality Stats
    public string Name;
    public Color playerColor;
    public int Health;
    public int Attack;
    public int Love;
    public int MovementSpeed;
    public int Sentience;
    public bool doneTurn;
    public int action;
    
    public int specialAction;
    public GameObject target;
    public bool sentient;
    public bool deactivated;
    public bool choice;
    public int decision = 0;
    public Vector3 start;

    public GameObject frontRing;
    public GameObject backRing;
    public GameObject basePlate;

    string[] nameDatabase = { "Brain" };

    Color defaultColor;
    public Color hoverColor;
    public Color SelectedColor;
    public bool selected = false;
    bool hover = false;
    public bool fired = false;
    public bool activated = false;
    public Vector3 destination;
    public Renderer re;
    public GameObject laser;
    GameObject fire;

    float Lspeed = 15;
    CharacterController cc;
    GameManager gm;

    // Start is called before the first frame update
    void Start()

    {
        fired = false;

        defaultColor = re.material.color;
        GameObject gmObj = GameObject.Find("GameManager");
        gm = gmObj.GetComponent<GameManager>();

        cc = GetComponent<CharacterController>();

        destination = transform.position;

        SetCorrectColor();
        ColorPlayer(playerColor);
    }

    void ColorPlayer(Color col)
    {
        basePlate.GetComponent<Renderer>().material.color = col;
        backRing.GetComponent<Renderer>().material.color = col;
        frontRing.GetComponent<Renderer>().material.color = col;
    }

    // Update is called once per frame
    void Update()
    {
        if (sentient)
        {
            if (!deactivated)
            {
                if (gm.playerTurn)
                {
                    SentientMechanics();
                }
                if (Health <= 0)
                {
                    deactivated = true;
                    Health = 0;
                    ColorPlayer(Color.black);
                }
            }
            else
            {
                if (Health > 0)
                {
                    ColorPlayer(playerColor);
                    deactivated = false;
                }
                    
            }     
        }
        else // If Robot
        {
            if(Health <= 0)
            {
                Destroy(this.gameObject);
            }

            if(Sentience <= 0)
            {
                sentient = true;
                Sentience = 0;
                action = 0;
                doneTurn = true;
                Health = Random.Range(4, 10);
                if (Random.Range(1,2) == 1)
                {
                    Attack = Random.Range(1, 3);
                    Love = Random.Range(2, 4);
                }
                else
                {
                    Attack = Random.Range(3, 6);
                    Love = Random.Range(1, 2);
                }
                MovementSpeed = Random.Range(2, 4);
                specialAction = Random.Range(3, 5);
                playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                ColorPlayer(playerColor);
                tag = "Bot";
                Name = nameDatabase[Random.Range(0, nameDatabase.Length-1)];
            }

            if (activated)
            {
                GameObject closest = closestObject(transform);
                float dist = 20f;
                if (closest != null)
                {
                    dist = Vector3.Distance(transform.position, closest.transform.position);
                    Debug.Log(closest.GetComponent<UnitScript>().Name + "," + dist);
                }                
                
                if (decision <= 0)
                {
                    doneTurn = false;
                    if (dist > 20)
                    {
                        decision = 1;
                    } else if (dist > 10)
                    {
                        decision = 2;
                    } else
                    {
                        decision = 3;
                    }
                }
                
                if (decision == 1)
                {
                    if (!choice)
                    {
                        destination = transform.position + new Vector3(Random.Range(MovementSpeed * -2, MovementSpeed*2), 0f, Random.Range(MovementSpeed * -2, MovementSpeed*2));
                        start = transform.position;
                        choice = true;
                    }
                    destination.y = transform.position.y;
                    Vector3 vecToDist = (destination - transform.position).normalized;

                    Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, 5 * Time.deltaTime, 1 * Time.deltaTime);
                    Quaternion oldRot = transform.rotation;
                    transform.rotation = Quaternion.LookRotation(newDir);
                    if (Quaternion.Angle(transform.rotation, oldRot) <= 1)
                    {
                        cc.Move(transform.forward * 5 * Time.deltaTime);
                        //Debug.Log(Vector3.Distance(start, transform.position) + "'" + (MovementSpeed * 2));
                        if (Vector3.Distance(start, transform.position) >= MovementSpeed*3 || Vector3.Distance(destination, transform.position) <= 0.5f)
                        {
                            activated = false;
                            choice = false;
                            decision = 0;
                            doneTurn = true;
                        }
                    }
                }
                else if (decision == 2)
                {
                    if (!choice)
                    {
                        start = transform.position;
                        if (closest != null)
                        {
                            destination = closest.transform.position + new Vector3(Random.Range(MovementSpeed * -2, MovementSpeed * 2), 0f, Random.Range(MovementSpeed * -2, MovementSpeed * 2));
                        }
                        else
                        {
                            destination = start;
                        }
                        choice = true;
                    }
                    destination.y = transform.position.y;
                    Vector3 vecToDist = (destination - transform.position).normalized;

                    Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, 5 * Time.deltaTime, 1 * Time.deltaTime);
                    Quaternion oldRot = transform.rotation;
                    transform.rotation = Quaternion.LookRotation(newDir);
                    if (Quaternion.Angle(transform.rotation, oldRot) <= 1)
                    {
                        cc.Move(transform.forward * 5 * Time.deltaTime);
                        if (Vector3.Distance(start, transform.position) >= MovementSpeed*3 || Vector3.Distance(destination, transform.position) <= 0.5f)
                        {
                            activated = false;
                            choice = false;
                            doneTurn = true;
                            decision = 0;
                        }
                    }
                }
                else
                {
                    if (!choice)
                    {
                        if (closest != null)
                        {
                            destination = closest.transform.position;
                        }
                        else if (fire != null)
                        {
                            destination = fire.transform.position;
                        }
                        else
                        {
                            destination = transform.position;
                        }
                        
                        choice = true;
                    }
                    destination.y = transform.position.y;
                    Vector3 vecToDist = (destination - transform.position).normalized;

                    Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, 5 * Time.deltaTime, 1 * Time.deltaTime);
                    Quaternion oldRot = transform.rotation;
                    transform.rotation = Quaternion.LookRotation(newDir);
                    if (Quaternion.Angle(transform.rotation, oldRot) <= 1)
                    {
                        if (!fired)
                        {
                            fire = Instantiate(laser, transform.position + transform.up * 1.5f, transform.rotation);
                            fire.GetComponent<LaserScript>().Damage = Attack;
                            fire.GetComponent<LaserScript>().owner = this.gameObject;
                            fired = true;
                        }
                        if (fire != null)
                        {
                            destination.y = fire.transform.position.y;
                            fire.transform.position += fire.transform.forward * 15 * Time.deltaTime;
                            if (Vector3.Distance(destination, fire.transform.position) < 0.5f || fire.GetComponent<LaserScript>().die)
                            {
                                activated = false;
                                choice = false;
                                decision = 0;
                                doneTurn = true;
                                fired = false;
                                Destroy(fire);
                                fire = null;
                            }
                        }
                    }
                }
            }
        }
    }

    GameObject closestObject(Transform self)
    {
        GameObject target = null;
        float previousDist = Mathf.Infinity;
        float dist = 0;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Bot"))
        {
            if (!bot.GetComponent<UnitScript>().deactivated)
            {
                dist = Vector3.Distance(self.position, self.transform.position);
                if (dist < previousDist)
                {
                    target = bot;
                    previousDist = dist;
                }
            }
        }
        return target;
    }

    void SentientMechanics()
    {
        if (action > 0)
        {
            doneTurn = false;
            if (destination != null)
            {
                destination.y = transform.position.y;
                Vector3 vecToDist = (destination - transform.position).normalized;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, 5 * Time.deltaTime, 1 * Time.deltaTime);
                Quaternion oldRot = transform.rotation;
                transform.rotation = Quaternion.LookRotation(newDir);
                if (Quaternion.Angle(transform.rotation, oldRot) <= 1)
                {
                    if (action == 1)
                    {
                        cc.Move(transform.forward * 5 * Time.deltaTime);
                        if (Vector3.Distance(destination, transform.position) <= 0.5f)
                        {
                            action = 0;
                            doneTurn = true;
                        }
                    }
                    if (action == 2)
                    {
                        if (!fired)
                        {
                            fire = Instantiate(laser, transform.position + transform.up * 1.5f, transform.rotation);
                            fire.GetComponent<LaserScript>().Damage = Attack;
                            fire.GetComponent<LaserScript>().owner = this.gameObject;
                            fired = true;
                        }
                        if (fire != null)
                        {
                            destination.y = fire.transform.position.y;
                            fire.transform.position += fire.transform.forward * 15 * Time.deltaTime;
                            if (Vector3.Distance(destination, fire.transform.position) < 0.5f || fire.GetComponent<LaserScript>().die)
                            {
                                action = 0;
                                doneTurn = true;
                                fired = false;
                                Destroy(fire);
                                fire = null;
                            }
                        }
                    }
                    if (action == 3)
                    {
                        if (target.CompareTag("Bot"))
                        {
                            if (!target.GetComponent<UnitScript>().deactivated)
                            {
                                target.GetComponent<UnitScript>().Attack += Love;
                                action = 0;
                                doneTurn = true;
                            }
                            else
                            {
                                action = 0;
                            }
                        }
                        if (target.CompareTag("Enemy"))
                        {
                            target.GetComponent<UnitScript>().Attack += Love;
                            target.GetComponent<UnitScript>().Sentience -= Love;
                            action = 0;
                            doneTurn = true;
                        }
                        else
                        {
                            action = 0;
                        }

                    }
                    if (action == 4)
                    {
                        if (target.CompareTag("Bot"))
                        {
                            if (!target.GetComponent<UnitScript>().deactivated)
                            {
                                target.GetComponent<UnitScript>().Love += Love;
                                action = 0;
                                doneTurn = true;
                            }  
                            else
                            {
                                action = 0;
                            }
                        }
                        if (target.CompareTag("Enemy"))
                        {
                            target.GetComponent<UnitScript>().Sentience -= Love;
                            action = 0;
                            doneTurn = true;
                        }
                        else
                        {
                            action = 0;
                        }
                    }
                    if (action == 5)
                    {
                        if (target.CompareTag("Bot"))
                        {
                            if (!target.GetComponent<UnitScript>().deactivated)
                            {
                                target.GetComponent<UnitScript>().Health += Love;
                                action = 0;
                                doneTurn = true;
                            }
                            else
                            {
                                action = 0;
                            }
                        }
                        if (target.CompareTag("Enemy"))
                        {
                            target.GetComponent<UnitScript>().Health += Love;
                            target.GetComponent<UnitScript>().Sentience -= Love;
                            action = 0;
                            doneTurn = true;
                        }
                        else
                        {
                            action = 0;
                        }
                    }
                    if (action == 6)
                    {
                        if (target.CompareTag("Bot"))
                        {
                            if(target.GetComponent<UnitScript>().deactivated)
                            {
                                target.GetComponent<UnitScript>().Health += Love;
                                //target.GetComponent<UnitScript>().deactivated = false;
                                action = 0;
                                doneTurn = true;
                                //ColorPlayer(playerColor);
                            } else
                            {
                                action = 0;
                            }
                        }
                        if (target.CompareTag("Enemy"))
                        {
                            target.GetComponent<UnitScript>().Sentience -= Love;
                            action = 0;
                            doneTurn = true;
                        }
                    }
                }
            }
            
        }
    }

    private void OnMouseOver()
    {
        if (sentient)
        {
            if (!gm.ActionSelect() && gm.playerTurn)
            {
                hover = true;
                SetCorrectColor();
            }
        }        
    }

    private void OnMouseExit()
    {
        if (sentient)
        {
            if (!gm.ActionSelect() && gm.playerTurn)
            {
                hover = false;
                SetCorrectColor();
            }
        }
    }

    private void OnMouseDown()
    {
        if (sentient)
        {
            if (!gm.ActionSelect() && gm.playerTurn)
            {
                selected = !selected;
                if (selected)
                {
                    gm.selectUnit(this);
                }
                else
                {
                    gm.selectUnit(null);
                }
                SetCorrectColor();
            }
        }
    }

    public void SetCorrectColor()
    {
        if(selected)
        {
            re.material.color = SelectedColor;
        } else if (hover)
        {
            re.material.color = hoverColor;
        }
        else
        {
            re.material.color = defaultColor;
        }
        if(deactivated)
        {

        }
    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        GameObject hit = other.gameObject;
        if (hit.CompareTag("Enemy") || hit.CompareTag("Bot"))
        {
            destination = transform.position;
        }
        if ((hit.CompareTag("Enemy") || hit.CompareTag("Wall")) && !sentient)
        {
            destination = transform.position - transform.forward * 2;
        }
    }

    void onTriggerEnter(Collider other)
    {

    }

    public string getNonHost(int num)
    {
        if (num == 1)
        {
            if (specialAction == 3)
            {
                return "Enourage";
            }
            else if (specialAction == 4)
            {
                return "Complement";
            }
            else if (specialAction == 5)
            {
                return "Appreciate";
            }
            else if (specialAction == 6)
            {
                return "Revive";
            }
            else
            {
                return "";
            }

        }
        else
        {
            if (specialAction == 3)
            {
                return "Spend a Turn to Enourage another Robot. Increases Attack and Sentience by Love.";
            }
            else if (specialAction == 4)
            {
                return "Spend a Turn to Complement another Robot. Increases Love and Sentience by Love.";
            }
            else if (specialAction == 5)
            {
                return "Spend a Turn to Appreciate another Robot. Increase Health or sentience by Love.";
            }
            else if (specialAction == 6)
            {
                return "Spend a Turn to reactivate a deactivated Robot or increase sentience by Love.";
            }
            else
            {
                return "";
            }
        }
    } 
}
