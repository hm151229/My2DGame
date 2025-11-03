using UnityEngine;
namespace MyBird
{
    public class CameraController : MonoBehaviour
    {
        #region Variables
        //플레이어 오브젝트
        public Transform player;

        //카메라 위치 조정
        private float offsetX = 1.5f;
        #endregion

        #region Unity Event Method
        // Update is called once per frame
        void Update()
        {
            //플레이어 따라가기
            FollowPlayer();
        }
        #endregion

        #region Custom Method
        //플레이어 따라가기
        void FollowPlayer()
        {
            this.transform.position = new Vector3(player.position.x + offsetX,
                this.transform.position.y, this.transform.position.z);
        }

        #endregion
    }
}
