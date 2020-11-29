using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLinePiece : ClearablePiece
{
    public bool isRow;

    public override void Clear() {
        base.Clear();
        if (isRow) {
            piece.GetGrid().ClearRow(piece.GetY());
        }
        else {
            piece.GetGrid().ClearColumn(piece.GetX());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
