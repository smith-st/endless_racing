using System.Collections.Generic;
using System.Linq;
using MVC.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MVC.Models
{
    public class RoadModel: BaseModel
    {
        private const int RoadParts = 2;
        private const float PartLength = 100f;
        private const float PartMinLength = 10f;
        private const float PartMaxLength = 20f;
        private const float HeightDifference = 10f;

        private List<RoadView> _roads = new List<RoadView>(RoadParts);
        private float _changeRoadDistance;
        private List<Vector2> _baseRoadPoints;

        public RoadModel(GameObject prefab)
        {
            _baseRoadPoints = GenerateBasePoint(PartMinLength * 2f, PartLength * (RoadParts + 1));
            AddStartupArea();
            RoadView road = null;
            RoadView lastAddedRoad = null;
            for (var i = 0; i < RoadParts; i++)
            {
                road = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity).GetComponent<RoadView>();
                CheckView(road);
                _roads.Add(road);

                var roadPoints = GenerateRoadPoint(PartLength);
                if (lastAddedRoad != null)
                {
                    roadPoints.Insert(0, lastAddedRoad.LastPoint);
                }
                road.DrawRoadByPoints(roadPoints);
                lastAddedRoad = road;
            }

            UpdateDistanceForUpdate(road);
        }

        private void AddStartupArea()
        {
            _baseRoadPoints.Insert(0, new Vector2(-PartMinLength*2f,0f));
            _baseRoadPoints.Insert(1, new Vector2(-PartMinLength,0f));
            _baseRoadPoints.Insert(2, new Vector2(0,0f));
            _baseRoadPoints.Insert(3, new Vector2(PartMinLength,0f));
        }

        private List<Vector2> GenerateRoadPoint(float length)
        {
            const float quality = 10f;
            var roadPoints = new List<Vector2>();
            var firstPointPosition = _baseRoadPoints[0].x<0?0:_baseRoadPoints[0].x;
            var countPointsToRemove = 0;
            for (var i = 1; i < _baseRoadPoints.Count; i++)
            {
                var p1 = _baseRoadPoints[i];
                var p0 = Vector2.Lerp(_baseRoadPoints[i - 1], _baseRoadPoints[i], 0.7f);
                var p2 = Vector2.Lerp(_baseRoadPoints[i], _baseRoadPoints[i + 1], 0.3f);
                for (var j = 0f; j < quality; j++)
                {
                    var t = j / quality;
                    var point = Utils.QuadraticLinearBezierPoint(t, p0, p1, p2);
                    roadPoints.Add(point);
                }

                if (p1.x - firstPointPosition > length)
                {
                    countPointsToRemove = i;
                    break;
                }
            }

            for (var i = 0; i < countPointsToRemove; i++)
            {
                _baseRoadPoints.RemoveAt(0);
            }

            var lastPosition = _baseRoadPoints[_baseRoadPoints.Count - 1].x;
            _baseRoadPoints.AddRange(GenerateBasePoint(lastPosition + PartMinLength, lastPosition + PartLength));

            return roadPoints;
        }

        private List<Vector2> GenerateBasePoint(float from = 0f, float to = 0f)
        {
            var currentPosition = from;
            var lastDifferenceDirection = 1;
            var repetitiveDifferenceDirection = 0;
            var basePoints = new List<Vector2>();
            do
            {
                var heightDifference = Random.Range(0f, HeightDifference);
                var differenceDirection = DifferenceDirection(ref lastDifferenceDirection, ref repetitiveDifferenceDirection);
                basePoints.Add(new Vector2(currentPosition, heightDifference * differenceDirection));

                if (currentPosition >= to)
                {
                    break;
                }
                currentPosition = NextPoint(currentPosition);
            } while (true);

            return basePoints;
        }

        private float NextPoint(float currentPosition)
        {
            var partLength = Random.Range(PartMinLength, PartMaxLength);
            return currentPosition + partLength;
        }

        private int DifferenceDirection(ref int lastDifferenceDirection, ref int repetitiveDifferenceDirection)
        {
            int differenceDirection;

            do
            {
                differenceDirection = Random.Range(-1, 2);

            } while (differenceDirection==0 || (differenceDirection == lastDifferenceDirection && repetitiveDifferenceDirection >= 4));

            if (differenceDirection == lastDifferenceDirection)
            {
                repetitiveDifferenceDirection++;
            }
            else
            {
                repetitiveDifferenceDirection = 0;
            }

            lastDifferenceDirection = differenceDirection;
            return differenceDirection;
        }

        public void BikePosition(Vector2 position)
        {
            if (position.x > _changeRoadDistance)
            {
                UpdateRoad();
            }
        }

        private void UpdateRoad()
        {
            var road = _roads.Shift();
            var roadPoints = GenerateRoadPoint(PartLength);
            roadPoints.Insert(0,_roads.Last().LastPoint);
            road.DrawRoadByPoints(roadPoints);
            UpdateDistanceForUpdate(road);
            _roads.Add(road);
        }


        private void UpdateDistanceForUpdate(RoadView road)
        {
            if (road == null)
            {
                return;
            }
            _changeRoadDistance = road.FirstPoint.x + road.Length / 2f;
        }
    }
}