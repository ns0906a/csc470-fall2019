using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string Name;

    public bool selected = false;
    bool hover = false;

    Color defaultColor;
    public Color hoverColor;
    public Color SelectedCover;

    public Vector3 destination;

    public Renderer re;

    CharacterController cc;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = re.material.color;
        GameObject gmObj = GameObject.Find("GameManager");
        gm = gmObj.GetComponent<GameManager>();

        cc = GetComponent<CharacterController>();

        destination = transform.position;

        SetCorrectColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (destination != null && Vector3.Distance(destination, transform.position) > 0.5f)
        {
            destination.y = transform.position.y;
            Vector3 vecToDist = (destination - transform.position).normalized;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, vecToDist, 3 * Time.deltaTime, 1 * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDir); 


            cc.Move(transform.forward * 5 * Time.deltaTime);
        }
    }

    private void OnMouseOver()
    {
        hover = true;
        SetCorrectColor();
    }

    private void OnMouseExit()
    {
        hover = false;
        SetCorrectColor();
    }

    private void OnMouseDown()
    {
        selected = !selected;
        if(selected)
        {
            gm.selectUnit(this);
        }
        else
        {
            gm.selectUnit(null);
        }
        SetCorrectColor();
    }

    public void SetCorrectColor()
    {
        if(selected)
        {
            re.material.color = SelectedCover;
        } else if (hover)
        {
            re.material.color = hoverColor;
        }
        else
        {
            re.material.color = defaultColor;
        }
    }
}
