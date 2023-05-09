using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class SlowTiles : MonoBehaviour  {
    [SerializeField] TileBase[] slowTiles = null;

    public bool Contain(TileBase tile) {
        return slowTiles.Contains(tile);
    }

    public TileBase[] Get() { return slowTiles;  }
}
