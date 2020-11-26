using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int score;

    private int x;
    private int y;
    private GridTwo.PieceType pieceType;
    private GridTwo grid;
    private MoveablePiece moveableComponent;
    private FruitPiece fruitComponent;
    private ClearablePiece clearableComponent;
    public int GetX() {
        return x;
    }

    public void SetX(int newX) {
        if (IsMovable()) {
            x = newX;
        }
    }

    public int GetY() {
        return y;
    }

    public void SetY(int newY) {
        if (IsMovable()) {
            y = newY;
        }
    }

    public GridTwo.PieceType GetPieceType() {
        return pieceType;
    }

    public GridTwo GetGrid() {
        return grid;
    }

    public MoveablePiece GetMoveableComponent() {
        return moveableComponent;
    }

    public FruitPiece GetFruitComponent() {
        return fruitComponent;
    }

    public ClearablePiece GetClearableComponent() {
        return clearableComponent;
    }

    void Awake() {
        moveableComponent = GetComponent<MoveablePiece>();
        fruitComponent = GetComponent<FruitPiece>();
        clearableComponent = GetComponent<ClearablePiece>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int x, int y, GridTwo grid, GridTwo.PieceType pieceType) {
        this.x = x;
        this.y = y;
        this.grid = grid;
        this.pieceType = pieceType;
    }

    public bool IsMovable() {
        return moveableComponent != null;
    }
    public bool IsFruit() {
        return fruitComponent != null;
    }

    void OnMouseEnter() {
        grid.EnterPiece(this);
    }

    void OnMouseDown() {
        grid.PressPiece(this);
    }

    void OnMouseUp() {
        grid.ReleasePiece();
    }

    public bool IsClearable() {
        return clearableComponent != null;
    }
}
