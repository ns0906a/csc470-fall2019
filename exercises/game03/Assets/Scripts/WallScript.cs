using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{

    public Color hoverColor;
    public Color defaultColor;

    bool hover = false;

    Renderer re;

    // Start is called before the first frame update
    void Start()
    {
        re = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetCorrectColor()
    {
        if (hover)
        {
            re.material.color = hoverColor;
        }
        else
        {
            re.material.color = defaultColor;
        }
    }
}
