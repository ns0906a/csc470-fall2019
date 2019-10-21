using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    UnitScript selectedUnit;
    public GameObject talkBox;
    public Text talkText;
    public GameObject selectedPanel;
    public Text TextName;
    public Image PotraitImage;
    public ToggleGroup actionSelectToggleGroup;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000))
            {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                {
                    if(selectedUnit != null)
                    {
                        selectedUnit.destination = hit.point;
                    }
                }
            }
            else
            {
                if (selectedUnit != null)
                {
                    selectedUnit.selected = false;
                    selectedUnit.SetCorrectColor();
                    selectUnit(null);
                    updateUI();
                }
            }
        }
    }

    public void selectUnit (UnitScript unit)
    {
        if(selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit.SetCorrectColor();
        }
        selectedUnit = unit;
        selectedUnit.selected = true;
        selectedUnit.SetCorrectColor();



        updateUI();
    }

    void updateUI()
    {
        if (selectedUnit == null)
        {
            selectedPanel.SetActive(false);
            return;
        }
        TextName.text = selectedUnit.Name;
        selectedPanel.SetActive(true);
    }

    public void TakeAction()
    {
        Vector3 pos = selectedUnit.transform.position + Vector3.up;
        pos = Camera.main.WorldToScreenPoint(pos);
        pos.y += talkBox.GetComponent<RectTransform>().rect.height / 2;
        talkBox.transform.position = pos;

        IEnumerable<Toggle> activeToggles = actionSelectToggleGroup.ActiveToggles();
        string Action = "";
        foreach (Toggle t in activeToggles)
        {
            if(t.IsActive())
            {
                Action = t.gameObject.GetComponentInChildren<Text>().text;
            }
        }

        talkText.text = Action;

        talkBox.SetActive(true);

        Debug.Log("Done");
    }
}
