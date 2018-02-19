using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public static PlayerMovement Instance;

	public bool isLargeMario = false;

	[SerializeField]
	private float moveSpeedDefault = 4.0f;
	[SerializeField]
	private float jumpSpeedDefault = 4.5f;
	private float fallMultiplier = 2f;
	private float lowJumpMultiplier = 1.5f;

	public bool isGrounded = true;
	public bool isCrouching = false;

	private Rigidbody2D mainRB;
	private SpriteRenderer mainRenderer;
	public Animator mainAnimator;

	[SerializeField]
	private BoxCollider2D defaultCollider;
	[SerializeField]
	private BoxCollider2D crouchedCollider;

	// Use this for initialization
	void Start () {
		Instance = this;

		mainRB = this.GetComponent<Rigidbody2D> ();
		mainRenderer = this.GetComponent<SpriteRenderer> ();
		mainAnimator = this.GetComponent<Animator> ();

		scaleDown ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		updateMovement ();
	}

	void scaleUp () {
		isLargeMario = true;
		mainAnimator.SetBool ("isLarge", true);
	}

	void scaleDown() {
		isLargeMario = false;
		mainAnimator.SetBool ("isLarge", false);
	}

	void updateMovement() {
		if (isCrouching == false || isGrounded == false) {//disabling movement if crouching
			float move = Input.GetAxis ("Horizontal");
			float moveSpeed = move * moveSpeedDefault;
	
			mainRB.AddForce (new Vector2 (moveSpeed, 0));

			//Getting and storing y velocity
			float tempY = mainRB.velocity.y;
			Vector3 tempVelocity = mainRB.velocity;
			//clamping velocity
			tempVelocity = Vector3.ClampMagnitude (tempVelocity, 1.5f);
			//reseting y velocity, therefore only clamping x velocity
			tempVelocity.y = tempY;
			mainRB.velocity = tempVelocity;
		}

		updateJumping ();
		updateCrouching ();

		updateAnimations ();
	}

	void updateCrouching() {
		if (Input.GetKey (KeyCode.S)) {
			isCrouching = true;
			defaultCollider.enabled = false;
			crouchedCollider.enabled = true;

			mainAnimator.SetBool ("isCrouching", true);
		} else {
			isCrouching = false;
			defaultCollider.enabled = true;
			crouchedCollider.enabled = false;

			mainAnimator.SetBool ("isCrouching", false);
		}
	}

	void updateJumping() {
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded == true) {//starting jumping
			Vector2 originalVelocity = mainRB.velocity;//storing velociy with x to preserve x velocity
			mainRB.velocity = (Vector2.up * jumpSpeedDefault) + originalVelocity; //jumping up
			isGrounded = false;//not on the ground, gets swapped back in seperate grounding script
			mainAnimator.SetBool ("isJumping", true); //setting jump animation
		}
			
		if (isGrounded == false) {//making the falling more weighty, basically increases the gravity as the player falls
			if (mainRB.velocity.y < 0) {//if its falling down
				mainRB.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;//gravity is higher
			} else if (mainRB.velocity.y > 0 && !Input.GetKey (KeyCode.Space)) {//smaller jump, if the jump key is not held makes gravity higher as the player is going up (making jump smaller)
				mainRB.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
			}
		}
	}

	void updateAnimations() {
		//Left/Right movement
		if (mainRB.velocity.x > 0.15f || mainRB.velocity.x < -0.15f) {
			mainAnimator.SetBool ("isMoving", true);
		} else {
			mainAnimator.SetBool ("isMoving", false);
		}

		mainAnimator.speed = Mathf.Clamp (mainRB.velocity.magnitude, 0.25f, 1.5f);//changing animation speed based on movement

		flipSprite ();
	}

	void flipSprite() {
		if (mainRB.velocity.x < -0.3f) 
		{
			mainRenderer.flipX = true;
		} 
		else if (mainRB.velocity.x > 0.3f) 
		{
			mainRenderer.flipX = false;
		}
	}
}
