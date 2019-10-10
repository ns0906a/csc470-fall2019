using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public CellScript[,] grid;
    public GameObject player;
	public GameObject cellPrefab;
    public GameObject boxobj;
    public InputField box;
    public GameObject shapes;
    public GameObject enemy;
    public GameObject replay;
    public GameObject menu;
    public GameObject map;
    public GameObject miniMap;
    public GameObject options;
    public GameObject howto;
    public Image mapCellPrefab;
    public Text main;
    public Text gridtxt;
    public Text pointtxt;
    public Text error;
    public AudioClip enemyBirth;

    public int points = 0;

    AudioSource au;

	bool simulate = true;

	int gridWidth;
	int gridHeight;
    int time = 0;

    float cellDimension = 3.3f;
	float cellSpacing = 0.2f;
    float mapDimension = 90f;
    float spawnTimerSet = 5f;
    float spawnTimer = 0;
    

	float generationRate = 1f;
	float generationTimer;

	// Start is called before the first frame update
	void Start()
	{
        au = GetComponent<AudioSource>();
        player.SetActive(false);
        gridtxt.text = "";
        pointtxt.text = "";
        error.text = "";
        menu.SetActive(false);
        replay.SetActive(false);
        miniMap.SetActive(false);
        toggleSimulate(false);
        options.SetActive(false);
    }

    public void test()
    {
        int value;
        if(int.TryParse(box.text,out value))
        {
            Begin(value);
        }
        else
        {
            error.text = "Please enter integers only";
        }
    }

    private void Begin(int value)
    {
        points = 0;
        spawnTimerSet = 5;
        spawnTimer = spawnTimerSet;
        toggleSimulate(true);
        boxobj.SetActive(false);
        shapes.SetActive(false);
        main.text = "";
        player.SetActive(false);
        gridtxt.text = "";
        pointtxt.text = "";
        menu.SetActive(false);
        replay.SetActive(false);
        miniMap.SetActive(true);
        options.SetActive(false);
        howto.SetActive(false);
        gridWidth = value;
        gridHeight = value;
        grid = new CellScript[gridWidth, gridHeight];

        //Using nested for loops, instantiate cubes with cell scripts in a way such that
        //	each cell will be places in a top left oriented coodinate system.
        //	I.e. the top left cell will have the x, y coordinates of (0,0), and the bottom right will
        //	have the coodinate (gridWidth-1, gridHeight-1)
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                //Create a cube, position/scale it, add the CellScript to it.
                Vector3 pos = new Vector3(x * (cellDimension + cellSpacing) + cellDimension / 2,
                                            0,
                                            y * (cellDimension + cellSpacing) + cellDimension / 2);
                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity);
                CellScript cs = cellObj.AddComponent<CellScript>();
                cs.x = x;
                cs.y = y;
                cs.alive = false; //(Random.value > 0.5f) ? true : false;
                

                cellObj.transform.position = pos;
                cellObj.transform.localScale = new Vector3(cellDimension,
                                                                cellDimension,
                                                                cellDimension);

                float mapCellDimension = cellDimension * mapDimension;
                float mapCellSpacing = cellSpacing * mapDimension;

                Image mapCell = Instantiate(mapCellPrefab);

                mapCell.transform.localPosition = new Vector3(x * (mapCellDimension + mapCellSpacing), y * (mapCellDimension + (mapCellSpacing)));
                mapCell.transform.localScale = new Vector3(cellDimension, cellDimension, 0);
                mapCell.transform.SetParent(map.transform);
                cs.map = mapCell;
                cs.updateColor();

                //Finally add the cell to it's place in the two dimensional array
                grid[x, y] = cs;
            }
        }
        //Initialize the timer
        generationTimer = generationRate;
        float playerStartx = (gridWidth * (cellDimension + cellSpacing)) / 2;
        float playerStarty = (gridHeight * (cellDimension + cellSpacing)) / 2;
        player.GetComponent<PlayerController>().setUp(playerStartx, playerStarty);
        map.transform.localScale /= gridWidth;
    }


	private void Update()
	{
		generationTimer -= Time.deltaTime;
		if (generationTimer < 0 && simulate) {
			//generate next state
			generate();

			//reset the timer
			generationTimer = generationRate;
		}

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && getSimulate())
        {
            spawnEnemy();
            spawnTimerSet *= 0.95f;
            spawnTimer = spawnTimerSet;
        }
	}

	void generate()
	{
		//This isn't really being used, but why not have the simulation know how
		//many times it has "generated" new states?
		time++;

		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				List<CellScript> liveNeighbors = gatherLiveNeighbors(x, y);
				//Apply the 4 rules from Conway's Gaem of Life (https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life)
				//1. Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
				if (grid[x, y].alive && liveNeighbors.Count < 2) {
					grid[x, y].nextAlive = false;
				}
				//2. Any live cell with two or three live neighbours lives on to the next generation.
				else if (grid[x, y].alive && (liveNeighbors.Count == 2 || liveNeighbors.Count == 3)) {
					grid[x, y].nextAlive = true;
                }
				//3. Any live cell with more than three live neighbours dies, as if by overpopulation.
				else if (grid[x, y].alive && liveNeighbors.Count > 3) {
					grid[x, y].nextAlive = false;
                }
				//4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
				else if (!grid[x, y].alive && liveNeighbors.Count == 3) {
					grid[x, y].nextAlive = true;
                }
			}
		}
		//Now that we have looped through all of the cells in the grid, and stored what their alive status should
		//	be in each cell's nextAlivevariable, update each cell's alive state to be that value.
		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				grid[x, y].alive = grid[x, y].nextAlive;
			}
		}
	}

	//This function returns all the live neighbors
	List<CellScript> gatherLiveNeighbors(int x, int y)
	{
		List<CellScript> neighbors = new List<CellScript>();
		//Spend some time thinking about how this considers all surrounding cells in grid[x,y]
		//why now indexing bad values of grid.
		for (int i = Mathf.Max(0, x - 1); i <= Mathf.Min(gridWidth - 1, x + 1); i++) {
			for (int j = Mathf.Max(0, y - 1); j <= Mathf.Min(gridHeight - 1, y + 1); j++) {
				//Add all live neighbors of (x, y) excluding itself
				if (grid[i,j].alive && !(i == x && j == y)) {
					neighbors.Add(grid[i, j]);
				}
			}
		}
		return neighbors;
	}

	//This function is called by the UI toggle's event system (look at the ToggleSimulateButton
	//child of the Canvas)
	public void toggleSimulate(bool value)
	{
		simulate = value;
	}

    public bool getSimulate()
    {
        return simulate;
    }

    public void gameOver()
    {
        toggleSimulate(false);
        deleteGrid();
        main.text = "Game Over";
        gridtxt.text = "Grid: " + gridWidth;
        pointtxt.text = "Points: " + points;
        menu.SetActive(true);
        replay.SetActive(true);
    }

    public void deleteGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].alive = true;
                grid[x, y].updateColor();
                grid[x, y].updateSize();
            }
        }
    }

    public void spawnEnemy()
    {
        int RandX = Random.Range(1, gridWidth);
        int RandY = Random.Range(1, gridHeight);
        CellScript spawn = grid[RandX, RandY];

        while(spawn.alive)
        {
            RandX = Random.Range(1, gridWidth);
            RandY = Random.Range(1, gridHeight);
            spawn = grid[RandX, RandY];
            if (!spawn.alive)
            {
                break;
            }
        }
        Vector3 spawnPos = spawn.transform.position;
        spawnPos.y = 5;
        Instantiate(enemy, spawnPos, Quaternion.identity);
        au.PlayOneShot(enemyBirth);
    }

    public void Replay()
    {
        Begin(gridWidth);
    }

    public void Menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void toggleOptions()
    {
        options.SetActive(!options.activeSelf);
    }
}