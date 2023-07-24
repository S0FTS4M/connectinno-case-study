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

    [Inject]
    public void Construct(ILevelManager levelManager, TilePool tilePool, Tile.Settings tileSettings)
    {
        _tilePool = tilePool;
        _tileSettings = tileSettings;

        _gridLayoutGroup = _gridParent.GetComponent<GridLayoutGroup>();

        levelManager.LevelLoaded += OnLevelLoaded;

        levelManager.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelLoaded(LevelData level)
    {
        DespawnAllTiles();
        GenerateNewGrid(level);
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
                if(_grid[i,j] != null)
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
            targetObjectiveCount += objective.count;
        }

        if(maxCellCount < targetObjectiveCount)
        {
            Debug.LogError("Not enough cells to fill all target objectives consider increasing the grid size or decreasing the number of target objectives!!!");
            return;
        }

        foreach (var objective in targetObjectives)
        {
            var itemData = _tileSettings.GetItemData(objective.name);
            
            //NOTE: I am trying to make sure there are at least 3 items or multiples of 3 in each objective in order to make the game more playable
            var remainingObjectiveCount = 3 - (objective.count % 3 == 0 ? 3 : objective.count % 3);

            for (int i = 0; i < objective.count + remainingObjectiveCount; i++)
            {
                var rndX = UnityEngine.Random.Range(0, _grid.GetLength(0));
                var rndY = UnityEngine.Random.Range(0, _grid.GetLength(1));
                while (_grid[rndX, rndY].ItemData != null)
                {
                    rndX = UnityEngine.Random.Range(0, _grid.GetLength(0));
                    rndY = UnityEngine.Random.Range(0, _grid.GetLength(1));
                }

                _grid[rndX, rndY].SetItemData(itemData);
            }
        }
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
            }
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
                _tilePool.Despawn(_grid[i, j]);
            }
        }

        _grid = null;
    }
}

