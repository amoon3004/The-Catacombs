using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

// The Catacombs - builds level from a .txt file
// #: wall, .: floor, S: player start, D: exit door, K: key, T: torch power up, L: lava

public class MazeLoader : MonoBehaviour
{
    [Header("Levels (text files, in order)")]
    public TextAsset[] levels;
    public int currentLevel = 0;

    [Header("Scene references")]
    public Transform player;
    public Tilemap floorMap;
    public Tilemap wallMap;

    [Header("Tiles (leave empty to use colored squares")]
    public TileBase floorTile;
    public TileBase wallTile;
    public Color placeholderFloorColor = new Color(0.45f, 0.40f, 0.33f);
    public Color placeholderWallColor = new Color(0.15f, 0.13f, 0.12f);

    [Header("Prefabs (leave empty until they exist")]
    public GameObject doorPrefab;
    public GameObject keyPrefab;
    public GameObject torchPickupPrefab;
    public GameObject lavaPrefab;
    public GameObject heartPrefab;

    // Where player starts the level
    public Vector3 StartPosition { get; private set; }

    Transform levelObjects; // Parent for everything spawned, can be cleared

    void Start()
    {
        if (floorTile == null) floorTile = MakePlainTile(placeholderFloorColor);
        if (wallTile == null) wallTile = MakePlainTile(placeholderWallColor);
        Load(currentLevel);
    }

    // Move to the next level (stays on last level when there are no more)
    public void NextLevel()
    {
        currentLevel = Mathf.Min(currentLevel + 1, levels.Length - 1);
        Load(currentLevel);
    }

    // Send player back to starting position of current level
    public void RespawnPlayer()
    {
        player.position = StartPosition;
    }

    public void Load(int index)
    {
        // Clear what previous level left behind
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();
        if (levelObjects != null) Destroy(levelObjects.gameObject);
        levelObjects = new GameObject("LevelObjects").transform;

        string[] rows = levels[index].text.Replace("\r", "").Trim().Split('\n');
        int height = rows.Length;
        int keyCount = 0;

        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < rows[r].Length; c++)
            {
                // Row 0 of the text file is the top of the maze, flip Y
                var cell = new Vector3Int(c, height - 1 - r, 0);
                char symbol = rows[r][c];

                if (symbol == '#')
                {
                    wallMap.SetTile(cell, wallTile);
                    continue;
                }

                // Everything not a wall sits on a floor tile
                floorMap.SetTile(cell, floorTile);
                Vector3 pos = floorMap.GetCellCenterWorld(cell);

                switch (symbol)
                {
                    case 'S': StartPosition = pos; RespawnPlayer(); break;
                    case 'D': Spawn(doorPrefab, pos); break;
                    case 'K': keyCount++; Spawn(keyPrefab, pos); break;
                    case 'T': Spawn(torchPickupPrefab, pos); break;
                    case 'L': Spawn(lavaPrefab, pos); break;
                    case 'H': if (!GameManager.Instance.HeartUsed(index)) Spawn(heartPrefab, pos); break;
                }
            }
        }

        GameManager.Instance.StartLevel(keyCount);
    }

    void Spawn (GameObject prefab, Vector3 pos)
    {
        if (prefab != null) Instantiate(prefab, pos, prefab.transform.rotation, levelObjects);
    }

    // Builds a 1x1 tile to test maze functionality before art is added
    Tile MakePlainTile(Color color)
    {
        Texture2D tex = Texture2D.whiteTexture;
        Sprite square = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), tex.width);
        Tile tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = square;
        tile.color = color;
        tile.colliderType = Tile.ColliderType.Grid;
        return tile;
    }
}
