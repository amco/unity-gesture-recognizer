using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class DetectGesture : TrailGesture
    {
        #region FIELDS

        protected const string GestureDetectedMessage = "{0}  recognized!";

        [SerializeField] protected Button submitButton = null;
        [SerializeField] protected Button clearButton = null;

        protected bool clearOnNextDraw = false;

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
