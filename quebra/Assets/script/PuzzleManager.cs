using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    public Sprite[] sprites;
    public Button undoButton;
    public Button replayButton;
    public Button skipReplayButton;
    public GameObject victoryPanel;

    private TileData selectedTile = null;
    private List<TileData> tiles = new List<TileData>();

    void Start()
    {
        CreatePuzzle();
        ShuffleTiles();
        undoButton.onClick.AddListener(() => CommandManager.Instance.Undo());
        replayButton.onClick.AddListener(() =>
        {
            victoryPanel.SetActive(false);
            CommandManager.Instance.Replay(() => victoryPanel.SetActive(true));
        });
        skipReplayButton.onClick.AddListener(() =>
        {
            CommandManager.Instance.SkipReplay();
            victoryPanel.SetActive(true);
        });
    }

    void CreatePuzzle()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject obj = Instantiate(tilePrefab, gridParent);
            TileData tile = obj.GetComponent<TileData>();
            tile.CorrectIndex = i;
            tile.SetIndex(i);
            obj.GetComponent<Image>().sprite = sprites[i];

            int localIndex = i; // capture
            obj.GetComponent<Button>().onClick.AddListener(() => SelectTile(tile));
            tiles.Add(tile);
        }
    }

    void ShuffleTiles()
    {
        for (int i = 0; i < 50; i++)
        {
            int a = Random.Range(0, 16);
            int b = Random.Range(0, 16);
            if (a != b)
                CommandManager.Instance.ExecuteCommand(new SwapCommand(tiles[a], tiles[b]));
        }

        CommandManager.Instance.Clear(); // clear after shuffle
    }

    void SelectTile(TileData tile)
    {
        if (selectedTile == null)
        {
            selectedTile = tile;
        }
        else
        {
            if (selectedTile != tile)
            {
                CommandManager.Instance.ExecuteCommand(new SwapCommand(selectedTile, tile));
                CheckVictory();
            }
            selectedTile = null;
        }
    }

    void CheckVictory()
    {
        foreach (var tile in tiles)
        {
            if (tile.CurrentIndex != tile.CorrectIndex)
                return;
        }

        victoryPanel.SetActive(true);
    }
}
