using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScriptPlayer2 : MonoBehaviour
{

    private Rigidbody rb;
    private float build = 0f;
    private SphereCollider sc;
    private AudioSource shot;

    public Text forceText;
    public float chargeRate = 1000;
    public GameObject bar;
    public GameObject ball;
    public float moveSpeed = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        shot = GetComponent<AudioSource>();
        build = 0;
        doForceText();
        rb.useGravity = false;
        sc.isTrigger = true;
        setForce();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.RightControl))
        {
            setForce();
        }

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(new Vector3(10, 0f, 0f));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(new Vector3(-10, 0f, 0f));
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0f,moveSpeed * Time.deltaTime,0f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0f, -moveSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKeyUp(KeyCode.RightControl))
        {
            shot.Play();
            rb.useGravity = true;
            sc.isTrigger = false;
            rb.AddForce(transform.forward * build);
            build = 0f;
            Destroy(bar);
            Instantiate(ball, transform.position, Quaternion.identity);
            Destroy(GetComponent<LaunchScriptPlayer2>());
        }
    }

    void doForceText()
    {
        forceText.text = "Force: " + build.ToString("0.00");
    }

    void setForce()
    {
        build += chargeRate * Time.deltaTime;
        doForceText();
        bar.transform.localScale = new Vector3(0.02f, 1f, build / 5000);
        bar.transform.localPosition = new Vector3(0f, 0f, (build / 5000) * 5);
    }
}
