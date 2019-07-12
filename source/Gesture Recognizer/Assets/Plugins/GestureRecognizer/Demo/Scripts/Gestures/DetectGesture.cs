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
            if (!IsReadyToDetect())
                return;

            List<Vector3[]> strokes = new List<Vector3[]>();
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Vector3[] pointsToDetect = new Vector3[trailRenderer.positionCount];
                trailRenderer.GetPositions(pointsToDetect);
                strokes.Add(pointsToDetect);
            }
            LogMessage(string.Format(GestureDetectedMessage, gestureDataset.Recognize(strokes)));
        }

        private bool IsReadyToDetect()
        {
            if (gestureDataset == null)
            {
                LogMessage(NoDatasetFoundMessage);
                return false;
            }
            if (gestureDataset.IsEmpty)
            {
                LogMessage(NoGesturesFoundMessage);
                return false;
            }
            return true;
        }

        #endregion
    }
}
