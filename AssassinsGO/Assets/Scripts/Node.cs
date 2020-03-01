using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
  // Links to game Objects Prefabs, Models and layers
  public GameObject geometry;
  public GameObject linkPrefab;
  public LayerMask obstacleLayer;

  // Link Class Variables
  private Board m_board;
  private bool m_isInitialized = false;

  private Vector2 m_coordinate;
  public Vector2 Coordinate { get { return Utility.Vector2Round(m_coordinate); } }

  // Node Lists for Telling Neighbor and Linked Nodes
  private List<Node> m_neighorNodes = new List<Node>();
  public List<Node> NeighorNodes { get { return m_neighorNodes; } }

  private List<Node> m_linkedNodes = new List<Node>();
  public List<Node> LinkedNodes { get { return m_linkedNodes; } }


  // iTween Settings
  public float delay = 1f;
  public float scaleTime = 0.3f;
  public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

  public bool isLevelGoal = false;
  
  private void Awake () {
    m_board = Object.FindObjectOfType<Board>();
    m_coordinate = new Vector2(transform.position.x, transform.position.z);
  }

  // Start is called before the first frame update
  void Start() {
    if (geometry != null) {
      geometry.transform.localScale = Vector3.zero;

      if (m_board != null) {
        m_neighorNodes = FindNeighbors(m_board.AllNodes);
      }
    }
  }

  // Handles the Animation of the Node Popping up
  public void ShowGeometry () {
    if (geometry != null) {
      iTween.ScaleTo(geometry, iTween.Hash(
        "time", scaleTime,
        "scale", Vector3.one,
        "easetype", easeType,
        "delay", delay
      ));
    }
  }

  // Finds the List of Neighboring Nodes
  public List<Node> FindNeighbors(List<Node> nodes) {
    List<Node> nList = new List<Node>();
    foreach (Vector2 dir in Board.directions) {
      Node foundNeighbor = nodes.Find(n => n.Coordinate == Coordinate + dir);
      if (foundNeighbor != null && !nList.Contains(foundNeighbor)) {
        nList.Add(foundNeighbor);
      }
    }
    return nList;
  }

  // Inits This node
  public void InitNode  () {
    if (!m_isInitialized) {
      m_isInitialized = true;
      ShowGeometry();
      InitNeighbors();
    }
  }

  // Starts the Coroutine to Init all Neighbor Nodes
  private void InitNeighbors () {
    StartCoroutine(InitNeighborRoutine());
  }

  // Inits the Neighboring Nodes and Draws the links to them
  private IEnumerator InitNeighborRoutine () {
    yield return new WaitForSeconds(delay);
    foreach (Node n in m_neighorNodes) {
      if (!m_linkedNodes.Contains(n)) {
        Obstacle obstacle = FindObstacle(n);
        if (obstacle == null) {
          LinkNode(n);
          n.InitNode();
        }
      }
    }
  }

  // Draws the link between 2 Nodes
  private void LinkNode (Node targetNode) {
    if (linkPrefab != null) {
      GameObject linkInstance = Instantiate(linkPrefab, transform.position, Quaternion.identity);
      linkInstance.transform.parent = transform;

      Link link = linkInstance.GetComponent<Link>();
      if (link != null) {
        link.DrawLink(transform.position, targetNode.transform.position);
      }
      if (!m_linkedNodes.Contains(targetNode)) {
        m_linkedNodes.Add(targetNode);
      }

      if (!targetNode.LinkedNodes.Contains(this)) {
        targetNode.LinkedNodes.Add(this);
      }
    }
  }

  private Obstacle FindObstacle(Node targetNode) {
    Vector3 checkDirection = targetNode.transform.position - this.transform.position;
    RaycastHit raycastHit;

    if (Physics.Raycast(transform.position, checkDirection, out raycastHit, Board.spacing + 0.1f, obstacleLayer)) {
      return raycastHit.collider.GetComponent<Obstacle>();
    }

    return null;
  }

}
