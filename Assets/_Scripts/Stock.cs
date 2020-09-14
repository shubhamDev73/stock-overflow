using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Stock : MonoBehaviour {

	[HideInInspector]
	public Stock[] adjacentStocks = new Stock[4]{null, null, null, null};

	[HideInInspector]
	public bool isAttached = false;

	public void Attach (Player player) {

		transform.SetParent(player.transform);
		isAttached = true;

	}

	public void Detach () {

		transform.SetParent(null);
		isAttached = false;

	}

}
