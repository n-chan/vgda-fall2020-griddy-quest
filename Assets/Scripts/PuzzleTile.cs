using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleTile : MonoBehaviour
{
    public AnimationClip clearAnimation;
    public AnimationClip clearAnimationReverse;

    public int column;
    public int row;

    private PuzzleBoard board;
    private static PuzzleTile previousSelected = null;
    private SpriteRenderer render;

    private bool isSelected = false;

    // Start is called before the first frame update
    void Start() {
        board = FindObjectOfType<PuzzleBoard>();
    }

    void Awake() {
        render = transform.Find("piece").GetComponent<SpriteRenderer>();
        render.sortingOrder = 1;
    }

    /**
     * Constructor; A PuzzleTile has a column and a row.
     */ 
    public void Init(int c, int r, PuzzleBoard b) {
        column = c;
        row = r;
        board = b;
    } 

    /**
     * When player clicks on a tile, dim the sprite color.
     */ 
    private void Select() {
        isSelected = true;
        previousSelected = gameObject.GetComponent<PuzzleTile>();
        transform.Find("piece").GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1.0f);
    }

    /**
     * Deselect the tile.
     */ 
    private void Deselect() {
        isSelected = false;
        render.color = Color.white;
        previousSelected = null;
    }
    
    /**
     * When player clicks on a tile...
     */ 
    private void OnMouseDown() {
        if (render.sprite == null) {
            return;
        }
        if (isSelected) { 
            Deselect();
        }
        else {
            if (previousSelected == null) { 
                Select();
            }
            else {
                SwapSprite();
                previousSelected.Deselect(); 
            }
        }
    }

    /**
     * Swaps two tiles' sprites.
     */ 
    public void SwapSprite() {
        Sprite temp = transform.Find("piece").GetComponent<SpriteRenderer>().sprite;
        transform.Find("piece").GetComponent<SpriteRenderer>().sprite = previousSelected.transform.Find("piece").GetComponent<SpriteRenderer>().sprite;
        previousSelected.transform.Find("piece").GetComponent<SpriteRenderer>().sprite = temp;

        if (board.CheckMatchesExist()) {
            StartCoroutine(board.DestroyMatches());
            /**
             * Go back to previous scene after 3 moves. Definitely, a placeholder. TODO!
             */
            board.AddCount();
            if (board.GetCount() == 3) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
        else {
            temp = transform.Find("piece").GetComponent<SpriteRenderer>().sprite;
            transform.Find("piece").GetComponent<SpriteRenderer>().sprite = previousSelected.transform.Find("piece").GetComponent<SpriteRenderer>().sprite;
            previousSelected.transform.Find("piece").GetComponent<SpriteRenderer>().sprite = temp;
        }
    }
}
