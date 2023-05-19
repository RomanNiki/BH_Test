using TMPro;
using UnityEngine;

namespace UI
{
    public class WinnerUI : MonoBehaviour
    {
        [SerializeField] private Transform _winnerPanel;
        [SerializeField] private TMP_Text _winnerNickname;
        [SerializeField] private string _winnerTemplate = "Winner: {0}";
    
        public void ShowWinner(string winner)
        {
            _winnerPanel.gameObject.SetActive(true);
            _winnerNickname.text = string.Format(_winnerTemplate, winner);
        }
    }
}
