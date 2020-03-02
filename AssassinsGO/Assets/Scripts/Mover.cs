using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour {
  // Is the Player currently moving?
  public bool isMoving = false;
  public bool faceDestination = false;

  // Reference to the game board
  protected Board m_board;

  // Player's destination Location
  public Vector3 destination;

  // iTween Variables
  public float iTweenDelay = 0f;
  public float moveSpeed = 1.5f;
  public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
  public float rotateTime = 0.5f;

  protected Node m_currentNode;

  public UnityEvent finishMovementEvent;

  protected virtual void Awake () {
    m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
  }

  protected virtual void Start () {
    UpdateCurrentNode();
  }

  // Moves the Player
  public void Move (Vector3 destinationPos, float delayTime = 0.25f) {
    if (isMoving) {
      return;
    }

    if (m_board != null) {
      Node targetNode = m_board.FindNodeAt(destinationPos);

      if (targetNode != null && m_currentNode != null) {
        if (m_currentNode.LinkedNodes.Contains(targetNode)) {
          StartCoroutine(MoveRoutine(destinationPos, delayTime));
        } else {
          Debug.Log("MOVER: " + m_currentNode.name + " not connected " + targetNode.name);
        }
      }
    }
  }

  // Start and stop the Movement animation
  protected virtual IEnumerator MoveRoutine (Vector3 destinationPos, float delayTime) {
    isMoving = true;
    destination = destinationPos;

    if (faceDestination) {
      FaceDestination();
      yield return new WaitForSeconds(0.25f);
    }

    yield return new WaitForSeconds(delayTime);
    iTween.MoveTo(this.gameObject, iTween.Hash(
      "x", destinationPos.x,
      "y", destinationPos.y,
      "z", destinationPos.z,
      "delay", iTweenDelay,
      "easetype", easeType,
      "speed", moveSpeed
    ));

    while (Vector3.Distance(destinationPos, transform.position) > 0.01f) {
      yield return null;
    }

    iTween.Stop(this.gameObject);
    this.transform.position = destinationPos;
    isMoving = false;
    UpdateCurrentNode();
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

  protected void UpdateCurrentNode() {
    if (m_board != null) {
      m_currentNode = m_board.FindNodeAt(transform.position);
    }
  }

  private void FaceDestination() {
    Vector3 relativePosition = destination = transform.position;
    Quaternion newRotation = Quaternion.LookRotation(relativePosition, Vector3.up);

    float newY = newRotation.eulerAngles.y;

    iTween.RotateTo(gameObject, iTween.Hash(
      "y", newY,
      "delay", 0f,
      "easetype", easeType,
      "time", rotateTime
    ));
  }
}
