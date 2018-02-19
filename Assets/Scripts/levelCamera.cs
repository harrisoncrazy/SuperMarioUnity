using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelCamera : MonoBehaviour {

	public Transform playerPos;

	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (playerPos.position.x, 0.8f, -10.0f);
	}
}
