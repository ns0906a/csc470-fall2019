using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public GameObject burger;
    public float rotSpeed = 25;
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rotSpeed = speed * 10;
    }

    // Update is called once per frame
    void Update()
    {

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        transform.Rotate(0f, rotSpeed * Time.deltaTime * hAxis, 0f);
        transform.position += transform.forward * speed * vAxis * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 pos = transform.position + Vector3.up * 1.25f + transform.forward * 1.5f;

            GameObject shot = Instantiate(burger,pos,transform.rotation);
            Destroy(shot, 3);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dog"))
        {
            Destroy(gameObject);
        }
    }
}
