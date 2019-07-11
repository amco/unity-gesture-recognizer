using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class ViewGesture : MonoBehaviour
    {
        #region FIELDS

        private const string NoDatasetFoundMessage = "No gesture dataset";
        private const string GestureShownMessage = "{0} shown!";
        private const string NoGesturesFoundMessage = "No gestures found";

        [SerializeField] GesturesDataset gestureDataset = null;
        [SerializeField] TrailRenderer trailRendererPrefab = null;
        [SerializeField] Text logText = null;
        [SerializeField] private Button nextButton = null;
        [SerializeField] private Button previousButton = null;
        [SerializeField] private Button deleteButton = null;

        private List<TrailRenderer> trailRenderers = null;
        private int index = 0;

        #endregion

        #region PROPERTIES

        private SerializableGesture CurrentGesture { get { return Gestures[index]; } }
        private List<SerializableGesture> Gestures { get { return gestureDataset.Gestures; } }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            trailRenderers = new List<TrailRenderer>();
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
            if (ShowErrorMessage())
                return;

            index++;
            if (index >= Gestures.Count)
                index = 0;
            ShowCurrentGesture();
        }

        private void PreviousGesture()
        {
            if (ShowErrorMessage())
                return;

            index--;
            if (index < 0)
                index = Gestures.Count - 1;
            ShowCurrentGesture();
        }

        private void DeleteGesture()
        {
            if (ShowErrorMessage())
                return;

            gestureDataset.DeleteGesture(CurrentGesture.name);
            if (index >= Gestures.Count)
                index = 0;
            ShowCurrentGesture();
        }

        private void DeleteTrailRenderers()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Destroy(trailRenderer.gameObject);
            }
            trailRenderers.Clear();
        }

        private void ShowCurrentGesture()
        {
            DeleteTrailRenderers();

            if (ShowErrorMessage())
                return;

            List<Vector3[]> listOfPoints = CurrentGesture.GetPointsVector3Lists();
            foreach (Vector3[] points in listOfPoints)
            {
                TrailRenderer trailRenderer = Instantiate<TrailRenderer>(trailRendererPrefab, Vector3.zero, Quaternion.identity, transform);
                trailRenderers.Add(trailRenderer);
                trailRenderer.AddPositions(points);
                trailRenderer.transform.position = points[points.Length - 1];
            }
            logText.text = string.Format(GestureShownMessage, CurrentGesture.name);
        }

        private bool ShowErrorMessage()
        {
            if (gestureDataset == null)
            {
                logText.text = NoDatasetFoundMessage;
                return true;
            }

            if (gestureDataset.IsEmpty)
            {
                logText.text = NoGesturesFoundMessage;
                return true;
            }
            return false;
        }

        #endregion
    }
}
