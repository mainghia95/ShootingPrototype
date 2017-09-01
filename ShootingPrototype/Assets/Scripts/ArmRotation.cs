using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

	public int rotationOffset = 90;
	public Vector2 moveDir;

	// Update is called once per frame
	void Update () {		
		moveDir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
		moveDir.Normalize();
		float rotZ = Mathf.Atan2 (moveDir.y, moveDir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + rotationOffset);
	}
}
