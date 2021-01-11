using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridTwo : MonoBehaviour
{
    public bool isClearing = false;
    private bool gameOver = false;
    public IntValue storedCharacterNum;

    public enum PieceType {
        EMPTY,
        NORMAL,
        BLOCK,
        ROW_CLEAR,
        COLUMN_CLEAR,
        COUNT,
    }

    [System.Serializable]
    public struct PiecePrefab {
        public PieceType type;
        public GameObject prefab;
    }

    public PiecePrefab[] piecePrefabs;
    public GameObject backgroundPrefab;
    private Dictionary<PieceType, GameObject> piecePrefabDict;

    public int xDimension;
    public int yDimension;
    public float fillTime;
    public Level level;

    private GamePiece[,] pieces;

    private GamePiece pressedPiece;
    private GamePiece enteredPiece;

    public AudioSource audioSrc;
    public AudioClip clearSound, swapSound;

    // Start is called before the first frame update
    void Start() {
        piecePrefabDict = new Dictionary<PieceType, GameObject>();
        pieces = new GamePiece[xDimension, yDimension];

        for (int i = 0; i < piecePrefabs.Length; i++) {
            if (!piecePrefabDict.ContainsKey (piecePrefabs[i].type)) {
                piecePrefabDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
            }
        }

        for (int x = 0; x < xDimension; x++) {
            for (int y = 0; y < yDimension; y++) {
                GameObject background = (GameObject)Instantiate(backgroundPrefab, GetWorldPosition(x, y), Quaternion.identity);
                background.transform.parent = transform;
                SpawnNewPiece(x, y, PieceType.EMPTY);
            }
        }
        
        StartCoroutine(Fill());
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(transform.position.x - xDimension / 2.3f + x, transform.position.y + yDimension / 2.5f - y, 10);
    }

    public GamePiece SpawnNewPiece(int x, int y, PieceType pieceType) {
        GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[pieceType], GetWorldPosition(x, y), Quaternion.identity);
        newPiece.transform.parent = transform;

        pieces[x, y] = newPiece.GetComponent<GamePiece>();
        pieces[x, y].Init(x, y, this, pieceType);

        return pieces[x, y];
    }

    /// <summary>
    /// Fills the board with tiles.
    /// </summary>
    public IEnumerator Fill() {
        isClearing = true;
        bool needsRefill = true;
        while(needsRefill) {
            yield return new WaitForSeconds(fillTime + 0.5f);
            int lvl = 0;
            while (FillStep(lvl)) {
                lvl += 1;
                yield return new WaitForSeconds(fillTime);
            }

            int randNum = Random.Range(1, 5);

            //Depending on random number, spawns an row clear or column clear tile randomly on the grid.
            if (storedCharacterNum.RuntimeValue == 0 && randNum == 1) {
                int spawnAtX = Random.Range(0, 8);
                int spawnAtY = Random.Range(0, 8);

                GamePiece temp = pieces[spawnAtX, spawnAtY];
                Destroy(pieces[spawnAtX, spawnAtY].gameObject);
                randNum = Random.Range(0, 2);
                GamePiece newPiece = SpawnNewPiece(spawnAtX, spawnAtY, (randNum == 0) ? PieceType.ROW_CLEAR : PieceType.COLUMN_CLEAR);
                newPiece.GetFruitComponent().SetFruit(temp.GetFruitComponent().GetFruitType());
                needsRefill = ClearAllValidMatches();
            }
            else {
                needsRefill = ClearAllValidMatches();
            }
        }
        isClearing = false;
        /*
        I could do something cool with this snippet of code:
        Destroy(pieces[4, 4].gameObject);
        GamePiece newPiece = SpawnNewPiece(4, 4, PieceType.ROW_CLEAR);
        newPiece.GetFruitComponent().SetFruit(pieces[0, 2].GetFruitComponent().GetFruitType());
        */

        level.OnMove();
    }

    public bool FillStep(int lvl) {
        bool movedPiece = false;
        for (int y = yDimension - 2; y >= 0; y--) {
            for (int x = 0; x < xDimension; x++) {
                GamePiece piece = pieces[x, y];
                if (piece.IsMovable()) {
                    GamePiece pieceBelow = pieces[x, y + 1];
                    if (pieceBelow.GetPieceType() == PieceType.EMPTY) {
                        Destroy(pieceBelow.gameObject);
                        piece.GetMoveableComponent().Move(x, y + 1, fillTime);
                        piece.name = "Piece(" + x + ", " + y + ")";
                        pieces[x, y + 1] = piece;
                        SpawnNewPiece(x, y, PieceType.EMPTY);
                        movedPiece = true;
                    }
                }
            }
        }

        for (int x = 0; x < xDimension; x++) {
            GamePiece pieceBelow = pieces[x, 0];
            if (pieceBelow.GetPieceType() == PieceType.EMPTY) {
                Destroy(pieceBelow.gameObject);
                GameObject newPiece = (GameObject)Instantiate(piecePrefabDict[PieceType.NORMAL], GetWorldPosition(x, -1), Quaternion.identity);
                newPiece.transform.parent = transform;

                pieces[x, 0] = newPiece.GetComponent<GamePiece>();
                pieces[x, 0].Init(x, -1, this, PieceType.NORMAL);
                pieces[x, 0].GetMoveableComponent().Move(x, 0, fillTime);
                
                //The following makes sure there is no match in the initial board.
                do {
                    pieces[x, 0].GetFruitComponent().SetFruit((FruitPiece.FruitType)Random.Range(0, pieces[x, 0].GetFruitComponent().GetNumFruits()));

                } while (
                    (x >= 2 && pieces[x, 0].GetFruitComponent().GetFruitType() == pieces[x - 1, 0].GetFruitComponent().GetFruitType()
                            && pieces[x, 0].GetFruitComponent().GetFruitType() == pieces[x - 2, 0].GetFruitComponent().GetFruitType()) ||

                    (lvl >= 2 && pieces[x, 0].GetFruitComponent().GetFruitType() == pieces[x, 1].GetFruitComponent().GetFruitType()
                            && pieces[x, 0].GetFruitComponent().GetFruitType() == pieces[x, 2].GetFruitComponent().GetFruitType()));
                movedPiece = true;
            }
        }
        return movedPiece;
    }

    public bool IsAdjacent(GamePiece piece1, GamePiece piece2) {
        return (piece1.GetX() == piece2.GetX() && (int)Mathf.Abs(piece1.GetY() - piece2.GetY()) == 1) ||
            (piece1.GetY() == piece2.GetY() && (int)Mathf.Abs(piece1.GetX() - piece2.GetX()) == 1);
    }

    public void SwapPieces(GamePiece piece1, GamePiece piece2) {
        if (gameOver) {
            return;
        }

        if (piece1.IsMovable() && piece2.IsMovable()) {
            pieces[piece1.GetX(), piece1.GetY()] = piece2;
            pieces[piece2.GetX(), piece2.GetY()] = piece1;

            if (CheckIfMatchesExist()) {
                int piece1X = piece1.GetX();
                int piece1Y = piece1.GetY();

                piece1.GetMoveableComponent().Move(piece2.GetX(), piece2.GetY(), fillTime);
                piece2.GetMoveableComponent().Move(piece1X, piece1Y, fillTime);

                ClearAllValidMatches();

                pressedPiece = null;
                enteredPiece = null;

                StartCoroutine(Fill());
            }
            else {
                pieces[piece1.GetX(), piece1.GetY()] = piece1;
                pieces[piece2.GetX(), piece2.GetY()] = piece2;
            }
        }
        
    }

    public void PressPiece(GamePiece piece) {
        pressedPiece = piece;
    }

    public void EnterPiece(GamePiece piece) {
        enteredPiece = piece;

    }

    public void ReleasePiece() {
        if (!isClearing && IsAdjacent(pressedPiece, enteredPiece)) {
            SwapPieces(pressedPiece, enteredPiece);
            audioSrc.PlayOneShot(swapSound);
        }   
    }

    /// <summary>
    /// Checks for any matches in the grid by comparing adjacent tiles with one another.
    /// Once one is found, return true. If there are none, return false.
    /// </summary>
    public bool CheckIfMatchesExist() {
        for (int y = 0; y < yDimension; y++) {
            for (int x = 0; x < xDimension; x++) {
                if (x >= 2 && pieces[x, y].GetFruitComponent().GetFruitType() == pieces[(x - 1), y].GetFruitComponent().GetFruitType() &&
                    pieces[(x - 1), y].GetFruitComponent().GetFruitType() == pieces[(x - 2), y].GetFruitComponent().GetFruitType()) {
                    return true;
                }
                if (y >= 2 && pieces[x, y].GetFruitComponent().GetFruitType() == pieces[x, (y - 1)].GetFruitComponent().GetFruitType() &&
                    pieces[x, (y - 1)].GetFruitComponent().GetFruitType() == pieces[x, (y - 2)].GetFruitComponent().GetFruitType()) {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Finds any matches in the grid by comparing adjacent tiles with one another.
    /// Stores them all in a list, that is then returned.
    /// </summary>
    public List<GamePiece> FindMatchThrees() {
        List<GamePiece> piecesToRemove = new List<GamePiece>();

        for (int y = 0; y < yDimension; y++) {
            for (int x = 0; x < xDimension; x++) {
                if (x >= 2 && pieces[x, y].GetFruitComponent().GetFruitType() == pieces[(x - 1), y].GetFruitComponent().GetFruitType() &&
                    pieces[(x - 1), y].GetFruitComponent().GetFruitType() == pieces[(x - 2), y].GetFruitComponent().GetFruitType()) {
                    piecesToRemove.Add(pieces[x, y]);
                    piecesToRemove.Add(pieces[(x-1), y]);
                    piecesToRemove.Add(pieces[(x-2), y]);
                    audioSrc.PlayOneShot(clearSound);
                }
                if (y >= 2 && pieces[x, y].GetFruitComponent().GetFruitType() == pieces[x, (y - 1)].GetFruitComponent().GetFruitType() &&
                    pieces[x, (y - 1)].GetFruitComponent().GetFruitType() == pieces[x, (y - 2)].GetFruitComponent().GetFruitType()) {
                    piecesToRemove.Add(pieces[x, y]);
                    piecesToRemove.Add(pieces[x, (y-1)]);
                    piecesToRemove.Add(pieces[x, (y-2)]);
                    audioSrc.PlayOneShot(clearSound);
                }
            }
        }
        return piecesToRemove;
    }

    /// <summary>
    /// Clears any matches on the board.
    /// </summary>
    public bool ClearAllValidMatches() {
        bool needsRefill = false;
        //Calls to FindMatchThrees to check if there are still any matches. Stores them in a list.
        List<GamePiece> match = FindMatchThrees();

        //Call to ClearPiece to clear the pieces stored in list, match.
        if (match != null) {
            for (int i = 0; i < match.Count; i++) {
                if (ClearPiece(match[i].GetX(), match[i].GetY())) {
                    needsRefill = true;
                }
            }
        }
        return needsRefill;
    }

    public bool ClearPiece(int x, int y) {
        if (pieces[x, y].IsClearable() && !pieces[x, y].GetClearableComponent().GetIsBeingCleared()) {
            pieces[x, y].GetClearableComponent().Clear();
            SpawnNewPiece(x, y, PieceType.EMPTY);

            return true;
        }
        return false;
    }

    /// <summary>
    /// Clears a whole row of tiles on the board.
    /// </summary>
    /// <param name="row">The row to be cleared</param>
    public void ClearRow(int row) {
        for (int x = 0; x < xDimension; x++) {
            ClearPiece(x, row);
        }
    }

    /// <summary>
    /// Clears a whole column of tiles on the board.
    /// </summary>
    /// <param name="col">The column to be cleared</param>
    public void ClearColumn(int col) {
        for (int y = 0; y < yDimension; y++) {
            ClearPiece(col, y);
        }
    }

    public void GameOver() {
        gameOver = true;
    }
}