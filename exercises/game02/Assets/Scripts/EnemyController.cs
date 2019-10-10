using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float yVel = 0f;
    CharacterController cc;
    float gravityMod = 0.03f;
    public AudioClip death;

    float moveSpeed = 5;

    AudioSource au;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        au = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = GameObject.Find("Player");
        transform.LookAt(target.transform);
        Vector3 amountToMove = transform.forward * moveSpeed * Time.deltaTime;

        yVel += Physics.gravity.y * gravityMod;
        amountToMove.y = yVel;
        cc.Move(amountToMove);

        CheckDead();
    }

    void CheckDead()
    {
        if (transform.position.y < -2)
        {
            GameManager gameCode = GameObject.Find("GameManagerObject").GetComponent<GameManager>();
            gameCode.points++;
            au.PlayOneShot(death);
            Destroy(this.gameObject);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            other.gameObject.GetComponent<PlayerController>().alive = false;
        }
        if (other.gameObject.CompareTag("cell"))
        {
            other.gameObject.GetComponent<CellScript>().map.color = Color.blue;
        }
    }
}
