using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalityScript : MonoBehaviour
{

    public AudioClip[] greetings;
    public AudioClip[] selected;
    public AudioClip[] movement;
    public AudioClip[] attack;
    public AudioClip[] support;
    public AudioClip[] helpEnemy;
    public AudioClip[] damaged;
    public AudioClip[] deactivate;
    public AudioClip[] activate;
    public AudioClip[] idle;

    public Sprite[] possibleFaces;

    public Sprite[] restFace;

    public Sprite o_o;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip getClip(string group)
    {

        if(group.Equals("greet"))
        {
            return greetings[Random.Range(0, greetings.Length)];
        }
        if (group.Equals("select"))
        {
            return selected[Random.Range(0, selected.Length)];
        }
        if (group.Equals("move"))
        {
            return movement[Random.Range(0, movement.Length)];
        }
        if (group.Equals("attack"))
        {
            return attack[Random.Range(0, attack.Length)];
        }
        if (group.Equals("support"))
        {
            return support[Random.Range(0, support.Length)];
        }
        if (group.Equals("help"))
        {
            return helpEnemy[Random.Range(0, helpEnemy.Length)];
        }
        if (group.Equals("damage"))
        {
            return damaged[Random.Range(0, damaged.Length)];
        }
        if (group.Equals("activate"))
        {
            return activate[Random.Range(0, activate.Length)];
        }
        return idle[Random.Range(0, idle.Length)];
    }
}
