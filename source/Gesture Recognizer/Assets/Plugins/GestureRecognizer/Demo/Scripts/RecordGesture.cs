using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class RecordGesture : MonoBehaviour
    {
        #region FIELDS

        private const string NoDatasetFoundMessage = "No gesture dataset";
        private const string NoNameMessage = "No name found";
        private const string GestureSavedMessage = "{0}  saved!";

        [SerializeField] private GesturesDataset gestureDataset = null;
        [SerializeField] private TrailRenderer trailRendererPrefab = null;
        [SerializeField] private Text logText = null;
        [SerializeField] private InputField gestureNameField = null;
        [SerializeField] private Button submitButton = null;
        [SerializeField] private Button clearButton = null;

        private List<TrailRenderer> trailRenderers;

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            trailRenderers = new List<TrailRenderer>();
            submitButton.onClick.AddListener(Submit);
            clearButton.onClick.AddListener(DeleteTrailRenderers);
        }

        public void OnDragStart()
        {
            trailRenderers.Add(Instantiate<TrailRenderer>(trailRendererPrefab, Vector3.zero, Quaternion.identity, transform));
            MoveToCursor();
        }

        public void OnDrag()
        {
            MoveToCursor();
        }

        private void Submit()
        {
            AddGesture();
            DeleteTrailRenderers();
        }

        private void DeleteTrailRenderers()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Destroy(trailRenderer.gameObject);
            }
            trailRenderers.Clear();
        }

        private void AddGesture()
        {
            if (ShowErrorMessage())
                return;

            List<Vector3[]> strokes = new List<Vector3[]>();
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Vector3[] pointsToDetect = new Vector3[trailRenderer.positionCount];
                trailRenderer.GetPositions(pointsToDetect);
                strokes.Add(pointsToDetect);
            }
            gestureDataset.AddGesture(strokes, gestureNameField.text);
            logText.text = string.Format(GestureSavedMessage, gestureNameField.text);
            gestureNameField.text = string.Empty;
        }

        private void MoveToCursor()
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos.z = 0;
            trailRenderers[trailRenderers.Count - 1].transform.position = newPos;
        }

        private bool ShowErrorMessage()
        {
            if (gestureDataset == null)
            {
                logText.text = NoDatasetFoundMessage;
                return true;
            }

            if (gestureNameField.text == string.Empty)
            {
                logText.text = NoNameMessage;
                return true;
            }
            return false;
        }

        #endregion
    }
}
