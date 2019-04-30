using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Chonk", menuName = "PigGame/Chonk")]
public class TileChonk : ScriptableObject
{
    public enum ChonkType
    {
        Chonk_OPEN,
        Chonk_BLOCKER,
        Chonk_STUFF,
    }

    public Transform tilePrefab;
    public int W = 1;
    public int H = 1;
    public ChonkType chonkType = ChonkType.Chonk_BLOCKER;
};
