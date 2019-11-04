using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public MenuScript menu;
    AudioSource au;

    // Start is called before the first frame update
    void Start()
    {
        au = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointerEnter()
    {
        au.PlayOneShot(menu.buttonHover);
    }

    public void OnMouseDown()
    {
        au.PlayOneShot(menu.buttonSelect);
    }
}
