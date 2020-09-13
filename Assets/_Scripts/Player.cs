using UnityEngine;

public class Player : MonoBehaviour {

	private Vector3 position;

	void Start () {

		position = transform.position;

	}

	void Update () {

		Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

	}

	void Move (float right, float forward) {

		Vector3 direction = (Vector3.right * right + Vector3.forward * forward).normalized;
		position += direction * GameManager.speed * Time.deltaTime;
		Vector3 displacement = GameManager.ClampToTile(position, direction) - transform.position;
		transform.Translate(displacement * (1 - GameManager.smoothness), Space.World);

	}

}
