using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
	[SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 1)] [SerializeField] private float jumpSpeed = .36f; 
	[SerializeField] private bool airControl = false;                 // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask groundMask;                  // A mask determining what is ground to the character

	private Transform groundCheck;    // A position marking where to check if the player is grounded.
	const float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool grounded;            // Whether or not the player is grounded.
	private Transform ceilingCheck;   // A position marking where to check for ceilings
	const float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	//private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D rbd2D;
	private bool facingRight = true;  // For determining which way the player is currently facing.
	Transform bodyGraphics;


	private void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		//m_Anim = GetComponent<Animator>();
		rbd2D = GetComponent<Rigidbody2D>();
		bodyGraphics = transform.Find ("BodyGraphics");
	}


	private void FixedUpdate()
	{
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius,groundMask );
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				grounded = true;
		}
		// m_Anim.SetBool("Ground", m_Grounded);

		// Set the vertical animation
		// m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}

	public void Die(){
		gameObject.SetActive (false);
		Debug.Log (gameObject.name + " is dead");
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)// && m_Anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundMask))
			{
				crouch = true;
			}
		}

		// Set whether or not the character is crouching in the animator
		//m_Anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if (grounded || airControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move*crouchSpeed : move);
			move = ( !grounded ? move*jumpSpeed : move);


			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//  m_Anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			rbd2D.velocity = new Vector2(move*maxSpeed, rbd2D.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (grounded && jump)// && m_Anim.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			grounded = false;
			//m_Anim.SetBool("Ground", false);
			rbd2D.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = bodyGraphics.localScale;
		theScale.x *= -1;
		bodyGraphics.localScale = theScale;
	}



}


