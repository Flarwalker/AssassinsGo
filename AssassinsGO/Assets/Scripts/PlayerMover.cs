using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {
  // Reference to the game board
  public Board m_board;

  // Is the Player currently moving?
  public bool isMoving = false;

  // Player's destination Location
  public Vector3 destination;

  // iTween Variables
  public float iTweenDelay = 0f;
  public float moveSpeed = 1.5f;
  public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;

  private void Awake () {
    m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
  }

  private void Start () {
    UpdateBoard();
  }

  // Moves the Player
  public void Move (Vector3 destinationPos, float delayTime = 0.25f) {
    if (m_board != null) {
      Node targetNode = m_board.FindNodeAt(destinationPos);

      if (targetNode != null && m_board.PlayerNode.LinkedNodes.Contains(targetNode)) {
        StartCoroutine(MoveRoutine(destinationPos, delayTime));
      }
    }
  }

  // Start and stop the Movement animation
  private IEnumerator MoveRoutine (Vector3 destinationPos, float delayTime) {
    isMoving = true;
    destination = destinationPos;
    yield return new WaitForSeconds(delayTime);
    iTween.MoveTo(this.gameObject, iTween.Hash(
      "x", destinationPos.x,
      "y", destinationPos.y,
      "z", destinationPos.z,
      "delay", iTweenDelay,
      "easetype", easeType,
      "speed", moveSpeed
    ));

    while(Vector3.Distance(destinationPos, transform.position) > 0.01f) {
      yield return null;
    }

    iTween.Stop(this.gameObject);
    this.transform.position = destinationPos;
    isMoving = false;
    UpdateBoard();
  }

  // Move the Player Right 2 Units
  public void MoveRight () {
    Vector3 newPostition = this.transform.position + new Vector3(-Board.spacing, 0f, 0f);
    Move(newPostition, 0);
  }

  // Move the Player Left 2 Units
  public void MoveLeft () {
    Vector3 newPostition = this.transform.position + new Vector3(Board.spacing, 0f, 0f);
    Move(newPostition, 0);
  }

  // Move the Player Forward 2 Units
  public void MoveForward () {
    Vector3 newPostition = this.transform.position + new Vector3(0f, 0f, Board.spacing);
    Move(newPostition, 0);
  }

  // Move the Player Backwards 2 Units
  public void MoveBackward () {
    Vector3 newPostition = this.transform.position + new Vector3(0f, 0f, -Board.spacing);
    Move(newPostition, 0);
  }

  private void UpdateBoard () {
    if (m_board != null) {
      m_board.UpdatePlayerNode();
    }
  }
}
