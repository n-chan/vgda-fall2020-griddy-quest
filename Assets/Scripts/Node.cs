using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector2 worldPosition;

    public int gCost;
    public int hCost;
    public int fCost;

    public int gridX;
    public int gridY;

    public Node parent;

    public Node(bool w, int gx, int gy) {
        walkable = w;
        gridX = gx;
        gridY = gy;
    }

    public int getFCost() {
        return gCost + hCost;
    }

    public int getGridX() {
        return gridX;
    }
    public int getGridY() {
        return gridY;
    }

    
}
