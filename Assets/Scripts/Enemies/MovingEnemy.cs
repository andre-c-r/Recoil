using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingEnemy : MonoBehaviour {
    [SerializeField] GameObject enemy;
    [SerializeField] float speed;
    [SerializeField] Transform[] travelPoints;
    [Range(0, 1)]
    [SerializeField] float startingPoint = 0;
    public bool snapReturn = false;

    int currentTravelPoint = 0, nextPoint = 1;
    float t = 0;
    bool returning = false;

    void UpdateT() {
        t = 0;
        currentTravelPoint = nextPoint;
    }

    void CalculateNextPoint() {
        if (t < 1) return;

        UpdateT();

        if (snapReturn && nextPoint + 1 >= travelPoints.Length) {
            ResetToZero();
            return;
        }

        if (nextPoint + 1 >= travelPoints.Length) {
            returning = true;
        }
        else if (nextPoint - 1 < 0) {
            returning = false;
        }

        nextPoint = returning ? currentTravelPoint - 1 : currentTravelPoint + 1;
    }

    void ResetToZero() {
        t = 0;
        currentTravelPoint = 0;
        nextPoint = 1;
    }

    public void Reset() {
        t = startingPoint;
        currentTravelPoint = 0;
        nextPoint = 1;
    }

    // Start is called before the first frame update
    void Start() {
        Reset();
    }

    // Update is called once per frame
    void FixedUpdate() {
        CalculateNextPoint();

        t += Time.deltaTime * (speed / Vector3.Distance(travelPoints[currentTravelPoint].position, travelPoints[nextPoint].position));

        enemy.transform.position = Vector3.Lerp(travelPoints[currentTravelPoint].position, travelPoints[nextPoint].position, t);
    }

#if UNITY_EDITOR
    public void OnDrawGizmos() {
        if (travelPoints.Length < 2) return;

        Vector3[] points = new Vector3[travelPoints.Length];

        for (int i = 0; i < travelPoints.Length; i++) points[i] = travelPoints[i].position;

        float sum = 0;
        for (int i = 0; i < points.Length - 1; i++) sum += Vector3.Distance(points[i], points[i + 1]);

        Color color = Color.red;

        Handles.color = color;
        Handles.DrawDottedLines(points, 2.0f);

        GUI.color = color;
        Handles.Label(this.transform.position + Vector3.up * 0.5f, (sum / speed).ToString("F1") + " S");
    }
#endif
}
