using UnityEngine;
using System.Collections;

public class RaycastController : MonoBehaviour {

	public LayerMask collisionMask;
	
	public const float skinWidth = .015f;

	const float dstBetweenRays = .25f;
	

	public int horizontalRayCount, verticalRayCount;

	[HideInInspector]
	public float horizontalRaySpacing;
	[HideInInspector]
	public float verticalRaySpacing;

	public BoxCollider2D collider;
	public RaycastOrigins raycastOrigins;

	public virtual void Awake() {
	}

	public virtual void Start() {
		CalculateRaySpacing ();
	}

	public void UpdateRaycastOrigins() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);
		
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	public void CalculateRaySpacing() {
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		float boundsWidth = bounds.size.x;
		float boundsHeight = bounds.size.y;
		
		if(verticalRayCount <= 0) {
			verticalRayCount = Mathf.RoundToInt (boundsWidth / dstBetweenRays);
		}

		if (horizontalRayCount <= 0) {
			horizontalRayCount = Mathf.RoundToInt (boundsHeight / dstBetweenRays);
		}
		
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	
	public struct RaycastOrigins {
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}
}
