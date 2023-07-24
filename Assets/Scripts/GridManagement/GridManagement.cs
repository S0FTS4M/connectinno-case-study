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

    //NOTE: this can cause infinite loops so I need to check a lot of things here. But i will leave it here for now
    private void FillGridWithTargetObjectives(List<TargetObjective> targetObjectives)
    {
        foreach (var objective in targetObjectives)
        {
            var itemData = _tileSettings.GetItemData(objective.name);

            for (int i = 0; i < objective.count; i++)
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
    }
}

