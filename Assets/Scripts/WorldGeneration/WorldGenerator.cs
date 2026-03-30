using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] int seed;

    [Header("Chunk floor")]
    [SerializeField] GameObject chunkFloor;
    [SerializeField] Transform chunkFloorParent;

    [SerializeField] GameObject spawnableObject;
    [SerializeField] GameObject enemy;
    [SerializeField] float chunkSize = 5;
    [SerializeField] float chunkRadius = 5;
    private int _chunkRadiusInt;

    private HashSet<Vector2Int> _currentChunks = new();
    private Vector2Int _lastChunk;

    public void Awake()
    {
        Random.InitState(seed);
        _chunkRadiusInt = Mathf.RoundToInt(chunkRadius);

        chunkFloor.transform.localScale = Vector3.one * chunkSize * 0.1f;

        GenerateChunksAt(GetCurrentChunk(Vector3.zero));
    }

    public Vector2Int GetCurrentChunk(Vector3 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x / chunkSize), Mathf.RoundToInt(position.z / chunkSize));
    }

    public void GenerateChunksAt(Vector2Int chunkPos)
    {
        // check and remove chunks from current chuncks

        _currentChunks.Clear();
        for (int y = -_chunkRadiusInt; y <= _chunkRadiusInt; y++)
        {
            for (int x = -_chunkRadiusInt; x <= _chunkRadiusInt; x++)
            {
                if (Mathf.Sqrt(x * x + y * y) <= chunkRadius)
                {
                    _currentChunks.Add(new Vector2Int(chunkPos.x + x, chunkPos.y + y));
                }
            }
        }

        foreach (Vector2Int pos in _currentChunks)
        {
            GenerateChunk(pos);
        }
    }

    public void GenerateChunk(Vector2Int pos)
    {
        Vector3 startPos = new Vector3(pos.x * chunkSize, 0, pos.y * chunkSize);
        Instantiate(chunkFloor, startPos, chunkFloor.transform.rotation, chunkFloorParent);
    }
}
