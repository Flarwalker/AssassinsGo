using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : Mover {
  protected override void Awake () {
    base.Awake();
    faceDestination = true;
  }

  // Start is called before the first frame update
  protected override void Start() {
    base.Start();
  }
}
