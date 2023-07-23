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

        //NOTE: I need to make sure that the created items matches the objectives
        var objectiveDict = new Dictionary<string, int>();
        foreach (var objective in level.targetObjectives)
        {
            objectiveDict.Add(objective.name, objective.count);
        }

        for (int i = 0; i < level.row; i++)
        {
            for (int j = 0; j < level.column; j++)
            {
                _grid[i, j] = _tilePool.Spawn();
                _grid[i, j].transform.SetParent(_gridParent);

                _grid[i, j].row = i;
                _grid[i, j].col = j;
                //select random item from itemDatas
                var randomIndex = UnityEngine.Random.Range(0, _tileSettings.itemDatas.Length);

                var randomItemData = _tileSettings.itemDatas[randomIndex];
                _grid[i, j].SetItemData(randomItemData);

                if (objectiveDict.ContainsKey(randomItemData.name))
                    objectiveDict[randomItemData.name]--;
            }
        }

        //NOTE: I will not implement this part yet, because this can cause infinite loops so I need to check a lot of things here.
        // maybe I will come back to this later. Maybe not... So if you have a time please implement this.
        // foreach (var objective in objectiveDict)
        // {
        //     if(objective.Value > 0)
        //     {
        //         var itemData = _tileSettings.GetItemData(objective.Key);
        //     }


        // }

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

