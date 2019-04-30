using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}

public struct TilePos
{
    public int x;
    public int y;
    public TilePos( int _x, int _y) {
        x = _x; y = _y;
    }
}

public class LevelGen : MonoBehaviour {

    public float tileSz = 1.0f;
    int[,] tilemap = new int[100, 5];

    public TileChonk[] chonks;

    public int difficulty = 3;

	// Use this for initialization
	void Start () {
        Debug.LogFormat("In LevelGen Start");
        Generate();
        Build();
	}
	
	// Update is called once per frame
	void Update () {		
	}

    public void Generate()
    {
        PlanOpenTiles();
    }

    public void Build()
    {
        // Nuke any placed tiles
        foreach(Transform t in transform) {            
            Destroy(t.gameObject);
        }

        // Gather chonk lists
        List<TileChonk> blockerChonks = new List<TileChonk>();
        List<TileChonk> stuffChonks = new List<TileChonk>();
        for (int i=0; i < chonks.Length; i++)
        {
            if (chonks[i].chonkType == TileChonk.ChonkType.Chonk_BLOCKER)
            {
                blockerChonks.Add(chonks[i]);
            }
            else
            {
                stuffChonks.Add(chonks[i]);
            }
        }
        Debug.LogFormat("Blocker Chonks {0} Stuff Chonks {1}", blockerChonks.Count, stuffChonks.Count);

        float halfTileSz = tileSz * 0.5f;
        for (int j = 0; j < tilemap.GetLength(1); j++)
        {
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                int j2 = tilemap.GetLength(1) - (j + 1);
                Vector3 tileCenter = new Vector3((i * tileSz) + halfTileSz,
                             (j2 * tileSz) + halfTileSz, 0.5f);

                // Blocker?
                Transform tile;
                if (tilemap[i, j] == 1)
                {
                   
                    tile = Instantiate<Transform>(blockerChonks[Random.Range(0, blockerChonks.Count)].tilePrefab, transform);
                    tile.localPosition = tileCenter;
                } else if (tilemap[i,j] == 2)
                {
                    tile = Instantiate<Transform>(stuffChonks[Random.Range(0, stuffChonks.Count)].tilePrefab, transform);
                    tile.localPosition = tileCenter;
                }
                
            }
        }

    }

    void ClearTile( int i, int j )
    {
        if ((i >=0) && (i < tilemap.GetLength(0)) &&
            (j >= 0) && (j < tilemap.GetLength(1)) )
        {
            tilemap[i, j] = 0;
        }
    }

    void PlanOpenTiles()
    {
        for (int j = 0; j < tilemap.GetLength(1); j++)
        {
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                
                tilemap[i, j] = ((i < 6) && (j > 0))?0:1 ;
            }
        }

        for (int i = 0; i < 5 - difficulty; i++)
        {
            CutTilePath();
        }

        PlaceStuffs();
    }

    void CutTilePath() 
    {
        int currX = 0;
        int currY = tilemap.GetLength(1) / 2;
        while (currX < tilemap.GetLength(0)) {

            // Clear this tile
            int offs = (currY == 0) ? +1 : -1;
            ClearTile(currX, currY);
            ClearTile(currX, currY);
            ClearTile(currX+1, currY+offs);
            ClearTile(currX+1, currY+offs);

            int moveDir = UnityEngine.Random.Range(0, 3);
            if ((moveDir == 1) && (currY < tilemap.GetLength(1) - 1)) {
                currY += 1; // UP
            }
            else if ((moveDir == 2) && (currY > 0)) {
                currY -= 1; // DOWN;
            } else {
                currX += 1; // always forward
            }
        }
    }

    void PlaceStuffs( )
    {
        List<TilePos> placements = new List<TilePos>();
        for (int j = 0; j < tilemap.GetLength(1); j++)
        {
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                if (tilemap[i, j] == 0) {
                    placements.Add(new TilePos(i, j));
                }
            }
        }

        placements.Shuffle();


        float[] pctForDifficulty = { 0.2f, 0.15f, 0.1f, 0.05f, 0.01f };
        int numThings = (int)(placements.Count * pctForDifficulty[difficulty-1]);
        for (int i = 0; i < numThings; i++) {
            TilePos pos = placements[i];
            tilemap[pos.x, pos.y] = 2;
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.cyan;
        Vector3 mapSz = new Vector3( tilemap.GetLength(0) * tileSz, 
                                     tilemap.GetLength(1) * tileSz, 1.0f);
        Gizmos.DrawWireCube( new Vector3( mapSz.x / 2f, mapSz.y / 2f, 0.5f), mapSz );


        // Draw "tiles"
        float halfTileSz = tileSz * 0.5f;
        for (int j = 0; j < tilemap.GetLength(1); j++)
        {
            for (int i = 0; i < tilemap.GetLength(0); i++)
            {
                if (tilemap[i, j] != 0)
                {
                    if (tilemap[i,j]==1) {
                        Gizmos.color = Color.blue;
                    } else {
                        Gizmos.color = Color.yellow;
                    }
                    int j2 = tilemap.GetLength(1) - (j + 1);
                    Vector3 tileCenter = new Vector3((i * tileSz) + halfTileSz, 
                                                     (j2 * tileSz) + halfTileSz, 0.5f);
                    Gizmos.DrawWireCube( tileCenter, new Vector3(tileSz , tileSz , 0.9f) );
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelGen))]
public class LevelGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelGen level = (LevelGen)target;
        if (GUILayout.Button("generate"))
        {
            level.Generate();
        }

        if (GUILayout.Button("build"))
        {
            level.Build();
        }

        DrawDefaultInspector();
    }
}
#endif
