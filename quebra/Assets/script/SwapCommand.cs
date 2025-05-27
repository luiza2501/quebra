using UnityEngine;    // <— adicione esta linha
public class SwapCommand : ICommand
{
    private TileData tileA, tileB;

    public SwapCommand(TileData a, TileData b)
    {
        tileA = a;
        tileB = b;
    }

    public void Execute()
    {
        Swap();
    }

    public void Undo()
    {
        Swap();
    }

    private void Swap()
    {
        int indexA = tileA.CurrentIndex;
        int indexB = tileB.CurrentIndex;

        tileA.SetIndex(indexB);
        tileB.SetIndex(indexA);

        // Agora o compilador "vê" o Transform corretamente
        Transform parent = tileA.transform.parent;
        parent.GetChild(indexA).SetSiblingIndex(indexB);
        parent.GetChild(indexB).SetSiblingIndex(indexA);
    }
}