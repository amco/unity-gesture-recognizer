using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class DetectGesture : MonoBehaviour
    {
        #region FIELDS

        private const string NoDatasetFoundMessage = "No gesture dataset";
        private const string NoGesturesFoundMessage = "No gestures found";
        private const string GestureDetectedMessage = "{0}  recognized!";

        [SerializeField] private GesturesDataset gestureDataset = null;
        [SerializeField] private TrailRenderer trailRendererPrefab = null;
        [SerializeField] private Text logText = null;
        [SerializeField] private Button submitButton = null;
        [SerializeField] private Button clearButton = null;

        private List<TrailRenderer> trailRenderers = null;
        private bool clearOnNextDraw = false;

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
            if (clearOnNextDraw)
            {
                DeleteTrailRenderers();
                clearOnNextDraw = false;
            }
            trailRenderers.Add(Instantiate<TrailRenderer>(trailRendererPrefab, Vector3.zero, Quaternion.identity, transform));
            MoveToCursor();
        }

        public void OnDrag()
        {
            MoveToCursor();
        }

        private void Submit()
        {
            DetectPattern();
            clearOnNextDraw = true;
        }

        private void DeleteTrailRenderers()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Destroy(trailRenderer.gameObject);
            }
            trailRenderers.Clear();
        }

        private void DetectPattern()
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
            logText.text = string.Format(GestureDetectedMessage, gestureDataset.Recognize(strokes));
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
