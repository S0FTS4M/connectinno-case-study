using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    string _selectedItemName = string.Empty;
    bool _isConnecting = false;

    HashSet<Tile> _connectedTiles = new HashSet<Tile>();

    private PlayerGoalsManager _playerGoalsManager;

    private Tile _prevTile;

    [Inject]
    private void Construct(PlayerGoalsManager playerGoalsManager)
    {
        _playerGoalsManager = playerGoalsManager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var tile = GetTileUnderTouch(eventData);
        if(tile != null)
        {
            _selectedItemName = tile.ItemData.itemName;
            _isConnecting = true;
            _connectedTiles.Add(tile);
            tile.OnTilePressed();

            _prevTile = tile;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_isConnecting == false)
            return;

        var tile = GetTileUnderTouch(eventData);
        if(tile != null && _connectedTiles.Contains(tile) == false)
        {
            if(tile.ItemData.itemName == _selectedItemName && _prevTile.IsValidNeighbor(tile))
            {
                _connectedTiles.Add(tile);
                tile.OnTilePressed();
                _prevTile = tile;
            }
            else
            {
                RevertTileChanges();
                ResetState();
                //TODO: update move count

            }
            
        }
    
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(_isConnecting == false)
            return;
        if(_connectedTiles.Count < 3)
        {
            RevertTileChanges();

            ResetState();
            return;
        }

        //Check if connected tiles are actually has connection. 
        //Player can try to connect tiles around the map

        foreach(var tile in _connectedTiles)
        {
            //EXPLODE tile

            tile.HideTile();

            tile.BreakTile();

            //Update move

            //Check goals and update
        }

        _playerGoalsManager.UpdatePlayerGoal(_selectedItemName, _connectedTiles.Count);

        ResetState();
    }

    private void ResetState()
    {
        _connectedTiles.Clear();
        _isConnecting = false;
        _selectedItemName = string.Empty;
        _prevTile = null;
    }

    private void RevertTileChanges()
    {
        foreach(var tile in _connectedTiles)
        {
            tile.OnTileReleased();
        }
    }
    

    private Tile GetTileUnderTouch(PointerEventData eventData)
    {
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, result);
        for (int i = 0; i < result.Count; i++)
        {
            var tile = result[i].gameObject.GetComponentInParent<Tile>();
            if(tile != null)
            {
                return tile;
            }
        }

        return null;
    }
}
