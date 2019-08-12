using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class DetectGesture : TrailGesture
    {
        #region FIELDS

        private const string GestureDetectedMessage = "{0}  recognized!";

        [SerializeField] private Button submitButton = null;
        [SerializeField] private Button clearButton = null;

        private bool clearOnNextDraw = false;

        #endregion

        #region BEHAVIORS

        protected override void Awake()
        {
            base.Awake();
            submitButton.onClick.AddListener(Submit);
            clearButton.onClick.AddListener(DeleteTrailRenderers);
        }

        public override void OnDragStart()
        {
            if (clearOnNextDraw)
            {
                DeleteTrailRenderers();
                clearOnNextDraw = false;
            }
            base.OnDragStart();
        }

        private void Submit()
        {
            DetectPattern();
            clearOnNextDraw = true;
        }

        private void DetectPattern()
        {
            if (!IsReadyToDetect())
                return;

            List<Vector3[]> strokes = new List<Vector3[]>();
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Vector3[] pointsToDetect = new Vector3[trailRenderer.positionCount];
                trailRenderer.GetPositions(pointsToDetect);
                strokes.Add(pointsToDetect);
            }
            ShowStatusMessage(string.Format(GestureDetectedMessage, gestureDataset.Recognize(strokes)));
        }

        private bool IsReadyToDetect()
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
