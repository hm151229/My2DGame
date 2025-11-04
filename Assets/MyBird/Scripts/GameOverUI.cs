using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace MyBird
{
    /// <summary>
    /// 게임오버 UI를 관리하는 클래스
    /// </summary>
    public class GameOverUI : MonoBehaviour
    {
        #region Variables
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI bestscoreText;
        public GameObject newText;

        //메뉴씬
        [SerializeField]
        private string loadToScene = "Title";
        #endregion

        #region Unity Event Method
        //게임오버 UI 값 설정
        private void OnEnable()
        {
            scoreText.text = GameManager.Score.ToString();

            //베스트 스코어 가져오기
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);

            //베스트 스코어와 현재 스코어 비교해서 베스트 스코어 갱신
            if(GameManager.Score > bestScore)
            {
                bestScore = GameManager.Score;
                //베스트 스코어 저장
                PlayerPrefs.SetInt("BestScore", bestScore);

                //UI
                newText.SetActive(true);
            }
            bestscoreText.text = bestScore.ToString();
        }
        #endregion

        #region Custom Method
        //다시하기 버튼 클릭 시 호출
        public void Retry()
        {
            string nowScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(nowScene);
        }
        //메뉴 버튼 클릭 시 호출
        public void Menu()
        {
            SceneManager.LoadScene(loadToScene);
        }
        #endregion
    }
}
