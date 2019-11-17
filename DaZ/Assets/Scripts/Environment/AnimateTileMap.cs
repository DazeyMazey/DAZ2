using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimateTileMap: MonoBehaviour
{
    public Sprite[] TileSet;
    public float animateSpeed;

    private int i;
    private Tilemap Map;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        Map = GetComponent<Tilemap>();
        SetTiles();
    }

    private void SetTiles()
    {
        foreach (Vector3Int pos in Map.cellBounds.allPositionsWithin)
        {
            Tile t = (Tile)Map.GetTile(pos);
            if (t && t.sprite)
            {
                t.sprite = TileSet[i % TileSet.Length];
            }
        }
        i++;
        Map.RefreshAllTiles();
        Invoke("SetTiles", animateSpeed);
    }
}
