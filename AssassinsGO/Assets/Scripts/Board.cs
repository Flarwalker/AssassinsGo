using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
  // Global Spacing of all objects
  public static float spacing = 2f;

  // Spacing in Each Direction Right, Left, Forward, Backward
  public static readonly Vector2[] directions = {
    new Vector2(spacing, 0f),
    new Vector2(-spacing, 0f),
    new Vector2(0f, spacing),
    new Vector2(0f, -spacing)
  };

  // List of All nodes on the Board
  private List<Node> m_allNodes = new List<Node>();
  public List<Node> AllNodes {  get { return m_allNodes; } }

  // Init Function gets all nodes
  private void Awake () {
    GetNodeList();
  }

  // Finds All Nodes in the Hierarchy
  public void GetNodeList() {
    Node[] nList = GameObject.FindObjectsOfType<Node>();
    m_allNodes = new List<Node>(nList);
  }

  // Find a Node at any giving location
  public Node FindNodeAt(Vector3 pos) {
    Vector2 boardCoord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
    return m_allNodes.Find(n => n.Coordinate == boardCoord);
  }

}
