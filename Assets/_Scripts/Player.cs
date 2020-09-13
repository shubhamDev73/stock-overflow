using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

[RequireComponent(typeof(Stock))]
public class Player : MonoBehaviour {

	private Vector3 position;
	private bool isRotating;
	private Stock playerStock;
	private List<Stock> attachedStocks = new List<Stock>();

	void Start () {

		position = transform.position;
		isRotating = false;
		playerStock = GetComponent<Stock>();
		Grab(playerStock);

	}

	void Update () {

		Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		Move(movement);

		if(Input.GetButtonDown("Rotate"))
			StartCoroutine(Rotate(Input.GetAxis("Rotate")));

		if(Input.GetButtonDown("Grab"))
			Grab(movement);

		if(Input.GetButtonDown("Leave"))
			Leave();

	}

	void Move (Vector2 movement) {

		Vector3 direction = (Vector3.right * movement.x + Vector3.forward * movement.y).normalized;
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

	void Grab (Vector2 dir) {

		Grab(GetAdjacentStock(GetIndex(dir)));

	}

	void Grab (Stock stock) {

		if(!stock) return;

		stock.Attach(this);
		if(stock == playerStock)
			attachedStocks.Add(stock);
		else
			attachedStocks.Insert(1, stock);

	}

	int GetIndex (Vector2 dir) {

		int index = -1;

		if(dir.sqrMagnitude > 0.0001f)
			if(Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
				index = dir.y > 0 ? 0 : 1;
			else
				index = dir.x > 0 ? 2 : 3;

		return index;

	}

	Stock GetAdjacentStock (int index, bool directionOnly = false) {

		if(directionOnly && index < 0) return null;

		foreach(Stock attachedStock in attachedStocks){
			if(index == -1){
				for(int i = 0; i < 4; i++){
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
