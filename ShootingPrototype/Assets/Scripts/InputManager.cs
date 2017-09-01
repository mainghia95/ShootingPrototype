using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour {

	public Player player;
	public int playerId;
	public string Horizontal;
	public string Vertical;
	public string CrouchButton;
	public string Jump;

	public bool canMove = true;

	static InputManager _Instance;
	public static InputManager Instance{
		get{ 
			return _Instance;
		}
	}
	private PlayerController character;
	private bool jump;

	float h;
	void Awake()
	{
//		if (Instance != null) {
//			Destroy (this.gameObject);
//		} else {
			_Instance = this;
//		}
		player = ReInput.players.GetPlayer (playerId);
		character = GetComponent<PlayerController>();

	}


	private void Update()
	{
		if (!jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			jump = player.GetButtonDown(Jump);
		}
	}


	private void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = player.GetButton(CrouchButton);
		if (canMove) {
			h = player.GetAxis (Horizontal);
		} else {
			h = 0;
		}// Pass all parameters to the character control script.
		character.Move(h, crouch, jump);
		jump = false;
	}
}
