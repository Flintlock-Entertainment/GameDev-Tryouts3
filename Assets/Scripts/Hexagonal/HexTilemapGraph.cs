using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * A graph that represents a tilemap, using only the allowed tiles.
 */
public class HexTilemapGraph: IGraph<Vector3Int> {
    private Tilemap tilemap;
    private TileBase[] allowedTiles;
    private TileBase[] slowTiles;

    public HexTilemapGraph(Tilemap tilemap, TileBase[] allowedTiles, TileBase[] slowTiles) {
        this.tilemap = tilemap;
        this.allowedTiles = allowedTiles;
        this.slowTiles = slowTiles;
    }

    static Vector3Int[] directions = {
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(-1, -1, 0),
            new Vector3Int(1, 1, 0),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(1, -1, 0),
    };

    public IEnumerable<Vector3Int> Neighbors(Vector3Int node) {
        foreach (var direction in directions) {
            Vector3Int neighborPos = node + direction;
            TileBase neighborTile = tilemap.GetTile(neighborPos);
            if (allowedTiles.Contains(neighborTile))
                yield return neighborPos;
                
        }
    }
    public float Heuristic(Vector3Int node, Vector3Int end)
    {
        return DistanceMatric(node, end, 0);
    }

    public float Distance(Vector3Int node1, Vector3Int node2)
    {
        TileBase tile1 = tilemap.GetTile(node1);
        TileBase tile2 = tilemap.GetTile(node2);
        float addition = 0;
        if (slowTiles.Contains(tile2))
        {
            addition = 1;
        }
        return DistanceMatric(node1, node2, addition);

    }

    private float DistanceMatric(Vector3Int node, Vector3Int end, float addition)
    {
        float dx = Mathf.Abs(node.x - end.x);
        float dy = Mathf.Abs(node.y - end.y);
        return Mathf.Sqrt((dx * dx) + (dy * dy)) + addition;
    }
}
