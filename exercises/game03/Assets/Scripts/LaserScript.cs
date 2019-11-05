using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public int Damage;
    public GameObject owner;
    public bool die;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Alive");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner)
        {
            return;
        }
        if (other.CompareTag("Bot"))
        {
            if (other.GetComponent<UnitScript>().Health <= 0)
            {
                other.GetComponent<UnitScript>().Health = 0;
            }
            else
            {
                other.GetComponent<UnitScript>().Health -= Damage;
            }
            die = true;
            if (other.GetComponent<UnitScript>().Health > 0)
            {
                other.GetComponent<UnitScript>().aud.PlayOneShot(other.GetComponent<UnitScript>().pers.getClip("damage"));
            }
            
           // Destroy(this.gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<UnitScript>().Health -= Damage;
            die = true;
           // Destroy(this.gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            die = true;
        }
        
    }
}
