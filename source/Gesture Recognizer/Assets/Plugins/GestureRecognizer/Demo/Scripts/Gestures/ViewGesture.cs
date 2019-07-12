using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class ViewGesture : TrailGesture
    {
        #region FIELDS

        protected const string GestureShownMessage = "{0} shown!";

        [SerializeField] protected Button nextButton = null;
        [SerializeField] protected Button previousButton = null;
        [SerializeField] protected Button deleteButton = null;

        protected int index = 0;

        #endregion

        #region PROPERTIES

        private SerializableGesture CurrentGesture { get { return Gestures[index]; } }
        private List<SerializableGesture> Gestures { get { return gestureDataset.Gestures; } }

        #endregion

        #region BEHAVIORS

        protected override void Awake()
        {
            base.Awake();
            nextButton.onClick.AddListener(NextGesture);
            previousButton.onClick.AddListener(PreviousGesture);
            deleteButton.onClick.AddListener(DeleteGesture);
        }

        private void OnEnable()
        {
            index = 0;
            ShowCurrentGesture();
        }

        private void NextGesture()
        {
            if (!IsReadyToView())
                return;

            index = (index < Gestures.Count - 1) ? index + 1 : 0;
            ShowCurrentGesture();
        }

        private void PreviousGesture()
        {
            if (!IsReadyToView())
                return;

            index = (index > 0) ? index - 1 : index = Gestures.Count - 1;
            ShowCurrentGesture();
        }

        private void DeleteGesture()
        {
            if (!IsReadyToView())
                return;

            gestureDataset.DeleteGesture(CurrentGesture.name);
            if (index >= Gestures.Count)
                index = 0;
            ShowCurrentGesture();
        }

        private void ShowCurrentGesture()
        {
            DeleteTrailRenderers();

            if (!IsReadyToView())
                return;

            List<Vector3[]> listOfPoints = CurrentGesture.GetPointsVector3Lists();
            foreach (Vector3[] points in listOfPoints)
            {
                TrailRenderer trailRenderer = Instantiate<TrailRenderer>(trailRendererPrefab, Vector3.zero, Quaternion.identity, transform);
                trailRenderers.Add(trailRenderer);
                trailRenderer.AddPositions(points);
                trailRenderer.transform.position = points[points.Length - 1];
            }
            ShowStatusMessage(string.Format(GestureShownMessage, CurrentGesture.name));
        }

        private bool IsReadyToView()
        {
            if (gestureDataset == null)
            {
                ShowStatusMessage(NoDatasetFoundMessage);
                return false;
            }
            if (gestureDataset.IsEmpty)
            {
                ShowStatusMessage(NoGesturesFoundMessage);
                return false;
            }
            return true;
        }

        #endregion
    }
}
