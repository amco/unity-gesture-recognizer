using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class RecordGesture : TrailGesture
    {
        #region FIELDS

        protected const string NoNameMessage = "No name inserted";
        protected const string GestureSavedMessage = "{0}  saved!";

        [SerializeField] protected InputField gestureNameField = null;
        [SerializeField] protected Button submitButton = null;
        [SerializeField] protected Button clearButton = null;

        #endregion

        #region BEHAVIORS

        protected override void Awake()
        {
            submitButton.onClick.AddListener(Submit);
            clearButton.onClick.AddListener(DeleteTrailRenderers);
        }

        private void Submit()
        {
            AddGesture();
            DeleteTrailRenderers();
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
