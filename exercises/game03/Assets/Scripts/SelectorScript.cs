using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorScript : MonoBehaviour
{
    public bool hasTarget = false;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget && target != null)
        {
            Color col = GetComponent<SpriteRenderer>().color;
            col.a = 1f;
            GetComponent<SpriteRenderer>().color = col;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot") || other.CompareTag("Enemy"))
        {
            hasTarget = true;
            target = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bot") || other.CompareTag("Enemy"))
        {
            hasTarget = false;
            target = null;
        }
    }
}
