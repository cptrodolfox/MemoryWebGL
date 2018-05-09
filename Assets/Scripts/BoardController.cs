using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {

    public Button tilePrefab;
    private int cap = 25;
    private int score;
    private int attempts;
    private Text scoreText;
    private Text attemptsText;
    private int[] numbers = new int[25 * 2];

    private Button fstSelectedTile;
    private Button sndSelectedTile;

    private GridLayoutGroup grid;
    private RectTransform rectTransform;
    void Awake()
    {
        InitState();
        // Set Cap for FPS.
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 15;
    }

	void Start ()
    {
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        attemptsText = GameObject.Find("Attempts").GetComponent<Text>();
        Button resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        grid = gameObject.GetComponent<GridLayoutGroup>();
        rectTransform = gameObject.GetComponent<RectTransform>();
        grid.cellSize = new Vector2(rectTransform.rect.width / 10, rectTransform.rect.height / 5);
        resetButton.onClick.AddListener(delegate { Reset(); });
        StartCoroutine(InitTiles());
	}
	
	void Update () {
        scoreText.text = "Score: " + score.ToString();
        attemptsText.text ="Attempts: " + attempts.ToString();

        if (fstSelectedTile != null)
        {
            if (sndSelectedTile != null)
            {
                if (CheckSelection())
                {
                    fstSelectedTile.GetComponent<TileController>().Hide();
                    sndSelectedTile.GetComponent<TileController>().Hide();
                    score++;
                }
                else
                {
                    IEnumerator fstTileDeselectCoroutine = fstSelectedTile.GetComponent<TileController>().Deselect();
                    IEnumerator sndTileDeselectCoroutine = sndSelectedTile.GetComponent<TileController>().Deselect();
                    StartCoroutine(fstTileDeselectCoroutine);
                    StartCoroutine(sndTileDeselectCoroutine);
                    attempts++;
                }
                fstSelectedTile = null;
                sndSelectedTile = null;
            }

        }
		
	}

    void TileClick (string tileName)
    {
        Button clickedTile = GameObject.Find(tileName).GetComponent<Button>();
        clickedTile.GetComponent<TileController>().Select();
        if (fstSelectedTile == null && sndSelectedTile == null)
        {
            fstSelectedTile = clickedTile;
        }
        else
        {
            if (sndSelectedTile == null)
            {
                sndSelectedTile = clickedTile;
            }
        }
    }

    bool CheckSelection()
    {
        int fstTileNumber = fstSelectedTile.GetComponent<TileController>().number;
        int sndTileNumber = sndSelectedTile.GetComponent<TileController>().number;
        
        if(fstTileNumber == sndTileNumber)
        {
            return true;
        }
        return false;
    }
    public void AddTile(Button tile)
    {
        if (tile == null)
        {
            return;
        }

        tile.transform.SetParent(gameObject.transform, false);
    }

    void Reset ()
    {
        DeleteTiles();
        InitState();
        StartCoroutine(InitTiles());
    }

    void InitState()
    {
        // Generates the list of numbers the numbers appear twice.
        for (int i = 0; i < cap; i++)
        {
            numbers[i] = i;
            numbers[cap + i] = i;
        }
        // Shuffles the number's list.
        System.Random rng = new System.Random();
        int n = numbers.Length;
        while (n > 1)
        {
            int h = rng.Next(0, n);
            n--;
            int temp = numbers[h];
            numbers[h] = numbers[n];
            numbers[n] = temp;
        }
        attempts = 0;
        score = 0;
    }

    // Deletes the Tiles from game.
    void DeleteTiles ()
    {
        for(int i=0; i < 2*cap; i++)
        {
            GameObject obj = GameObject.Find(i.ToString());
            Destroy(obj);
        }
    }

    // Create and draw tiles.
    IEnumerator InitTiles ()
    {

        // Creates tiles and adds it to the gridlayout.
        int tileName = 0;
        foreach (var number in numbers)
        {
            Button tile = Instantiate(tilePrefab);
            tile.GetComponentInChildren<TileController>().number = number;
            tile.name = tileName.ToString();
            tile.GetComponent<Button>().onClick.AddListener(delegate { TileClick(tile.name); });
            AddTile(tile);
            tileName++;
            yield return new WaitForSeconds(0.001f);
        }
    }

    void OnRectTransformDimensionsChange ()
    {
        if (grid != null && rectTransform != null)
        {
            grid.cellSize = new Vector2(rectTransform.rect.width / 10, rectTransform.rect.height / 5);
        }
    }
}
