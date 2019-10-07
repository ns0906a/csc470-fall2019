using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    float lifespan = 7f; // How long the Cat lasts before being deleted
    float speed = 2.5f;  // How fast the Cat travels

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Cheese(Clone)");                                              // Find the Cheese GameObject on the Map
        gameObject.transform.LookAt(target.transform, transform.up);                            // Point towards Cheese
        transform.rotation = new Quaternion(0f,transform.rotation.y, 0f, transform.rotation.w); // Set X and Z rotation to 0 (to prevent Cat clipping through floor)
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World); // Move Cat

        if (lifespan < 0) // Delete cat once lifespan reaches below 0
        {
            Destroy(this.gameObject);
        }
    }
}
