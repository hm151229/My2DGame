using UnityEngine;
namespace MyBird
{
    /// <summary>
    /// 게임 전체(흐름)를 관리하는 클래스
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        #region Variables
        //게임 시작 여부 체크
        private static bool isStart;

        //게임 오버 체크
        private static bool isGameOver;

        //게임 스코어
        private static int score;
        #endregion

        #region Property
        public static bool IsStart
        {
            get { return isStart; }
            set { isStart = value; }
        }

        public static bool IsGameOver
        {
            get { return isGameOver; }
            set { isGameOver = value;  }
        }

        public static int Score
        {
            get { return score; }
            set { score = value; }
        }
        #endregion

        #region Unity Event Method
        private void Start()
        {
            isStart = false;
            isGameOver = false;
            score = 0;
        }
        #endregion
    }
}