using UnityEngine;

public static class GameManager {

	public const float speed = 7f;
	public const float smoothness = 0.9f;
	public const float movementThreshold = 0.0001f;

	public const float rotateSpeed = 0.4f;
	public const float rotateSqrError = 30f;

	public const float tileSize = 1f;

	public const float holdDuration = 0.1f;
	public const float holdGap = 0.1f;

	public enum StockEdgeType {
		Up, Down, Right, Left,
	};
	public const int totalEdges = 4;

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
