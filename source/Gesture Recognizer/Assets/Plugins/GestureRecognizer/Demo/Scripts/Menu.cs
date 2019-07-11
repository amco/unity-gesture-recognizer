using UnityEngine;
using UnityEngine.UI;

namespace GestureRecognizer.Demo
{
    public class Menu : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] private GameObject[] sections = null;
        [SerializeField] private Button nextSectionButton = null;
        [SerializeField] private Button previousSectionButton = null;

        private int index = 0;

        #endregion

        #region PROPERTIES

        private GameObject CurrentSection { get { return sections[index]; } }

        #endregion

        #region BEHAVIORS

        private void Awake()
        {
            nextSectionButton.onClick.AddListener(NextSection);
            previousSectionButton.onClick.AddListener(PreviousSection);
            HideAllSections();
            ShowCurrentSection();
        }

        private void NextSection()
        {
            HideCurrentSection();
            index++;
            if (index >= sections.Length)
                index = 0;
            ShowCurrentSection();
        }

        private void PreviousSection()
        {
            HideCurrentSection();
            index--;
            if (index < 0)
                index = sections.Length - 1;
            ShowCurrentSection();
        }

        private void HideCurrentSection()
        {
            CurrentSection.SetActive(false);
        }

        private void ShowCurrentSection()
        {
            CurrentSection.SetActive(true);
        }

        private void HideAllSections()
        {
            foreach (GameObject section in sections)
            {
                section.SetActive(false);
            }
        }

        #endregion
    }
}
