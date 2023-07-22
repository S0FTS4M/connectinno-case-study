// GridManager.cs
using System;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public GameObject tilePrefab;
    public Transform gridParent;

    private Tile[,] grid;

    [Inject]
    public void Construct(ILevelManager levelManager)
    {
         levelManager.LevelLoaded += OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelData level)
    {
        
    }
}

