using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGrid : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform start;
    public Transform target;
    public List<GameObject> paths;
    bool foundPath = false;
    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        foreach (var position in tilemap.cellBounds.allPositionsWithin) {
            if (tilemap.HasTile(position)) {
                Debug.Log("TILE: " + position);
            }
        }
        
        //Debug.Log(tilemap.HasTile(new Vector3Int(148, -42, 0)));
        
        RunAlgo();
    }


    public void RunAlgo() {
        Node startNode = new Node(true, (int)start.position.x, (int)start.position.y);
        Node targetNode = new Node(true, (int)target.position.x, (int)target.position.y);

        Debug.Log("Start: " + startNode.getGridX() + " " + startNode.getGridY());
        Debug.Log("End: " + targetNode.getGridX() + " " + targetNode.getGridY());

        startNode.fCost = 0;

        //Initialize the open list.
        List<Node> openSet = new List<Node>();

        //Initialize the closed list.
        HashSet<Node> closedSet = new HashSet<Node>();

        //Put the starting node on the open list.
        openSet.Add(startNode);

        //While open list is not empty
        while (openSet.Count > 0) {
            //Find the node with the least f
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].getFCost() < currentNode.getFCost()) {
                    currentNode = openSet[i];
                }
            }
            //Pop the node with the least f from the open list
            openSet.Remove(currentNode);

            //Generate successors.
            foreach (Node neighbor in GetNeighbors(currentNode)) {
                //Set parent to q.
                //Debug.Log("neighbor: " + neighbor.getGridX() + " " + neighbor.getGridY());
                
                //Debug.Log("parentIsNow: " + neighbor.parent.getGridX() + " " + neighbor.parent.getGridY());
                if (!neighbor.walkable) {
                    continue;
                }
                else {
                    neighbor.parent = currentNode;
                }
                
                //If goal...
                if ((neighbor.getGridX() == targetNode.getGridX() &&
                     neighbor.getGridY() == targetNode.getGridY()) || neighbor == targetNode) {
                    neighbor.parent = currentNode;
                    targetNode.parent = currentNode;
                    //RetracePath(startNode, targetNode);
                    //Debug.Log("My start node is: " + startNode.getGridX() + " " + startNode.getGridY());
                    //Debug.Log("My end node is: " + targetNode.getGridX() + " " + targetNode.getGridY());
                    //Debug.Log("My end node's parent is: " + targetNode.parent.getGridX() + " " + targetNode.parent.getGridY());
                    StartCoroutine(GoTo(RetracePath(startNode, targetNode)));
                    Debug.Log("DONE");
                    return;
                }

                neighbor.gCost = currentNode.gCost + getDistance(neighbor, currentNode);
                neighbor.hCost = getDistance(targetNode, neighbor);
                neighbor.fCost = neighbor.gCost + neighbor.hCost;

                bool skip = false;

                foreach (Node n in openSet) {
                    if (n.getGridX() == neighbor.getGridX()
                        && n.getGridY() == neighbor.getGridY()
                        && n.fCost < neighbor.fCost) {
                        skip = true;
                        break;
                    }
                }

                foreach (Node n in closedSet) {
                    if (n.getGridX() == neighbor.getGridX()
                        && n.getGridY() == neighbor.getGridY()
                        && n.fCost < neighbor.fCost) {
                        skip = true;
                        break;
                    }
                }

                if (!skip) {
                    openSet.Add(neighbor);
                }
            }
            closedSet.Add(currentNode);
        }
    }

    IEnumerator GoTo(List<Node>path) {
        int index = 0;
        foreach (Node n in path) {
            //Debug.Log(n.getGridX() + " " + n.getGridY());
            //start.position = new Vector3(n.getGridX(), n.getGridY(), 0);
            GameObject waypoint = new GameObject("wp: " + index);
            index += 1;
            waypoint.transform.position = new Vector3(n.getGridX(), n.getGridY(), 0 );
            paths.Add(waypoint);
            //start.position = Vector3.MoveTowards(start.position, targetPos, 125f * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
            //start.position = new Vector3(n.getGridX(), n.getGridY(), 0);
        }
        foundPath = true;
    }

    public List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        //Debug.Log("A:" + currentNode.parent.getGridX() + " " + endNode.parent.getGridY());
        //Debug.Log("A:" + endNode.parent.getGridX() + " " + endNode.parent.getGridY());

        while (currentNode != startNode) {
            Debug.Log("PATH: " + currentNode.getGridX() + " " + currentNode.getGridY() + "Is walkable: " + currentNode.walkable);
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
  
    public List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x==0 && y == 0) {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;
                Debug.Log(checkX + " " + checkY + "!!!Is walkable: " + !tilemap.HasTile(new Vector3Int(checkX, checkY, 0)));
                //Debug.Log(tile);
                

                Node newNode = new Node(!tilemap.HasTile(new Vector3Int(checkX, checkY, 0)), checkX, checkY);
                neighbors.Add(newNode);
            }
        }
        return neighbors;
    }

    public int getDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }
    

    // Update is called once per frame
    void Update()
    {
        //If path is found, then start moving towards the newly created waypoints.
        if (foundPath && (index < paths.Count)) {
            if ((Vector3.Distance(start.position, paths[index].transform.position)) > 0.5f) {
                start.position = Vector3.MoveTowards(start.position, paths[index].transform.position, 5f * Time.deltaTime);
            }
            else {
                index += 1;
            }
        }
    }
}

/*
 *                 if (!neighbor.walkable || closedSet.Contains(neighbor)) {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + getDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = getDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
*/