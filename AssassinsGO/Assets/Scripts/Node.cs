using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
  private Vector2 m_coordinate;
  public Vector2 Coordinate { get { return Utility.Vector2Round(m_coordinate); } }

  private List<Node> m_neighorNodes = new List<Node>();
  public List<Node> NeighorNodes { get { return m_neighorNodes; } }

  private List<Node> m_linkedNodes = new List<Node>();
  public List<Node> LinkedNodes { get { return m_linkedNodes; } }

  private Board m_board;

  public GameObject geometry;

  public float scaleTime = 0.3f;

  public iTween.EaseType easeType = iTween.EaseType.easeInExpo;

  public bool autoRun;

  private bool m_isInitialized = false;

  public float delay = 1f;

  public GameObject linkPrefab;

  private void Awake () {
    m_board = Object.FindObjectOfType<Board>();
    m_coordinate = new Vector2(transform.position.x, transform.position.z);
  }

  // Start is called before the first frame update
  void Start() {
    if (geometry != null) {
      geometry.transform.localScale = Vector3.zero;

      if (autoRun) {
        InitNode();
      }

      if (m_board != null) {
        m_neighorNodes = FindNeighbors(m_board.AllNodes);
      }
    }
  }

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

  public void InitNode  () {
    if (!m_isInitialized) {
      m_isInitialized = true;
      ShowGeometry();
      InitNeighbors();
    }
  }

  private void InitNeighbors () {
    StartCoroutine(InitNeighborRoutine());
  }

  private IEnumerator InitNeighborRoutine () {
    yield return new WaitForSeconds(delay);
    foreach (Node n in m_neighorNodes) {
      if (!m_linkedNodes.Contains(n)) {
        LinkNode(n);
        n.InitNode();
      }
    }
  }

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

}
