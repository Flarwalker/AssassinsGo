﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public enum Turn {
  Player,
  Enemy
}

public class GameManager : MonoBehaviour {
  private Board m_board;
  private PlayerManager m_player;

  private List<EnemyManager> m_enemies;

  private Turn m_currentTurn = Turn.Player;
  public Turn CurrentTurn { get { return m_currentTurn; } }

  // State Booleans
  private bool m_hasLevelStarted = false;
  public bool HasLevelStarted { get => m_hasLevelStarted; set => m_hasLevelStarted = value; }

  private bool m_isGamePlaying = false;
  public bool IsGamePlaying { get => m_isGamePlaying; set => m_isGamePlaying = value; }

  private bool m_isGameOver = false;
  public bool IsGameOver { get => m_isGameOver; set => m_isGameOver = value; }

  private bool m_hasLevelFinished = false;
  public bool HasLevelFinished { get => m_hasLevelFinished; set => m_hasLevelFinished = value; }

  public float delay = 1f;

  public UnityEvent setUpEvent;
  public UnityEvent startLevelEvent;
  public UnityEvent playLevelEvent;
  public UnityEvent endLevelEvent;
  public UnityEvent LoseLevelEvent;

  private void Awake () {
    m_board = GameObject.FindObjectOfType<Board>().GetComponent<Board>();
    m_player = GameObject.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    EnemyManager[] enemies = GameObject.FindObjectsOfType<EnemyManager>() as EnemyManager[];
    m_enemies = enemies.ToList<EnemyManager>();
  }

  // Start is called before the first frame update
  void Start() {
    if (m_player != null && m_board != null) {
      StartCoroutine("RunGameLoop");
    } else {
      Debug.LogWarning("GAMEMANGER Error: no player or board found!");
    }
  }

  private IEnumerator RunGameLoop() {
    yield return StartCoroutine("StartLevelRoutine");
    yield return StartCoroutine("PlayLevelRoutine");
    yield return StartCoroutine("EndLevelRoutine");
  }

  private IEnumerator StartLevelRoutine () {
    if (setUpEvent != null) {
      setUpEvent.Invoke();
    }

    m_player.playerInput.InputEnabled = false;

    while (!m_hasLevelStarted) {
      yield return null;
    }

    if (startLevelEvent !=null) {
      startLevelEvent.Invoke();
    }
  }
  
  private IEnumerator PlayLevelRoutine () {
    m_isGamePlaying = true;
    yield return new WaitForSeconds(delay);
    m_player.playerInput.InputEnabled = true;

    if (playLevelEvent != null) {
      playLevelEvent.Invoke();
    }

    while(!m_isGameOver) {
      yield return null;
      m_isGameOver = IsWinner();
    }

  }

  public void LoseLevel() {
    StartCoroutine(LoseLevelRoutine());
  }

  private IEnumerator LoseLevelRoutine() {
    m_isGameOver = true;

    if (LoseLevelEvent != null) {
      LoseLevelEvent.Invoke();
    }

    yield return new WaitForSeconds(2f);

    Debug.Log("LOSE! ==================================");
  }
  
  private IEnumerator EndLevelRoutine () {

    m_player.playerInput.InputEnabled = false;

    if (endLevelEvent != null) {
      endLevelEvent.Invoke();
    }

    while (!m_hasLevelFinished) {
      yield return null;
    }

    RestartLevel();
  }

  private void RestartLevel () {
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }

  public void PlayLevel() {
    m_hasLevelStarted = true;
  }

  private bool IsWinner() {
    if (m_board.PlayerNode != null) {
      return (m_board.PlayerNode == m_board.GoalNode);
    }
    return false;
  }

  public void UpdateTurn() {
    if (m_currentTurn == Turn.Player && m_player != null) {
      if (m_player.IsTurnComplete) {
        PlayEnemyTurn();
      }
    } else if (m_currentTurn == Turn.Enemy) {
      if (IsEnemyTurnComplete()) {
        PlayPlayerTurn();
      }
    }
  }

  private void PlayPlayerTurn() {
    m_currentTurn = Turn.Player;
    m_player.IsTurnComplete = false;
  }

  private void PlayEnemyTurn() {
    m_currentTurn = Turn.Enemy;

    foreach (EnemyManager enemy in m_enemies) {
      if (enemy != null) {
        enemy.IsTurnComplete = false;
        enemy.PlayTurn();
      }
    }
  }

  private bool IsEnemyTurnComplete() {
    foreach (EnemyManager enemy in m_enemies) {
      if (!enemy.IsTurnComplete) {
        return false;
      }
    }

    return true;
  }

}
