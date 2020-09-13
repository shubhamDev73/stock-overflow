using System.Collections;
﻿using UnityEngine;

public class Player : MonoBehaviour {

	private Vector3 position;
	private bool isRotating;

	void Start () {

		position = transform.position;
		isRotating = false;

	}

	void Update () {

		Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if(Input.GetButtonDown("Rotate"))
			StartCoroutine(Rotate(Input.GetAxis("Rotate")));

	}

	void Move (float right, float forward) {

		Vector3 direction = (Vector3.right * right + Vector3.forward * forward).normalized;
		position += direction * GameManager.speed * Time.deltaTime;
		Vector3 displacement = GameManager.ClampToTile(position, direction) - transform.position;
		transform.Translate(displacement * (1 - GameManager.smoothness), Space.World);

	}

	IEnumerator Rotate (float dir) {
		if(isRotating) yield break;

		isRotating = true;
		Vector3 initial = transform.localEulerAngles;
		Vector3 final = initial + Vector3.up * 90 * dir;

		while((final - initial).sqrMagnitude > GameManager.rotateSqrError){
			initial = Vector3.Lerp(initial, final, GameManager.rotateSpeed);
			transform.localEulerAngles = initial;
			yield return new WaitForFixedUpdate();
		}

		transform.localEulerAngles = final;
		isRotating = false;

	}

}
