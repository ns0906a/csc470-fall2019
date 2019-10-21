using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScriptSprite : MonoBehaviour
{

    public bool selected = false;
    bool hover = false;

    Color defaultColor;
    public Color hoverColor;
    public Color SelectedCover;

    public Vector3 destination;

    public SpriteRenderer re;

    CharacterController cc;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = re.color;
        GameObject gmObj = GameObject.Find("GameManager");
        gm = gmObj.GetComponent<GameManager>();

        cc = GetComponent<CharacterController>();

        destination = transform.position;

        SetCorrectColor();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
        if (destination != null && Vector3.Distance(destination, transform.position) > 0.5f)
        {
            
            Vector3 vecToDist = (destination - transform.position).normalized;
            float face = vecToDist.x - vecToDist.z;
            if (face > 0)
            {
                re.flipX = true;
            } else
            {
                re.flipX = false;
            }
            cc.Move(vecToDist * 5 * Time.deltaTime);
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
            //gm.selectUnit(this);
        }
        else
        {
            //gm.selectUnit(null);
        }
        SetCorrectColor();
    }

    public void SetCorrectColor()
    {
        if(selected)
        {
            re.color = SelectedCover;
        } else if (hover)
        {
            re.color = hoverColor;
        }
        else
        {
            re.color = defaultColor;
        }
    }
}
