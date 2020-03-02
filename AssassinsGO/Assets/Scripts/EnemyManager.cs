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
    m_enemySenor.UpdateSensor();

    // attack

    yield return new WaitForSeconds(0.5f);

    //move
    m_enemyMover.MoveOneTurn();
  }
}
