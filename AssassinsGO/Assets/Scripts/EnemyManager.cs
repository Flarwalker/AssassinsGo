using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(EnemySensor))]
public class EnemyManager : MonoBehaviour {
  private Board m_board;
  private EnemyMover m_enemyMover;
  private EnemySensor m_enemySenor;

  private void Awake () {
    m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    m_enemyMover = GetComponent<EnemyMover>();
    m_enemySenor = GetComponent<EnemySensor>();
  }
}
