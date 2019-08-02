using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class RecordGesture : TrailGesture
    {
        #region FIELDS

        private const string NoNameMessage = "No name inserted";
        private const string GestureSavedMessage = "{0}  saved!";

        [SerializeField] private InputField gestureNameField = null;
        [SerializeField] private Button submitButton = null;
        [SerializeField] private Button clearButton = null;

        #endregion

        #region BEHAVIORS

        protected override void Awake()
        {
            base.Awake();
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
            if (!IsReadyToRecord())
                return;

            List<Vector3[]> strokes = new List<Vector3[]>();
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Vector3[] pointsToDetect = new Vector3[trailRenderer.positionCount];
                trailRenderer.GetPositions(pointsToDetect);
                strokes.Add(pointsToDetect);
            }
            gestureDataset.AddGesture(strokes, gestureNameField.text);
            ShowStatusMessage(string.Format(GestureSavedMessage, gestureNameField.text));
            gestureNameField.text = string.Empty;
        }

        private bool IsReadyToRecord()
        {
            if (gestureDataset == null)
            {
                ShowStatusMessage(NoDatasetFoundMessage);
                return false;
            }
            if (gestureNameField.text == string.Empty)
            {
                ShowStatusMessage(NoNameMessage);
                return false;
            }
            return true;
        }

        #endregion
    }
}
