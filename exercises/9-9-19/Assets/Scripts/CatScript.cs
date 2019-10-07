using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatScript : MonoBehaviour
{

    public float speed = 5;
    public GameObject treeObj;
    public Text txt;
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        txt.text = "Cat Loves Tree";
        music = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //    Vector3 vecToTree =  treeObj.transform.position - gameObject.transform.position;
        //      vecToTree = vecToTree.normalized;

        //        transform.position = transform.position + (vecToTree * speed * Time.deltaTime);
        if (treeObj != null)
        {
            gameObject.transform.LookAt(treeObj.transform, transform.up);
            transform.position = transform.position + (transform.forward * speed * Time.deltaTime);

        }
        else
        {
            
            transform.position = transform.position + (transform.forward * speed * Time.deltaTime);
        }

        if (!music.isPlaying)
        {
            transform.position = new Vector3(Random.Range(-100,100), Random.Range(-100, 100), Random.Range(-100, 100));
            gameObject.transform.LookAt(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)));
            txt.rectTransform.anchoredPosition = new Vector3(Random.Range(20f, 30f), Random.Range(-10f, -20f), 0f);
            txt.text = "MEOOOOWWWWW!!!!";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(treeObj.gameObject);
        gameObject.transform.LookAt(Camera.main.transform, transform.up);
        txt.text = "Cat Loves You";
    }
}
