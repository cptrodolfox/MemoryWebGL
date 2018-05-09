using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    public Sprite tileNormal;
    public Sprite tileSelected;
    public Sprite tileHidden;

    public int number;
    private bool selected;
    private bool hidden;

    private Color normalTileColor = new Color(72.5f, 72.5f, 72.2f);
    private Color selectedTileColor = new Color(91.4f, 38.4f, 11.8f);

	void Start () {
        gameObject.GetComponentInChildren<Text>().text = "";
        gameObject.GetComponent<Image>().color = selectedTileColor;
        
    }

    void Awake() {
       selected = false;
       hidden = false;
    }

    void Update()
    {
        if (hidden)
        {
            gameObject.GetComponentInChildren<Image>().sprite = tileHidden;
            gameObject.GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
            gameObject.GetComponentInChildren<Text>().text = "";
            gameObject.GetComponent<Button>().enabled = false;
        }
        else
        {
            if (!selected)
            {
                gameObject.GetComponentInChildren<Image>().sprite = tileNormal;
                gameObject.GetComponentInChildren<Text>().text = "";
            }
            else
            {
                gameObject.GetComponentInChildren<Image>().sprite = tileSelected;
                gameObject.GetComponentInChildren<Text>().text = number.ToString();
            }
        }
        
    }

    public void Hide()
    {
        hidden = true;
    }

    public void Select()
    {
        selected = true;
    }

    public IEnumerator Deselect()
    {
        yield return new WaitForSeconds(1f);
        selected = false;
    }
    
    IEnumerator ChangeColorAnimation()
    {
        float animationTime = 0.3f;
        int steps = 20;

        float deltaR = (selectedTileColor.r - normalTileColor.r)/steps;
        float deltaG = (selectedTileColor.g - normalTileColor.g)/steps;
        float deltaB = (selectedTileColor.b - normalTileColor.b)/steps;

        float deltaTime = animationTime / steps;

        for (int i=0; i<steps; i++)
        {
            float red = selectedTileColor.r + deltaR * i;
            float green = selectedTileColor.g + deltaG * i;
            float blue = selectedTileColor.b + deltaB * i;

            Color tileColor = new Color(red, green, blue, 1f);
            gameObject.GetComponentInChildren<Image>().color = tileColor;
            yield return new WaitForSeconds(deltaTime);
        }
        gameObject.GetComponent<Image>().color = Color.white;
        gameObject.GetComponentInChildren<Image>().sprite = tileNormal;
        yield return null;

    }

}
