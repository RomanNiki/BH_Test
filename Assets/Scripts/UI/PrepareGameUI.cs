using UnityEngine;

namespace UI
{
    public class PrepareGameUI : MonoBehaviour
    {
         [SerializeField] private Transform _preparePanel;

        public void ShowPrepareScreen()
        {
            _preparePanel.gameObject.SetActive(true);
        }
    }
}