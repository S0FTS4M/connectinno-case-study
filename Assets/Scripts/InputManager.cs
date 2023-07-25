using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    string _selectedItemName = string.Empty;
    bool _isConnecting = false;

    HashSet<Tile> _connectedTiles = new HashSet<Tile>();

    private PlayerGoalsManager _playerGoalsManager;

    private ILevelManager _levelManager;

    private Tile _prevTile;

    [Inject]
    private void Construct(PlayerGoalsManager playerGoalsManager, ILevelManager levelManager)
    {
        _playerGoalsManager = playerGoalsManager;
        _levelManager = levelManager;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var tile = GetTileUnderTouch(eventData);
        if (tile != null)
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
        if (_isConnecting == false)
            return;

        var tile = GetTileUnderTouch(eventData);
        if (tile != null && _connectedTiles.Contains(tile) == false)
        {
            if (tile.ItemData.itemName == _selectedItemName && _prevTile.IsValidNeighbor(tile))
            {
                _connectedTiles.Add(tile);
                tile.OnTilePressed();
                _prevTile = tile;
            }
            else
            {
                RevertTileChanges();
                ResetState();
                _levelManager.PlayerMadeAMove();
            }

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isConnecting == false)
            return;
        if (_connectedTiles.Count < 3)
        {
            RevertTileChanges();

            ResetState();

            _levelManager.PlayerMadeAMove();
            return;
        }

        var seq = DOTween.Sequence();
        foreach (var tile in _connectedTiles)
        {
            seq.AppendCallback(() => tile.HideTile());
            seq.AppendCallback(() => tile.BreakTile());
            seq.AppendInterval(0.05f);
        }
        _levelManager.AddScore(_connectedTiles.Count);

        seq.AppendInterval(0.2f);
        var selectedItemName = _selectedItemName;
        var itemAmount = _connectedTiles.Count;
        seq.AppendCallback(() => _playerGoalsManager.UpdatePlayerGoal(selectedItemName, itemAmount));
        seq.AppendCallback(_levelManager.PlayerMadeAMove);

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
        foreach (var tile in _connectedTiles)
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
            if (tile != null)
            {
                return tile;
            }
        }

        return null;
    }
}
