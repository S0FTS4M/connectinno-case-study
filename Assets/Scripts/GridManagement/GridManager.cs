// GridManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GridManager : MonoBehaviour
{
    public Transform _gridParent;

    private Tile[,] _grid;

    private TilePool _tilePool;

    private Tile.Settings _tileSettings;

    private GridLayoutGroup _gridLayoutGroup;

    private LevelData _currentLevel;

    private ILevelManager _levelManager;

    private int tryCount = 100;

    private PlayerGoalsManager _goalsManager;


    [Inject]
    public void Construct(ILevelManager levelManager, TilePool tilePool, Tile.Settings tileSettings, PlayerGoalsManager goalsManager)
    {
        _tilePool = tilePool;
        _tileSettings = tileSettings;
        _levelManager = levelManager;
        _goalsManager = goalsManager;

        _gridLayoutGroup = _gridParent.GetComponent<GridLayoutGroup>();

        levelManager.LevelLoaded += OnLevelLoaded;

        levelManager.LevelCompleted += OnLevelCompleted;
        levelManager.LevelFailed += OnLevelFailed;

        levelManager.PlayerMadeMove += OnPlayerMadeMove;
    }

    private void GenerateNewGrid(LevelData level)
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = level.column;

        _grid = new Tile[level.row, level.column];


        for (int i = 0; i < level.row; i++)
        {
            for (int j = 0; j < level.column; j++)
            {
                if (_grid[i, j] != null)
                    continue;

                _grid[i, j] = _tilePool.Spawn();
                _grid[i, j].transform.SetParent(_gridParent);

                _grid[i, j].row = i;
                _grid[i, j].col = j;
            }
        }

        FillGridWithTargetObjectives(level.targetObjectives);
        FillEmptyTilesWithRandomItems();
    }

    private void FillGridWithTargetObjectives(List<TargetObjective> targetObjectives)
    {
        var maxCellCount = _grid.GetLength(0) * _grid.GetLength(1);
        var targetObjectiveCount = 0;

        foreach (var objective in targetObjectives)
        {
            var remainingObjectiveCount = 3 - (objective.count % 3 == 0 ? 3 : objective.count % 3);
            targetObjectiveCount += objective.count + remainingObjectiveCount;

        }

        if (maxCellCount < targetObjectiveCount)
        {
            Debug.LogError("Not enough cells to fill all target objectives consider increasing the grid size or decreasing the number of target objectives!!!");
            return;
        }

        var tryCount = 100;

        foreach (var objective in targetObjectives)
        {
            var itemData = _tileSettings.GetItemData(objective.name);

            //NOTE: I am trying to make sure there are at least 3 items or multiples of 3 in each objective in order to make the game more playable
            var remainingObjectiveCount = 3 - (objective.count % 3 == 0 ? 3 : objective.count % 3);

            for (int i = 0; i < objective.count + remainingObjectiveCount; i++)
            {
                var rndX = UnityEngine.Random.Range(0, _grid.GetLength(0));
                var rndY = UnityEngine.Random.Range(0, _grid.GetLength(1));
                while (_grid[rndX, rndY].ItemData != null && tryCount > 0)
                {
                    rndX = UnityEngine.Random.Range(0, _grid.GetLength(0));
                    rndY = UnityEngine.Random.Range(0, _grid.GetLength(1));
                    tryCount--;
                }

                _grid[rndX, rndY].SetItemData(itemData);
                _grid[rndX, rndY].ShowTile();
            }
        }
    }

    private bool BFSHasValidMatch(int row, int col)
    {
        if (_grid[row, col].IsTileAvailable == false)
            return false;

        string itemName = _grid[row, col].ItemData.itemName;
        List<Vector2Int> visited = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(row, col));
        visited.Add(new Vector2Int(row, col));
        int matchCount = 0;

        while (queue.Count > 0)
        {
            Vector2Int currentTile = queue.Dequeue();
            matchCount++;

            // Check neighboring tiles for matches
            // Assuming 4-directional matching, you can extend this logic to support other matching patterns

            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(0, 1), // Up
                new Vector2Int(1, 0), // Right
                new Vector2Int(0, -1), // Down
                new Vector2Int(-1, 0), // Left
                new Vector2Int(1, 1), // Diagonal Up Right
                new Vector2Int(-1, -1), // Diagonal Down Left
                new Vector2Int(-1, 1), // Diagonal Up Left
                new Vector2Int(1, -1) // Diagonal Down Right
            };

            foreach (Vector2Int dir in directions)
            {
                int newRow = currentTile.x + dir.x;
                int newCol = currentTile.y + dir.y;

                // Check if the neighboring tile is within the grid boundaries and has the same itemName
                if (newRow >= 0 && newRow < _currentLevel.row && newCol >= 0 && newCol < _currentLevel.column && !visited.Contains(new Vector2Int(newRow, newCol)) && _grid[newRow, newCol].IsTileAvailable && _grid[newRow, newCol].ItemData.itemName == itemName)
                {
                    queue.Enqueue(new Vector2Int(newRow, newCol));
                    visited.Add(new Vector2Int(newRow, newCol));
                }
            }
        }

        return matchCount >= 3;
    }

    private void FillEmptyTilesWithRandomItems()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                if (_grid[i, j].ItemData != null)
                    continue;

                //select random item from itemDatas
                var randomIndex = UnityEngine.Random.Range(0, _tileSettings.itemDatas.Length);

                var randomItemData = _tileSettings.itemDatas[randomIndex];
                _grid[i, j].SetItemData(randomItemData);
                _grid[i, j].ShowTile();
            
            }
        }
    }


    private void ShuffleGridAndCheck()
    {
        SetparentTiles(null);
        
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                var temp = _grid[i, j];
                int rndX = UnityEngine.Random.Range(0, _grid.GetLength(0));
                int rndY = UnityEngine.Random.Range(0, _grid.GetLength(1));
                _grid[i, j] = _grid[rndX, rndY];
                _grid[i, j].row = i;
                _grid[i, j].col = j;

                _grid[rndX, rndY] = temp;
                _grid[rndX, rndY].row = rndX;
                _grid[rndX, rndY].col = rndY;
            }
        }

        SetparentTiles(_gridParent);
    }

    private void SetparentTiles(Transform parentTransform)
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                _grid[i, j].transform.SetParent(parentTransform);
                _grid[i, j].transform.localPosition = Vector3.zero;
                _grid[i, j].transform.localScale = Vector3.one;
            }
        }
    }
    private void OnLevelFailed(LevelData data)
    {
        DespawnAllTiles();
    }

    private void OnLevelLoaded(LevelData level)
    {
        _currentLevel = level;

        DespawnAllTiles();
        GenerateNewGrid(level);
    }

    private void OnPlayerMadeMove(int remainingMoves)
    {
        if(_goalsManager.IsGoalsAchived)
            return;
            
        tryCount = 100;

        while (tryCount > 0)
        {

            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    if (BFSHasValidMatch(i, j))
                        return;
                }
            }

            ShuffleGridAndCheck();

            tryCount -= 1;
        }
        if(tryCount <= 0)
        {
            _levelManager.NoAvailableSet();   
        }
    }
    

    private void OnLevelCompleted(LevelData levelData, bool isHighScoreSet)
    {
        DespawnAllTiles();
    }

    private void DespawnAllTiles()
    {
        if (_grid == null)
            return;

        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                _grid[i, j].Clear();
                _tilePool.Despawn(_grid[i, j]);
            }
        }

        _grid = null;
    }
}

