using UnityEngine;

public class StockEdge : MonoBehaviour {

	public Stock stock;

	void OnTriggerStay (Collider col) {

		Stock currentStock = col.GetComponent<Stock>();
		if(!currentStock) return;
		ChangeStock(currentStock);

	}

	void OnTriggerExit (Collider col) {

		Stock currentStock = col.GetComponent<Stock>();
		if(!currentStock) return;
		stock.ResetAdjacentStocks();

	}

	void ChangeStock (Stock newStock) {
		if(newStock == stock) return;

		int index;
		float angle = transform.eulerAngles.y;

		if(Mathf.Abs(angle - 0) < 0.1f)
			index = (int)GameManager.StockEdgeType.Up;
		else if(Mathf.Abs(angle - 180) < 0.1f)
			index = (int)GameManager.StockEdgeType.Down;
		else if(Mathf.Abs(angle - 90) < 0.1f)
			index = (int)GameManager.StockEdgeType.Right;
		else if(Mathf.Abs(angle - 270) < 0.1f)
			index = (int)GameManager.StockEdgeType.Left;
 		else return;

		stock.adjacentStocks[index] = newStock;

	}

}
