using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleBoard : MonoBehaviour
{
    public int width;
    public int height;
    public int count;

    public GameObject[,] backgroundTiles;
    public PuzzleTile[,] puzzleTiles;
    public Sprite[] puzzleSprites;

    public GameObject backgroundTilePrefab;
    public GameObject puzzleTilePrefab;

    public FloatValue storedCharacterHealth;

    List<int> indexes;

    public void Awake() {
        puzzleSprites = Resources.LoadAll<Sprite>("Art/TileSprites/PheaTileSprites");
    }

    // Start is called before the first frame update
    void Start() {
        backgroundTiles = new GameObject[width, height];
        puzzleTiles = new PuzzleTile[width, height];
        
        SetUp();
    }

    /**
     * Sets up the initial board.
     */
    private void SetUp() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {

                // Creates the background grid tiles.
                Vector3 tempPosition = new Vector3(i, j, 0);
                GameObject backgroundTile = Instantiate(backgroundTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "( " + i + ", " + j + " )";

                int randomIndex;

                // Makes sure there is no matches in the initial board.
                do {
                    randomIndex = Random.Range(0, puzzleSprites.Length);
                } while (
                    (i >= 2 && puzzleTiles[(i-1), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleSprites[randomIndex] &&
                    puzzleTiles[(i - 2), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleSprites[randomIndex]) ||

                    (j >= 2 && puzzleTiles[i, (j-1)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleSprites[randomIndex] &&
                    puzzleTiles[i, (j-2)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleSprites[randomIndex]));

                // Creates the actual puzzle tiles.
                GameObject puzzleTile = Instantiate(puzzleTilePrefab, tempPosition, Quaternion.identity);
                
                puzzleTile.transform.Find("piece").GetComponent<SpriteRenderer>().sprite = puzzleSprites[randomIndex];
                puzzleTile.name = "Puzzle Tile";
                puzzleTile.transform.parent = this.transform;
                puzzleTiles[i, j] = puzzleTile.GetComponent<PuzzleTile>();
                puzzleTiles[i, j].Init(i, j, this);
            }
        }
    }
    
    /**
     * A coroutine method that works the following way:
     * Wait 0.1s.
     * Check to see if there are matches on the board.
     * If there is, find them.
     * Remove them (By removing their sprites).
     * Refill the board.
     */ 
    public IEnumerator DestroyMatches() {
        yield return new WaitForSeconds(0.1f);
        while (CheckMatchesExist()) {
            FindMatchThrees();
            yield return new WaitForSeconds(0.5f);
            //Sets the matching tiles to have a dimmer color.
            foreach (int i in indexes) {
                int x = i % 8;
                int y = i / 8;

                Animator animator = puzzleTiles[x, y].GetComponent<Animator>();
                if (animator) {
                    animator.Play(puzzleTiles[x, y].clearAnimation.name);
                }
            }
            yield return new WaitForSeconds(1f);
            RemoveMatches();
            RefillBoard();
        }
    }

    /**
     * Checks to see if matches exist on the board by checking 
     * puzzles pieces, starting from the bottom left corner.
     */
    public bool CheckMatchesExist() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (i >= 2 && puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite != null &&
                    puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[(i - 1), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite &&
                    puzzleTiles[(i - 1), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[(i - 2), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite) {
                    return true;
                }
                if (j >= 2 && puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite != null &&
                    puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[i, (j - 1)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite &&
                    puzzleTiles[i, (j - 1)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[i, (j - 2)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite) {
                    return true;
                }

            }
        }
        return false;
    }

    /**
     * Finds all the matching tiles.
     */
    public void FindMatchThrees() {
        indexes = new List<int>();

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (i >= 2 && puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite != null &&
                    puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[(i-1), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite &&
                    puzzleTiles[(i-1), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[(i-2), j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite) {
                    indexes.Add(j * 8 + i);
                    indexes.Add(j * 8 + (i-1));
                    indexes.Add(j * 8 + (i-2));
                    if (puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite.name.Equals("SpritePuzzle_Critical") || puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite.name.Equals("SpritePuzzle_Poison")) {
                        storedCharacterHealth.RuntimeValue--;
                    }
                    Debug.Log(puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite);
                }
                if (j >= 2 && puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite != null &&
                    puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[i, (j-1)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite &&
                    puzzleTiles[i, (j-1)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == puzzleTiles[i, (j-2)].transform.Find("piece").GetComponent<SpriteRenderer>().sprite) {
                    indexes.Add(j * 8 + i);
                    indexes.Add((j-1) * 8 + i);
                    indexes.Add((j-2) * 8 + i);

                    if (puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite.name.Equals("SpritePuzzle_Critical") || puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite.name.Equals("SpritePuzzle_Poison")) {
                        storedCharacterHealth.RuntimeValue--;
                    }
                    Debug.Log(puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>());
                }

            }
        }
    }

    /**
     * Removes the sprites in the matching tiles.
     */
    public void RemoveMatches() {
        foreach (int i in indexes) {
            int x = i % 8;
            int y = i / 8;
            
            puzzleTiles[x, y].transform.Find("piece").GetComponent<SpriteRenderer>().sprite = null;
        }
        foreach (int i in indexes) {
            int x = i % 8;
            int y = i / 8;
            Animator animator = puzzleTiles[x, y].GetComponent<Animator>();
            animator.Play(puzzleTiles[x, y].clearAnimationReverse.name);
        }
    }

    /**
     * Refills all the tiles where needed.
     */
    public void RefillBoard() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                //Finds the newly destroyed puzzle tile
                if (puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == null) {
                    int tempH = j + 1;


                    //Look for the closest tile in the y axis from the newly destroyed puzzle tile that is not null.
                    while (tempH != 8 && puzzleTiles[i, tempH].transform.Find("piece").GetComponent<SpriteRenderer>().sprite == null) {
                        tempH++;
                    }
                    //If there is none, then we know that we have to fill it with a new tile
                    if (tempH == 8) {
                        puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite = puzzleSprites[Random.Range(0, puzzleSprites.Length)];
                        //puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                    else {
                        puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().sprite = puzzleTiles[i, tempH].transform.Find("piece").GetComponent<SpriteRenderer>().sprite;
                        //puzzleTiles[i, j].transform.Find("piece").GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        puzzleTiles[i, tempH].transform.Find("piece").GetComponent<SpriteRenderer>().sprite = null;
                    }
                }
            }
        }
    }

    public void AddCount() {
        count += 1;
    }

    public int GetCount() {
        return count;
    }
}
