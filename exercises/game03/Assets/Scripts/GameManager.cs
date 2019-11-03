using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    UnitScript selectedUnit;

    //UI
    public GameObject bot;
    public GameObject selectedPanel;
    public ToggleGroup actionSelectToggleGroup;
    public GameObject faceCam;
    public GameObject block;
    public Text HealthStat;
    public Text AttackStat;
    public Text LoveStat;
    public Text SpeedStat;
    public Text descTitle;
    public Text desc;
    public Text turnText;
    public GameObject deactivated;
    public Button endTurnButton;

    public GameObject talkBox;

    GameObject camTarg;
    public Text talkText;

    public Text TextName;
    public bool playerTurn;
    float camSpeed = 15;
    float scrollSpeed = 25;
    float screenScrollSpeed = 0.5f;
    float zoomSpeed = 200;
    bool viable = false;
    bool doAI = false;
    bool cameraPan = false;
    public GameObject selector;
    LineRenderer line;
    GameObject lineBox;
    string ActionName;
    Vector3 mouseSet;

    public string[] nameDatabase;
    public GameObject[] personalities;




    // Start is called before the first frame update
    void Start()
    {
        line = selector.GetComponent<LineRenderer>();
        camTarg = GameObject.Find("CameraTarget");
        mouseSet = Input.mousePosition;
        viable = false;
        spawnEnemies(15);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && actionsDone())
        {
            endTurn();
        }

        if (!NullTeamAlive())
        {
            Debug.Log("You Lose");
        }

        if (actionsDone() && playerTurn)
        {
            endTurnButton.interactable = true;
        }
        else
        {
            endTurnButton.interactable = false;
        }

        if (!playerTurn)
        {
            if (doAI)
            {
                foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    bot.GetComponent<UnitScript>().activated = true;
                }
                doAI = false;
            }
            if(aiTurnDone())
            {
                endTurn();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedUnit != null)
            {
                selectUnit(null);
            }
        }

        cameraControls();
        if (selectedUnit != null)
        {
            IEnumerable<Toggle> activeToggles = actionSelectToggleGroup.ActiveToggles();
            ActionName = "-";
            foreach (Toggle t in activeToggles)
            {
                ActionName = t.name;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!ActionName.Equals("-"))
            {
                if (ActionName.Equals("Move"))
                {
                    descTitle.text = "Move";
                    desc.text = "Take a turn to move Bot to a new position. How far a Bot can move depends on their Speed.";
                }
                if (ActionName.Equals("Hostile"))
                {
                    descTitle.text = "Shoot a Laser";
                    desc.text = "Take a turn to fire a laser. Laser deals damage to robot they take damage based on your Robot's Attack.";
                }
                if (ActionName.Equals("Non-Hostile"))
                {
                    descTitle.text = selectedUnit.getNonHost(1);
                    desc.text = selectedUnit.getNonHost(0);
                }
                if (ActionName.Equals("-"))
                {

                }
                if (!selector.activeSelf)
                {
                    selector.SetActive(true);
                }
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ground")))
                {
                    if (ActionName.Equals("Move"))
                    {
                        Vector3 selPos = hit.point;
                        selPos.y = 0.1f;
                        float dist = Vector3.Distance(selPos, selectedUnit.transform.position);
                        Color col;
                        if (dist > selectedUnit.MovementSpeed * 3 || selector.GetComponent<SelectorScript>().hasTarget)
                        {
                            viable = false;
                            col = Color.red;
                            col.a = 0.5f;
                        }
                        else
                        {
                            viable = true;
                            col = Color.green;
                            col.a = 0.5f;
                        }
                        selector.GetComponent<SpriteRenderer>().color = col;
                        line.material.color = col;
                        line.material.color = col;
                        selector.transform.position = selPos;
                        line.SetPosition(0, selectedUnit.transform.position);
                        line.SetPosition(1, selector.transform.position);
                    }
                    if (ActionName.Equals("Hostile"))
                    {
                        Vector3 selPos = hit.point;
                        selPos.y = 0.1f;
                        float dist = Vector3.Distance(selPos, selectedUnit.transform.position);
                        Color col;
                        if (dist > selectedUnit.MovementSpeed * 3)
                        {
                            viable = false;
                            col = Color.red;
                            col.a = 0.5f;
                        }
                        else
                        {
                            viable = true;
                            col = Color.white;
                            col.a = 0.5f;
                        }
                        selector.GetComponent<SpriteRenderer>().color = col;
                        line.material.color = col;
                        line.material.color = col;
                        selector.transform.position = selPos;
                        line.SetPosition(0, selectedUnit.transform.position);
                        line.SetPosition(1, selector.transform.position);
                    }
                    if (ActionName.Equals("Non-Hostile"))
                    {
                        Vector3 selPos = hit.point;
                        selPos.y = 0.1f;
                        float dist = Vector3.Distance(selPos, selectedUnit.transform.position);
                        Color col;
                        if (dist > selectedUnit.MovementSpeed * 3 || !selector.GetComponent<SelectorScript>().hasTarget || (selector.GetComponent<SelectorScript>().target.GetComponent<UnitScript>().deactivated && selectedUnit.specialAction != 6) || selector.GetComponent<SelectorScript>().target == selectedUnit)
                        {
                            viable = false;
                            col = Color.red;
                            col.a = 0.5f;
                        }
                        else
                        {
                            viable = true;
                            col = selectedUnit.playerColor;
                            col.a = 0.5f;
                        }
                        selector.GetComponent<SpriteRenderer>().color = col;
                        line.material.color = col;
                        line.material.color = col;
                        selector.transform.position = selPos;
                        line.SetPosition(0, selectedUnit.transform.position);
                        line.SetPosition(1, selector.transform.position);
                    }
                }
                else
                {
                    if (selector.activeSelf)
                    {
                        selector.SetActive(false);
                    }
                }
            }
            else
            {
                if (selector.activeSelf)
                {
                    selector.SetActive(false);
                }
                descTitle.text = "Action Description";
                desc.text = "Select an action from the list on the right for your bot to perform. What the action does will be listed here.";
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (ActionSelect())
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ground")))
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                        {
                            if (viable)
                            {
                                if (ActionName.Equals("Move"))
                                {
                                    if (selectedUnit != null)
                                    {
                                        selectedUnit.aud.PlayOneShot(selectedUnit.pers.getClip("move"));
                                        selectedUnit.doneTurn = true;
                                        updateUI();
                                        selectedUnit.action = 1;
                                        selectedUnit.destination = hit.point;

                                    }
                                }

                                if (ActionName.Equals("Hostile"))
                                {
                                    if (selectedUnit != null)
                                    {
                                        selectedUnit.aud.PlayOneShot(selectedUnit.pers.getClip("attack"));
                                        selectedUnit.doneTurn = true;
                                        updateUI();
                                        selectedUnit.action = 2;
                                        selectedUnit.destination = hit.point;

                                    }
                                }

                                if (ActionName.Equals("Non-Hostile"))
                                {
                                    if (selectedUnit != null)
                                    {

                                        selectedUnit.doneTurn = true;
                                        updateUI();
                                        selectedUnit.action = selectedUnit.specialAction;
                                        selectedUnit.destination = hit.point;
                                        selectedUnit.target = selector.GetComponent<SelectorScript>().target;
                                    }
                                }
                            }
                            if (ActionName.Equals("-"))
                            {
                                if (selectedUnit != null)
                                {
                                    selectedUnit.selected = false;
                                    selectedUnit.SetCorrectColor();
                                    selectedUnit = null;
                                    updateUI();
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("ground"))
                        {
                            if (selectedUnit != null)
                            {
                                selectedUnit.selected = false;
                                selectedUnit.SetCorrectColor();
                                selectedUnit = null;
                                updateUI();
                            }
                        }
                    }
                    else
                    {
                        if (selectedUnit != null)
                        {
                            selectedUnit.selected = false;
                            selectedUnit.SetCorrectColor();
                            selectedUnit = null;
                            updateUI();
                        }
                    }
                }
                foreach (Toggle t in activeToggles)
                {
                    t.isOn = false;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                bool selectIsOn = false;
                foreach (Toggle t in activeToggles)
                {
                    selectIsOn = t.isOn;
                }
                if (selectIsOn)
                {
                    foreach (Toggle t in activeToggles)
                    {
                        t.isOn = false;
                    }
                }
            }
        }
    }

    bool aiTurnDone()
    {
        bool value = true;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!bot.GetComponent<UnitScript>().doneTurn)
            {
                value = false;
            }
        }
        return value;
    }

    public void cameraControls()
    {
        if (selectedUnit != null)
        {
            Vector3 fcamPos = selectedUnit.transform.position + selectedUnit.transform.forward * 0.95f + Vector3.up * 1.75f;
            faceCam.transform.position = fcamPos;
            faceCam.transform.LookAt(selectedUnit.transform.position + selectedUnit.transform.up * 1.75f);
        }

        Vector3 camPos = camTarg.transform.position + camTarg.transform.forward * (-6) + Vector3.up * 3;
        Camera.main.transform.position = camPos;
        Camera.main.transform.LookAt(camTarg.transform);

        float zoom = Input.GetAxis("Zoom") * zoomSpeed * Time.deltaTime * -1;
        Camera.main.orthographicSize += zoom;
        if (Camera.main.orthographicSize >= 15)
        {
            Camera.main.orthographicSize = 15;
        }
        if (Camera.main.orthographicSize <= 3)
        {
            Camera.main.orthographicSize = 3;
        }

        if (!cameraPan)
        {
            if (Input.mousePosition.x >= Screen.width)
            {
                camTarg.transform.position += camTarg.transform.right * screenScrollSpeed;
            }
            if (Input.mousePosition.x <= 1)
            {
                camTarg.transform.position -= camTarg.transform.right * screenScrollSpeed;
            }
            if (Input.mousePosition.y >= Screen.height)
            {
                camTarg.transform.position += camTarg.transform.forward * screenScrollSpeed;
            }
            if (Input.mousePosition.y <= 1)
            {
                camTarg.transform.position -= camTarg.transform.forward * screenScrollSpeed;
            }
        }
        else
        {
            
                Vector3 destination = selectedUnit.transform.position;
                Transform targ = camTarg.transform;
                Vector3 targPos = destination + Vector3.up;
                targPos.y = 2.5f;
                Vector3 vecToDist = (targPos - targ.position).normalized;
                float dist = Vector3.Distance(targPos, targ.position);
                
                if (dist <= 1)
                {
                    //dist = 0.001f;
                    cameraPan = false;
                }
                targ.position += vecToDist * (camSpeed - camSpeed / (dist + 1)) * Time.deltaTime;
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseSet = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mousePos = Input.mousePosition;
            float scrollDist = (mouseSet.x - Input.mousePosition.x) * -1;
            mouseSet.x += scrollDist;
            camTarg.transform.Rotate(0f, scrollDist * scrollSpeed * Time.deltaTime, 0f);
        }
        
    }

    public void selectUnit (UnitScript unit)
    {
        if(selectedUnit != null && selectedUnit.action < 1)
        {
            selectedUnit.selected = false;
            selector.SetActive(false);
            selector.GetComponent<SelectorScript>().hasTarget = false;
            selector.GetComponent<SelectorScript>().target = null;
            selectedUnit.SetCorrectColor();
        }
        if (unit != null)
        {
            selectedUnit = unit;
            selectedUnit.selected = true;
            cameraPan = true;
            selectedUnit.SetCorrectColor();
        }
        else
        {
            selectedUnit.SetCorrectColor();
            selectedUnit = null;
        }
        updateUI();
    }

    bool NullTeamAlive()
    {
        bool value = false;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Bot"))
        {
            if (bot != null && !bot.GetComponent<UnitScript>().deactivated)
            {
                value = true;
            }
        }
        return value;
    }

    public void spawnEnemies(int number)
    {
        int room1 = Mathf.RoundToInt(number * 0.1f);
        int room2 = Mathf.RoundToInt(number * 0.15f);
        int room3 = Mathf.RoundToInt(number * 0.15f);
        int room4 = Mathf.RoundToInt(number * 0.2f);
        int room5 = Mathf.RoundToInt(number * 0.2f);
        int room6 = Mathf.RoundToInt(number * 0.2f);
        Debug.Log(room1 + "," + room2 + "," + room3 + "," + room4 + "," + room5 + "," + room6);
        for (int i = 0; i<room1; i++)
        {
            float randX = Random.Range(-17, 17);
            float randZ = Random.Range(-17, 17);
            GameObject newBot = bot;
            UnitScript setUp = newBot.GetComponent<UnitScript>();
            setUp.sentient = false;
            setUp.Sentience = Random.Range(4, 6);
            setUp.Health = Random.Range(3, 5);
            setUp.Attack = Random.Range(1, 3);
            setUp.MovementSpeed = Random.Range(2, 3);
            setUp.playerColor = Color.black;
            Instantiate(newBot, new Vector3(randX, 0.2f, randZ),Quaternion.identity);
        }
        for (int i = 0; i < room2; i++)
        {
            float randX = Random.Range(-17, 17);
            float randZ = Random.Range(22, 58);
            GameObject newBot = bot;
            UnitScript setUp = newBot.GetComponent<UnitScript>();
            setUp.sentient = false;
            setUp.Sentience = Random.Range(4, 7);
            setUp.Health = Random.Range(3, 6);
            setUp.Attack = Random.Range(2, 3);
            setUp.MovementSpeed = Random.Range(2, 3);
            setUp.playerColor = Color.black;
            Instantiate(newBot, new Vector3(randX, 0.2f, randZ), Quaternion.identity);
        }
        for (int i = 0; i < room3; i++)
        {
            float randX = Random.Range(-17, 17);
            float randZ = Random.Range(-22, -58);
            GameObject newBot = bot;
            UnitScript setUp = newBot.GetComponent<UnitScript>();
            setUp.sentient = false;
            setUp.Sentience = Random.Range(4, 7);
            setUp.Health = Random.Range(3, 6);
            setUp.Attack = Random.Range(2, 3);
            setUp.MovementSpeed = Random.Range(2, 3);
            setUp.playerColor = Color.black;
            Instantiate(newBot, new Vector3(randX, 0.2f, randZ), Quaternion.identity);
        }
        for (int i = 0; i < room4; i++)
        {
            float randX = Random.Range(-58, -22);
            float randZ = Random.Range(22, 58);
            GameObject newBot = bot;
            UnitScript setUp = newBot.GetComponent<UnitScript>();
            setUp.sentient = false;
            setUp.Sentience = Random.Range(6, 9);
            setUp.Health = Random.Range(4, 7);
            setUp.Attack = Random.Range(3, 4);
            setUp.MovementSpeed = Random.Range(3, 4);
            setUp.playerColor = Color.black;
            Instantiate(newBot, new Vector3(randX, 0.2f, randZ), Quaternion.identity);
        }
        for (int i = 0; i < room5; i++)
        {
            float randX = Random.Range(-58, -22);
            float randZ = Random.Range(-22, -58);
            GameObject newBot = bot;
            UnitScript setUp = newBot.GetComponent<UnitScript>();
            setUp.sentient = false;
            setUp.Sentience = Random.Range(6, 9);
            setUp.Health = Random.Range(4, 7);
            setUp.Attack = Random.Range(3, 4);
            setUp.MovementSpeed = Random.Range(3, 4);
            setUp.playerColor = Color.black;
            Instantiate(newBot, new Vector3(randX, 0.2f, randZ), Quaternion.identity);
        }
        for (int i = 0; i < room6; i++)
        {
            float randX = Random.Range(-58, -22);
            float randZ = Random.Range(-17, 17);
            GameObject newBot = bot;
            UnitScript setUp = newBot.GetComponent<UnitScript>();
            setUp.sentient = false;
            setUp.Sentience = Random.Range(7, 10);
            setUp.Health = Random.Range(5, 8);
            setUp.Attack = Random.Range(4, 6);
            setUp.MovementSpeed = Random.Range(3, 4);
            setUp.playerColor = Color.black;
            Instantiate(newBot, new Vector3(randX, 0.2f, randZ), Quaternion.identity);
        }
    }

    public void updateUI()
    {
        if (selectedUnit != null)
        {
            HealthStat.text = selectedUnit.Health.ToString();
            AttackStat.text = selectedUnit.Attack.ToString();
            LoveStat.text = selectedUnit.Love.ToString();
            SpeedStat.text = selectedUnit.MovementSpeed.ToString();

            Color col = selectedUnit.playerColor;
            col.a = 1;
            foreach (Image t in selectedPanel.transform.GetChild(0).GetComponentsInChildren<Image>())
            {
                t.color = col;
            }

            if (selectedUnit.doneTurn)
            {
                block.SetActive(true);
            } else
            {
                block.SetActive(false);
            }
            if (selectedUnit.deactivated)
            {
                deactivated.SetActive(true);
                block.SetActive(true);
            }
            else
            {
                deactivated.SetActive(false);
            }
            TextName.text = selectedUnit.Name;
            selectedPanel.SetActive(true);
        }
        else
        {
            selectedPanel.SetActive(false);
            return;
        }
    }

    bool actionsDone()
    {
        bool value = true;
        foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Bot"))
        {
            if (bot.GetComponent<UnitScript>().action != 0)
            {
                value = false;
            }
        }
        return value;
    }



    public void endTurn()
    {
        playerTurn = !playerTurn;
        if (playerTurn)
        {
            endTurnButton.interactable = true;
            turnText.text = "Team Null's Turn";
            foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Bot"))
            {
                UnitScript unit = bot.GetComponent<UnitScript>();
                unit.action = 0;
                unit.doneTurn = false;
            }
        }
        else
        {
            endTurnButton.interactable = false;
            doAI = true;
            turnText.text = "Factory Bots Turn";
            foreach (GameObject bot in GameObject.FindGameObjectsWithTag("Bot"))
            {
                UnitScript unit = bot.GetComponent<UnitScript>();
                unit.action = 0;
                unit.doneTurn = true;
            }
        }
        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit.SetCorrectColor();
            selectedUnit = null;
        }
        updateUI();
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
    }

    public bool ActionSelect()
    {
        IEnumerable<Toggle> activeToggles = actionSelectToggleGroup.ActiveToggles();
        bool value = false;
        foreach (Toggle t in activeToggles)
        {
           value = t.isOn;
        }
        return value;
    }
}
