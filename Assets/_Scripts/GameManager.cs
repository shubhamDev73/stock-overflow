using UnityEngine;

public static class GameManager {

	public static float speed {get;} = 7f;
	public static float smoothness {get;} = 0.9f;

	public static float rotateSpeed {get;} = 0.4f;
	public static float rotateSqrError {get;} = 30f;

	public static float tileSize {get;} = 1f;

	public enum StockEdgeType {
		up, down, right, left
	};

	public static float ClampToTile (float n, float direction) {

		float result = n / tileSize;

		if(Mathf.Approximately(direction, 0))
			result = Mathf.Round(result);
		else if (direction > 0)
			result = Mathf.Ceil(result);
		else
			result = Mathf.Floor(result);

		return tileSize * result;

	}

	public static Vector3 ClampToTile (Vector3 vector, Vector3 direction) {
		return new Vector3(ClampToTile(vector.x, direction.x), ClampToTile(vector.y, direction.y), ClampToTile(vector.z, direction.z));
	}

	public static Vector3 ClampToTile (Vector3 vector) {
		return ClampToTile(vector, Vector3.zero);
	}

}
