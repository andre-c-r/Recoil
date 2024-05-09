using UnityEngine;
using System.Collections;
using System.Reflection;
using Unity.Mathematics;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine.EventSystems;
using System.Linq;

public class CollisionChecker : RaycastController {
    public CollisionInfo collisions;

    public float checkDistance = 0.5f;

    public Transform[] raycastOriginPoints;

    public override void Start() {
        base.Start();
        collisions.verticalTag = new string[verticalRayCount];
    }

    public void CheckGrounder(Vector3 currentSpeed) {
        Vector2 aux = Vector2.zero;
        collisions.Reset();
        VerticalCollisions(currentSpeed);
    }

    void VerticalCollisions(Vector3 currentSpeed) {
        UpdateRaycastOrigins();


        float directionY = currentSpeed.y == 0 ? -1 : Mathf.Sign(currentSpeed.y);


        float rayLength = checkDistance;

        if (raycastOriginPoints.Length > 0) verticalRayCount = raycastOriginPoints.Length;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = new Vector2();

            if (verticalRayCount == 0) {
                rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + currentSpeed.x * Time.deltaTime);
            }
            else {
                rayOrigin = raycastOriginPoints[i].position;
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                //rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;

                collisions.verticalTag[i] = hit.collider.tag;
            }
            else {
                collisions.verticalTag[i] = null;
            }
        }
    }

    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public string[] verticalTag;

        public void Reset() {
            above = below = false;
            left = right = false;
        }
    }

}
