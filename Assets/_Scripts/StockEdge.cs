using UnityEngine;

public class StockEdge : MonoBehaviour {

	public Stock stock;
	public GameManager.StockEdgeType edgeType;

	void OnTriggerStay (Collider col) {

		Stock stock = col.GetComponent<Stock>();
		if(!stock) return;
		ChangeStock(stock);

	}

	void OnTriggerExit (Collider col) {

		Stock stock = col.GetComponent<Stock>();
		if(!stock) return;
		ChangeStock(null);

	}

	void ChangeStock (Stock newStock) {
		if(newStock == stock) return;

		stock.adjacentStocks[(int)edgeType] = newStock;

	}

}
