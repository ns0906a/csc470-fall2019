using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScriptPlayer1 : MonoBehaviour
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
        transform.rotation = new Quaternion(180f,0f,0f,0f);
        rb.useGravity = false;
        sc.isTrigger = true;
        setForce();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKey(KeyCode.Space))
        {
            setForce();
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(10, 0f, 0f));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(-10, 0f, 0f));
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0f,moveSpeed * Time.deltaTime,0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0f, -moveSpeed * Time.deltaTime, 0f);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            shot.Play();
            rb.useGravity = true;
            sc.isTrigger = false;
            rb.AddForce(transform.forward * build);
            build = 0f;
            Destroy(bar);
            Instantiate(ball, transform.position, Quaternion.identity);
            Destroy(GetComponent<LaunchScriptPlayer1>());
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

    void onTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player2"))
        {
            sc.isTrigger = false;
        }
        else
        {
            sc.isTrigger = true;
        }
    }
}
