using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
  // Acces Axis for Movement
  private float m_h;
  public float H { get { return m_h; } }

  private float m_v;
  public float V { get { return m_v; } }

  // Boolean for if input is Enabled
  private bool m_inputEnabled = false;
  public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }

  // Gets the current input from the axises
  public void GetKeyInput () {
    if (m_inputEnabled) {
      m_h = Input.GetAxisRaw("Horizontal");
      m_v = Input.GetAxisRaw("Vertical");
    } else {
      m_h = 0f;
      m_v = 0f;
    }
  }
}
