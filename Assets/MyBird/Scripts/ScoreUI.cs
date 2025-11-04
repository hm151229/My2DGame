using UnityEngine;
using TMPro;
namespace MyBird
{
    public class ScoreUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI scoreCountText;
        #endregion

        #region
        private void Update()
        {
            scoreCountText.text = GameManager.Score.ToString();
        }
        #endregion
    }
}
