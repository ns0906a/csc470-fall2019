using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellScript : MonoBehaviour
{
    public bool selected = false;
    public bool prevSelected;
    public Image map;
	public bool alive = false;
	public bool nextAlive;
	bool prevAlive;
	public int x = -1;
	public int y = -1;

	Renderer renderer;

	// Start is called before the first frame update
	void Start()
    {
        prevAlive = alive;
        prevSelected = selected;
	}

    // Update is called once per frame
    void Update()
    {
        float activate1 = Input.GetAxis("Fire1");
        float activate2 = Input.GetAxis("Fire2");

        if (this.selected)
        {
            if (alive)
            {
                selected = false;
            }

            if (activate1 < 0 || activate2 > 0)
            {
                alive = true;
                selected = false;
            }
        }

		if (prevAlive != alive || prevSelected != selected) {
            updateSize();
		}

		prevAlive = alive;
        prevSelected = selected;
        updateColor();
    }

    public void updateColor()
    {
        if (renderer == null) {
            renderer = gameObject.GetComponent<Renderer>();
        }

        if (this.selected) {
            renderer.material.color = Color.green;
            map.color = Color.green;
        } else if (this.alive) { 
            renderer.material.color = Color.magenta;
            map.color = Color.magenta;
        } else {
			renderer.material.color = Color.yellow;
            map.color = Color.yellow;
        }
	}

    public void updateSize()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        BoxCollider bc = GetComponent<BoxCollider>();
        if (this.alive)
        {
            mr.enabled = false;
            bc.enabled = false;
        } else
        {
            mr.enabled = true;
            bc.enabled = true;
        }
    }
}
