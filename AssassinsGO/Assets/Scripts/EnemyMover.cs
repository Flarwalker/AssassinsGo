using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover {
  public float standTime = 1f;

  protected override void Awake () {
    base.Awake();
    faceDestination = true;
  }

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
  }

  public void MoveOneTurn() {
    Stand();
  }

  private void Stand () {
    StartCoroutine(StandRoutine());
  }

  private IEnumerator StandRoutine () {
    yield return new WaitForSeconds(standTime);
    base.finishMovementEvent.Invoke();
  }
}
