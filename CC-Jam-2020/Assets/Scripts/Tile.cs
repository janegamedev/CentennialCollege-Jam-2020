using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public List<ObjectInstance> objects;
    private Vector2Int _position;

    public Tile(Vector2Int pos)
    {
        objects = new List<ObjectInstance>();
        _position = pos;
    }

    public bool IsEmpty => objects.Count == 0;

    public bool IsPassable() => IsEmpty || (objects.Count == 1 && objects[0].data.isPassable);
    public ObjectInstance GetMovable => objects.First(x => x.data.isMovable && !x.data.isPassable);

    public void AddObject(ObjectInstance o)
    {
        objects.Add(o);
        o.gridPos = _position;
        o.tile = this;
    }

    public void RemoveObject(ObjectInstance o)
    {
        objects.Remove(o);
    }

    public void ClearTile()
    {
        objects.Clear();
    }
}