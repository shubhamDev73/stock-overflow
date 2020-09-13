using UnityEngine;

public class StockEdge : MonoBehaviour {

	public Stock stock;
	public GameManager.StockEdgeType edgeType;

	void OnTriggerEnter (Collider col) {

		Stock stock = col.GetComponent<Stock>();
		if(stock == null) return;
		ChangeStock(stock);

	}

	void OnTriggerExit (Collider col) {

		Stock stock = col.GetComponent<Stock>();
		if(stock == null) return;
		ChangeStock(null);

	}

	void ChangeStock (Stock newStock) {

		stock.adjacentStocks[(int)edgeType] = newStock;

	}

}
