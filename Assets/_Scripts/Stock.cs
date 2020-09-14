using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Stock : MonoBehaviour {

	[HideInInspector]
	public Stock[] adjacentStocks = new Stock[GameManager.totalEdges];

	[HideInInspector]
	public bool isAttached = false;

	public Player attachedPlayer{
		get {return _attachedPlayer;}
		set{
			Vector3 initPos;

			// clamping attachedPlayer to tile, setting parent, unclamping
			if(value && value.transform != transform){
				initPos = value.transform.position;
				value.transform.position = GameManager.ClampToTile(value.transform.position, Vector3.zero);
				transform.SetParent(value.transform);
				value.transform.position = initPos;
			}else if(attachedPlayer){
				initPos = attachedPlayer.transform.position;
				attachedPlayer.transform.position = GameManager.ClampToTile(attachedPlayer.transform.position, Vector3.zero);
				transform.SetParent(initParent);
				attachedPlayer.transform.position = initPos;
			}

			_attachedPlayer = value;
			isAttached = attachedPlayer != null;
		}
	}

	private Player _attachedPlayer = null;
	private Transform initParent;

	void Start () {

		initParent = transform.parent;

	}

	public void Attach (Player player) {

		attachedPlayer = player;

	}

	public void Detach () {

		attachedPlayer = null;

	}

	public void ResetAdjacentStocks () {

		for(int i = 0; i < GameManager.totalEdges; i++)
			adjacentStocks[i] = null;

	}

}
