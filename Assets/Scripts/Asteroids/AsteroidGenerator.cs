using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AsteroidGenerator : MonoBehaviour
{
    [Header("References")]
    public Tilemap tilemap;
    [SerializeField] private Tilemap miniMapTilemap;
    public AsteroidType asteroidType;
    [SerializeField] private TileBase miniMapTileBase;

    [Header("Size & Shape")]
    [SerializeField] private float massPerUnit = .25f;
    [SerializeField] private float radius = 14f;
    [SerializeField] private float noise = 3f;

    void Start()
    {
        miniMapTilemap.color = asteroidType.minimapColor;

        GenerateAsteroid(asteroidType);
        CopyTilemapToMiniMap();
    }

    public void GenerateAsteroid(AsteroidType type)
    {
        if (!IsTilemapEmpty())
            return;

        if (type == null)
        {
            Destroy(gameObject);
            return;
        }

        float size = radius * 2 + 4;

        tilemap.ClearAllTiles();
        Vector2 center = new Vector2(size / 2f, size / 2f);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Vector2 p = new Vector2(x, y);
                float dist = Vector2.Distance(p, center);
                float n = Mathf.PerlinNoise(x * 0.15f, y * 0.15f);
                float r = radius + (n - 0.5f) * noise * 2f;

                if (dist < r)
                    tilemap.SetTile(new Vector3Int(x, y, 0), type.tileBase);
            }
        }

        RecalculateMassAndCenter();
    }

    public void RecalculateMassAndCenter()
    {
        BoundsInt bounds = tilemap.cellBounds;

        int tileCount = 0;
        Vector2 sum = Vector2.zero;

        // mass and center
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            tileCount++;

            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
            Vector3 localPos = transform.InverseTransformPoint(worldPos);

            sum += (Vector2)localPos;
        }

        if (tileCount == 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 centerOfMass = sum / tileCount;

        // inertia
        float inertia = 0f;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            Vector3 worldPos = tilemap.GetCellCenterWorld(pos);
            Vector3 localPos = transform.InverseTransformPoint(worldPos);

            Vector2 offset = (Vector2)localPos - centerOfMass;

            inertia += massPerUnit * offset.sqrMagnitude;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = tileCount * massPerUnit;
        rb.centerOfMass = centerOfMass;
        rb.inertia = inertia;
    }

    public void CopyTilemapToMiniMap()
    {
        miniMapTilemap.ClearAllTiles();
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (tilemap.HasTile(pos))
            {
                miniMapTilemap.SetTile(pos, miniMapTileBase);
            }
        }

        miniMapTilemap.RefreshAllTiles();
    }

    public bool IsTilemapEmpty()
    {
        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0); // Z is typically 0 for 2D
                if (tilemap.HasTile(cellPosition))
                {
                    return false;
                }
            }
        }
        return true;
    }
}