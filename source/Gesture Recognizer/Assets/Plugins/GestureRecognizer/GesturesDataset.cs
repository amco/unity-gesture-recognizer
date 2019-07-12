using System.Collections.Generic;
using UnityEngine;

using PDollar = PDollarGestureRecognizer;
using QDollar = QDollarGestureRecognizer;

namespace GestureRecognizer
{
    [CreateAssetMenu(menuName = MenuName)]
    public class GesturesDataset : ScriptableObject
    {
        #region FIELDS

        private const string MenuName = "Gesture Recognizer/Gesture Dataset";

        [SerializeField] private List<SerializableGesture> gestures;

        private PDollar.Gesture[] pDollarGestures;
        private bool shouldRecalculate = true;

        #endregion

        #region PROPERTIES

        public bool IsEmpty { get { return Gestures.Count == 0; } }
        public List<SerializableGesture> Gestures
        {
            get
            {
                if (gestures == null)
                    gestures = new List<SerializableGesture>();
                return gestures;
            }
            private set
            {
                gestures = value;
            }
        }

        #endregion

        #region BEHAVIORS

        public PDollar.Gesture[] PDollarGestures()
        {
            if (!shouldRecalculate)
                return pDollarGestures;

            pDollarGestures = new PDollar.Gesture[gestures.Count];
            for (int i = 0; i < gestures.Count; i++)
            {
                pDollarGestures[i] = gestures[i].ToPDollarGesture();
            }
            return pDollarGestures;
        }

        public string Recognize(Vector3[] points)
        {
            List<Vector3[]> gesturesList = new List<Vector3[]>();
            gesturesList.Add(points);
            return Recognize(gesturesList);
        }

        public string Recognize(List<Vector3[]> points)
        {
            SerializableGesture gestureToDetect = new SerializableGesture(Vectors3ListToPoints(points));
            return QDollar.QPointCloudRecognizer.Classify(gestureToDetect.ToPDollarGesture(), PDollarGestures());
        }

        public SerializableGesture GetGesture(string name)
        {
            return Gestures.Find(gesture => gesture.name == name);
        }

        public void AddGesture(Vector3[] points, string name)
        {
            List<Vector3[]> gesturesList = new List<Vector3[]>();
            gesturesList.Add(points);
            AddGesture(gesturesList, name);
        }

        public void AddGesture(List<Vector3[]> points, string name)
        {
            SerializableGesture foundGesture = Gestures.Find(gesture => gesture.name == name);
            if (foundGesture == null)
                Gestures.Add(new SerializableGesture(Vectors3ListToPoints(points), name));
            else
                foundGesture.points = Vectors3ListToPoints(points);
            shouldRecalculate = true;
        }

        public bool DeleteGesture(string name)
        {
            SerializableGesture foundGesture = Gestures.Find(gesture => gesture.name == name);
            if (foundGesture == null)
                return false;
            Gestures.Remove(foundGesture);
            shouldRecalculate = true;
            return true;
        }

        private SerializablePoint[] Vectors3ListToPoints(List<Vector3[]> vector3Points)
        {
            List<SerializablePoint> pDollarPoints = new List<SerializablePoint>();
            int strokeIndex = 0;
            foreach (Vector3[] vector3Stroke in vector3Points)
            {
                pDollarPoints.AddRange(Vectors3ToPoints(vector3Stroke, strokeIndex++));
            }
            return pDollarPoints.ToArray();
        }

        private SerializablePoint[] Vectors3ToPoints(Vector3[] vector3Points, int strokeId = 0)
        {
            SerializablePoint[] pDollarPoints = new SerializablePoint[vector3Points.Length];
            for (int i = 0; i < vector3Points.Length; i++)
            {
                pDollarPoints[i] = Vector3ToPoint(vector3Points[i], strokeId);
            }
            return pDollarPoints;
        }

        private SerializablePoint Vector3ToPoint(Vector3 vector3Point, int strokeId)
        {
            return new SerializablePoint(vector3Point.x, vector3Point.y, strokeId);
        }

        #endregion
    }
}
