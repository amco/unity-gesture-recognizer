using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class TrailGesture : MonoBehaviour
    {
        #region FIELDS

        protected const string NoDatasetFoundMessage = "No gesture dataset";
        protected const string NoGesturesFoundMessage = "No gestures found";

        [SerializeField] protected GesturesDataset gestureDataset = null;
        [SerializeField] protected TrailRenderer trailRendererPrefab = null;
        [SerializeField] protected Text statusText = null;

        protected List<TrailRenderer> trailRenderers = null;

        #endregion

        #region BEHAVIORS

        protected virtual void Awake()
        {
            trailRenderers = new List<TrailRenderer>();
        }

        public virtual void OnDragStart()
        {
            trailRenderers.Add(Instantiate<TrailRenderer>(trailRendererPrefab, Vector3.zero, Quaternion.identity, transform));
            MoveTrailToCursor();
        }

        public virtual void OnDrag()
        {
            MoveTrailToCursor();
        }

        protected void DeleteTrailRenderers()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Destroy(trailRenderer.gameObject);
            }
            trailRenderers.Clear();
        }

        protected void MoveTrailToCursor()
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            trailRenderers[trailRenderers.Count - 1].transform.position = newPosition;
        }

        protected void ShowStatusMessage(string errorMessage)
        {
            statusText.text = errorMessage;
        }

        #endregion
    }
}
