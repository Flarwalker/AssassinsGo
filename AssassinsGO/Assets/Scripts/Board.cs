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

}
