using System.Collections.Generic;
using UnityEngine;

namespace MVC.Views
{
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class RoadView : MonoBehaviour
    {
        public Vector2 FirstPoint { get; private set;}
        public Vector2 LastPoint { get; private set; }
        public float Length => LastPoint.x - FirstPoint.x;

        private EdgeCollider2D _edgeCollider;
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _edgeCollider = GetComponent<EdgeCollider2D>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void DrawRoadByPoints(List<Vector2> roadPoints)
        {
            _edgeCollider.points = roadPoints.ToArray();
            _lineRenderer.positionCount = roadPoints.Count;
            for (var i = 0; i < roadPoints.Count; i++)
            {
                _lineRenderer.SetPosition(i, new Vector3(roadPoints[i].x, roadPoints[i].y, 0f));
            }

            FirstPoint = roadPoints[0];
            LastPoint = roadPoints[roadPoints.Count - 1];
        }
    }
}
