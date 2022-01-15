using _Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : Singleton<GridManager> {

    private Level _currentLevel;

    private List<List<Node>> grid = new List<List<Node>>();
    //private List<GameObject> usedCells;

    private List<Node> _enemies = new List<Node>();

    public Transform cellParent;

    public GameObject emptyPrefab;
    public GameObject fillPrefab;

    public List<GameObject> enemyPrefabs;


    public List<GameObject> percentageTexts;

    public GameObject playerPrefab;

    private Node _startingNode;

    private bool _isComplete = false;


    public bool SelectCell(int x, int z) {
        if (!IsCellEmpty(x, z))
            return false;

        Destroy(grid[x][z]);
        Node cell = Instantiate(playerPrefab, new Vector3(x, 0, -z), Quaternion.identity, cellParent).GetComponent<Node>();
        grid[x][z] = cell;
        _startingNode = cell;


        return true;
    }


    public void GridSetup(Level level) {
        _isComplete = false;
        _currentLevel = level;
        var arrayEnum = _currentLevel.arrayEnum;
        int gridRow = arrayEnum.GridSize.x;
        int gridColumn = arrayEnum.GridSize.y;

        for (int i = 0; i < gridRow; i++) {
            List<Node> temp = new List<Node>();
            for (int j = 0; j < gridColumn; j++) {
                var type = arrayEnum.GetCell(i, j);
                var cell = AddCell(type, i, j);
                if (type == GridType.Enemy1 || type == GridType.Enemy2 || type == GridType.Enemy3)
                    _enemies.Add(cell);
                temp.Add(cell);
            }
            grid.Add(temp);
        }
        Debug.Log($"Grid Size: {gridRow}x{gridColumn}");
        Camera.main.transform.position = new Vector3(((float)gridRow - 1) / 2, Mathf.Max(gridRow, (float)gridColumn * 0.75f), -1f * (((float)gridColumn - 1) / 2 - 0.5f));

    }

    public void StartRun() {
        Debug.Log($"Starting at: {_startingNode.transform.position.x}, {_startingNode.transform.position.z}");

        _startingNode.StartRoll();

        foreach (Node enemy in _enemies)
            enemy.StartRoll();
    }

    public void CleanGrid() {
        foreach (List<Node> cellList in grid) {
            cellList.Clear();
        }
        grid.Clear();
        _enemies.Clear();
        cellParent.DestroyChildren();
    }

    private Node AddCell(GridType type, int i, int j) {
        Node cell;
        if (type == GridType.Empty) { // If cell is empty at x, z
            cell = Instantiate(emptyPrefab, new Vector3(i, -0.35f, -j), Quaternion.identity, cellParent).GetComponent<Node>();
        } else { // If it's not empty
            if (type == GridType.Filled) { // If cell is filled with walls at x, z
                cell = Instantiate(fillPrefab, new Vector3(i, -0.45f, -j), Quaternion.identity, cellParent).GetComponent<Node>();
                //grid[i].Add(cell);
            } else if (type == GridType.Player) { // If cell is player at x, z
                cell = Instantiate(playerPrefab, new Vector3(i, 0, -j), Quaternion.identity, cellParent).GetComponent<Node>();
            } else { // Last call, if cell is an enemy at x, z
                if (type == GridType.Enemy1) { // It's Enemy1
                    cell = Instantiate(enemyPrefabs[0], new Vector3(i, 0, -j), Quaternion.identity, cellParent).GetComponent<Node>();
                } else if (type == GridType.Enemy2) { // It's Enemy2
                    cell = Instantiate(enemyPrefabs[1], new Vector3(i, 0, -j), Quaternion.identity, cellParent).GetComponent<Node>();
                } else { // It's Enemy3
                    cell = Instantiate(enemyPrefabs[2], new Vector3(i, 0, -j), Quaternion.identity, cellParent).GetComponent<Node>();
                }
            }

        }

        return cell;
    }

    public Transform AddNewCell(GridType type, int i, int j, int gridI, int gridJ) {

        Node cell;

        if (type == GridType.Player) { // If cell is player at x, z
            cell = Instantiate(playerPrefab, new Vector3(i, 0, j), Quaternion.identity, cellParent).GetComponent<Node>();
        } else { // Last call, if cell is an enemy at x, z
            if (type == GridType.Enemy1) { // It's Enemy1
                cell = Instantiate(enemyPrefabs[0], new Vector3(i, 0, j), Quaternion.identity, cellParent).GetComponent<Node>();
            } else if (type == GridType.Enemy2) { // It's Enemy2
                cell = Instantiate(enemyPrefabs[1], new Vector3(i, 0, j), Quaternion.identity, cellParent).GetComponent<Node>();
            } else { // It's Enemy3
                cell = Instantiate(enemyPrefabs[2], new Vector3(i, 0, j), Quaternion.identity, cellParent).GetComponent<Node>();
            }
        }

        grid[gridI][-gridJ] = cell;

        return cell.transform;
    }

    public bool IsCellEmpty(int x, int y) {

        if (x < 0 || x >= grid.Count || y < 0 || y >= grid[0].Count) {
            return false;
        } else if (grid[x][y] == null) {
            return false;
        } else if (grid[x][y].nodeType == GridType.Empty) {
            return true;
        } else {
            return false;
        }
    }

    public void CheckComplete() {

        bool check = true;

        int player = 0;
        int enemy1 = 0;
        int enemy2 = 0;

        foreach (List<Node> cellList in grid) {
            foreach (Node cell in cellList) {
                if (cell?.nodeType == GridType.Empty) {
                    check = false;
                }
            }
        }


        if (!check) return;


        foreach (List<Node> cellList in grid) {
            foreach (Node cell in cellList) {
                if (cell?.nodeType == GridType.Enemy1) {
                    enemy1++;
                } else if (cell?.nodeType == GridType.Enemy2) {
                    enemy2++;
                } else if (cell?.nodeType == GridType.Player) {
                    player++;
                }
            }
        }
        bool lose = false;
        {
            int total = player + enemy1 + enemy2;
            player = (int)((float)player / total * 100f);
            enemy1 = (int)((float)enemy1 / total * 100f);
            enemy2 = (int)((float)enemy2 / total * 100f);
            if (player != 0)
                if (percentageTexts[0] == null)
                    GameObject.Find("PlayerPercentage").GetComponent<Text>().text = $"{player}%";
            else
            percentageTexts[0].GetComponent<Text>().text = $"{player}%";
            if (enemy1 != 0)
                if (percentageTexts[1] == null)
                    GameObject.Find("EnemyPercentage").GetComponent<Text>().text = $"{enemy1}%";
                else
                    percentageTexts[1].GetComponent<Text>().text = $"{enemy1}%";
            if (enemy2 != 0)
                if (percentageTexts[2] == null)
                    GameObject.Find("Enemy2Percentage").GetComponent<Text>().text = $"{enemy2}%";
                else
                    percentageTexts[2].GetComponent<Text>().text = $"{enemy2}%";

            if (Mathf.Max(player,enemy1,enemy2) != player) {
                lose = true; 
            }
        }
        GameObject.Find("GameCanvas")?.SetActive(false);
        StartCoroutine(Check(lose));

    }

    IEnumerator Check(bool lost) {
        yield return new WaitForSeconds(2f);

        percentageTexts[0].GetComponent<Text>().text = "";
        percentageTexts[1].GetComponent<Text>().text = "";
        percentageTexts[2].GetComponent<Text>().text = "";

        if (lost)
            GameManager.Instance.ChangeState(GameManager.GameState.Lose);
        else
            GameManager.Instance.ChangeState(GameManager.GameState.Win);

    }

}