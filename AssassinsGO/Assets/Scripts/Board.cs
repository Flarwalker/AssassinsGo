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

  // Player Node
  private Node m_playerNode;
  public Node PlayerNode { get { return m_playerNode; } }

  // the Node representing the end of the maze
  private Node m_goalNode;
  public Node GoalNode { get { return m_goalNode; } }

  // iTween parameters for drawing the goal
  public GameObject goalPrefab;
  public float drawGoalTime = 2f;
  public float drawGoalDelay = 2f;
  public iTween.EaseType drawGoalEaseType = iTween.EaseType.easeOutExpo;

  private PlayerMover m_player;

  // Init Function gets all nodes
  private void Awake () {
    m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
    GetNodeList();
    m_goalNode = FindGoalNode();
  }

  // Finds All Nodes in the Hierarchy
  public void GetNodeList () {
    Node[] nList = GameObject.FindObjectsOfType<Node>();
    m_allNodes = new List<Node>(nList);
  }

  // Find a Node at any giving location
  public Node FindNodeAt (Vector3 pos) {
    Vector2 boardCoord = Utility.Vector2Round(new Vector2(pos.x, pos.z));
    return m_allNodes.Find(n => n.Coordinate == boardCoord);
  }

  private Node FindGoalNode () {
    return m_allNodes.Find(n => n.isLevelGoal);
  }

  public Node FindPlayerNode () {
    if (m_player != null && !m_player.isMoving) {
      return FindNodeAt(m_player.transform.position);
    }
    return null;
  }

  public void UpdatePlayerNode () {
    m_playerNode = FindPlayerNode();
  }

  private void OnDrawGizmos () {
    Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
    if (m_playerNode != null) {
      Gizmos.DrawSphere(m_playerNode.transform.position, 0.2f);
    }
  }

  public void DrawGoal () {
    if (goalPrefab != null && m_goalNode != null) {
      GameObject goalInstance = Instantiate(goalPrefab, m_goalNode.transform.position, Quaternion.identity);

      iTween.ScaleFrom(goalInstance, iTween.Hash(
        "scale", Vector3.zero,
        "time", drawGoalTime,
        "delay", drawGoalTime,
        "easeType", drawGoalEaseType
      ));
    }
  }

  public void InitBoard() {
    if (m_playerNode != null) {
      m_playerNode.InitNode();
    }
  }

}
