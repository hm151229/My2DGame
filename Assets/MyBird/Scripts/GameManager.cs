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
        #endregion

        #region Property
        public static bool IsStart
        {
            get { return isStart; }
            set { isStart = value; }
        }
        #endregion

        #region Unity Event Method
        //isStart = false;
        #endregion
    }
}