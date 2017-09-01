using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ladder : MonoBehaviour
{
	public string Vertical;
	public string TriggerLadderButton;
	public float ladderSpeed = 1f;
	public float resistanceForce = 1f;


	void OnTriggerStay2D (Collider2D other)
	{
		if (other.tag == "Player") {
			if (other.gameObject.GetComponent<InputManager> ().player.GetButtonDown ("TriggerButton")) {
				InputManager.Instance.canMove = false;
			}
			else if (other.gameObject.GetComponent<InputManager> ().player.GetButtonDown ("Jump")) {
				InputManager.Instance.canMove = true;
				other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
			}

			if (!InputManager.Instance.canMove) {
				if (other.gameObject.GetComponent<InputManager> ().player.GetAxisRaw (Vertical) > 0) {
					other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, ladderSpeed);
				} else if (other.gameObject.GetComponent<InputManager> ().player.GetAxisRaw (Vertical) < 0) {					
					other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, -ladderSpeed);
				} else {
					other.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, resistanceForce);				
				}
			}
		}
	}
}

