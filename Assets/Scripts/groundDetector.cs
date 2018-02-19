using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundDetector : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		PlayerMovement.Instance.isGrounded = true;
		PlayerMovement.Instance.mainAnimator.SetBool ("isJumping", false);
	}
}
