using UnityEngine;

public class TileData : MonoBehaviour
{
    public int CorrectIndex;
    public int CurrentIndex;

    public void SetIndex(int index)
    {
        CurrentIndex = index;
        transform.SetSiblingIndex(index);
    }
}

