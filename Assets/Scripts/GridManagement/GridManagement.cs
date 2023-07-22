// GridManager.cs
using System;
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
                _grid[i, j] = _tilePool.Spawn();
                _grid[i, j].transform.SetParent(_gridParent);  
                //select random item from itemDatas
                var randomIndex = UnityEngine.Random.Range(0, _tileSettings.itemDatas.Length);
                _grid[i, j].SetItemData(_tileSettings.itemDatas[randomIndex]);
            }
        }
    }

    private void DespawnAllTiles()
    {
        if(_grid == null)
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

