using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

[RequireComponent(typeof(Stock))]
public class Player : MonoBehaviour {

	private Vector3 position;
	private bool isRotating;
	private Stock playerStock;
	private List<Stock> attachedStocks = new List<Stock>();
	private float prevGrab;

	void Start () {

		position = transform.position;
		isRotating = false;
		playerStock = GetComponent<Stock>();
		Grab(playerStock);
		prevGrab = 0f;

	}

	void Update () {

		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		Move(movement);

		if(Input.GetButtonDown("Rotate"))
			StartCoroutine(Rotate(Input.GetAxis("Rotate")));

		if(Input.GetButtonDown("Grab"))
			Grab(movement);

		float grab = Input.GetAxis("Grab");
		if(grab > GameManager.holdDuration && grab - prevGrab > GameManager.holdGap){
			prevGrab = grab;
			Grab(movement);
		}

		if(Input.GetButtonUp("Grab"))
			prevGrab = 0f;

		if(Input.GetButtonDown("Leave"))
			Leave();

	}

	void Move (Vector2 movement) {

		Vector3 initPos = position;

		Vector3 direction = (Vector3.right * movement.x + Vector3.forward * movement.y).normalized;
		position += direction * GameManager.speed * Time.deltaTime;

		if(GetAdjacentStock(movement, true))
			position = GameManager.ClampToTile(initPos, -direction);

		Vector3 displacement = GameManager.ClampToTile(position, direction) - transform.position;
		transform.Translate(displacement * (1 - GameManager.smoothness), Space.World);

	}

	IEnumerator Rotate (float direction) {
		if(isRotating) yield break;

		isRotating = true;
		Vector3 initial = transform.eulerAngles;
		Vector3 final = initial + Vector3.up * 90 * direction;

		while((final - initial).sqrMagnitude > GameManager.rotateSqrError){
			initial = Vector3.Lerp(initial, final, GameManager.rotateSpeed);
			transform.eulerAngles = initial;
			yield return new WaitForFixedUpdate();
		}

		transform.eulerAngles = final;
		isRotating = false;
		foreach(Stock attachedStock in attachedStocks)
			attachedStock.ResetAdjacentStocks();

	}

	void Grab (Vector2 direction) {
		Grab(GetAdjacentStock(direction));
	}

	void Grab (Stock stock) {
		if(!stock) return;

		stock.Attach(this);
		if(stock == playerStock)
			attachedStocks.Add(stock);
		else
			attachedStocks.Insert(1, stock);

	}

	int GetIndex (Vector2 direction) {
		if(direction.sqrMagnitude <= GameManager.movementThreshold) return -1;

		if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
			return (int)((direction.y > 0) ? GameManager.StockEdgeType.Up : GameManager.StockEdgeType.Down);
		else
			return (int)((direction.x > 0) ? GameManager.StockEdgeType.Right : GameManager.StockEdgeType.Left);

	}

	Stock GetAdjacentStock (Vector2 direction, bool directionOnly = false) {
		return GetAdjacentStock(GetIndex(direction), directionOnly);
	}

	Stock GetAdjacentStock (int index, bool directionOnly = false) {
		if(directionOnly && index < 0) return null;

		foreach(Stock attachedStock in attachedStocks){
			if(index == -1){
				for(int i = 0; i < GameManager.totalEdges; i++){
					Stock stock = attachedStock.adjacentStocks[i];
					if(stock && !stock.isAttached)
						return stock;
				}
			}else{
				Stock stock = attachedStock.adjacentStocks[index];
				if(stock && !stock.isAttached)
					return stock;
			}
		}
		return null;

	}

	void Leave () {

		foreach(Stock stock in attachedStocks)
			if(stock != playerStock)
				Leave(stock);
		attachedStocks.Clear();
		Grab(playerStock);

	}

	void Leave (Stock stock) {

		stock.Detach();

	}

}
