using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : TurnManager {
  // References the the Mover and Input Classes
  public PlayerMover playerMover;
  public PlayerInput playerInput;

  // Get the Mover and Input Objects
  protected override void Awake () {
    base.Awake();
    playerMover = GetComponent<PlayerMover>();
    playerInput = GetComponent<PlayerInput>();
    playerInput.InputEnabled = true;
  }

  // Checks for Movement and Moves the Player
  void Update() {
    if (playerMover.isMoving || m_gameManager.CurrentTurn != Turn.Player) {
      return;
    }

    playerInput.GetKeyInput();

    if (playerInput.V == 0) {
      if (playerInput.H > 0) {
        playerMover.MoveLeft();
      } else if (playerInput.H < 0) {
        playerMover.MoveRight();
      }
    }

    if (playerInput.H == 0) {
      if (playerInput.V < 0) {
        playerMover.MoveBackward();
      } else if (playerInput.V > 0) {
        playerMover.MoveForward();
      }
    }
  }
}
