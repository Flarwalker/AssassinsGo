using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : TurnManager {
  private Board m_board;
  private EnemyMover m_enemyMover;
  private EnemySensor m_enemySenor;

  protected override void Awake () {
    base.Awake();
    m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    m_enemyMover = GetComponent<EnemyMover>();
    m_enemySenor = GetComponent<EnemySensor>();
  }

  public void PlayTurn () {
    StartCoroutine(PlayTurnRoutine());
  }

  private IEnumerator PlayTurnRoutine() {
    if (m_gameManager != null && m_gameManager.IsGameOver) {
      m_enemySenor.UpdateSensor();

      yield return new WaitForSeconds(0f);

      if (m_enemySenor.FoundPlayer) {
        //attack

        m_gameManager.LoseLevel();
      } else {
        m_enemyMover.MoveOneTurn();
      }
    }
  }
}
