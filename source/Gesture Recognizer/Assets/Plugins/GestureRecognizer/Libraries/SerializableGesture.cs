using System;
using System.Collections.Generic;
using UnityEngine;

using PDollar = PDollarGestureRecognizer;

namespace GestureRecognizer
{
    [Serializable]
    public class SerializableGesture
    {
        #region FIELDS

        public string name;
        public SerializablePoint[] points;

        #endregion

        #region CONSTRUCTORS

        public SerializableGesture(SerializablePoint[] points, string gestureName = "")
        {
            this.points = points;
            this.name = gestureName;
        }

        #endregion

        #region BEHAVIORS

        public PDollar.Gesture ToPDollarGesture()
        {
            return new PDollar.Gesture(points, name);
        }

        public List<Vector3[]> GetPointsVector3Lists()
        {
            List<Vector3[]> listOfPoints = new List<Vector3[]>();
            List<Vector3> pointsInStroke = new List<Vector3>();
            int strokeId = -1;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].StrokeID == strokeId)
                {
                    pointsInStroke.Add(new Vector3(points[i].X, points[i].Y));
                }
                else
                {
                    strokeId = points[i].StrokeID;
                    if (pointsInStroke.Count > 0)
                        listOfPoints.Add(pointsInStroke.ToArray());
                    pointsInStroke.Clear();
                    pointsInStroke.Add(new Vector3(points[i].X, points[i].Y));
                }
            }
            if (pointsInStroke.Count > 0)
                listOfPoints.Add(pointsInStroke.ToArray());

            return listOfPoints;
        }

        #endregion
    }
}
