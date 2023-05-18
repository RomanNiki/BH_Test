using PlayerComponents;
using UnityEngine;

namespace UI
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] private ScoreBoardItem _scoreBoardItemPrefab;

        public static ScoreBoard Instance { get; private set; }

        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            if (this == Instance)
            {
                Instance = null;
            }
        }

        public ScoreBoardItem CreateScoreBoardItem(Player player)
        {
           var scoreBoardItem = Instantiate(_scoreBoardItemPrefab, transform);
           scoreBoardItem.Init(player);
           return scoreBoardItem;
        }

        public void SetActive(bool active)
        {
            transform.gameObject.SetActive(active);
        }
    }
}