using UnityEngine;
namespace MyBird
{
    /// <summary>
    /// 플에이어 캐릭터(Bird)를 관리하는 클래스
    /// 점프, 이동, 충돌
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Variables
        //참조
        private Rigidbody2D rb2D;

        //대기
        [SerializeField]
        private float readyForce = 5f;

        //점프
        private bool keyJump = false;
        [SerializeField]
        private float jumpForce = 5f;

        //회전
        private Vector3 birdRotation = Vector3.zero;    //회전값
        [SerializeField]
        private float upRotate = 5f;                    //위로 회전하는 스피드
        [SerializeField]
        private float downRotate = -5f;                 //아래로 회전하는 스피드

        //이동
        [SerializeField]
        private float moveSpeed = 5f;                   //이동속도
        #endregion
        #region Unity Event Method 
        private void Start()
        {
            rb2D = this.GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            //입력 처리
            InputBird();

            //시작 여부 체크
            if (GameManager.IsStart == false)
                return;

            //버드 회전
            RotateBird();
            //버드 이동
            MoveBird();
        }

        private void FixedUpdate()
        {
            //시작 여부 체크
            if (GameManager.IsStart == false)
            {
                //버드 대기
                ReadyBird();
                return;
            }
            //점프하기
            if (keyJump)
            {
                JumpBird();
                //Debug.Log("점프하기");
                keyJump = false;
            }
        }
        #endregion

        #region Cudtom Method
        //입력 처리
        void InputBird()
        {
            //스페이스 키 OR 마우스 왼클릭 입력받기
            keyJump |= Input.GetKeyUp(KeyCode.Space);
            keyJump |= Input.GetMouseButtonDown(0);

            //플레이어 이동 시작
            if(GameManager.IsStart == false && keyJump == true)
            {
                GameManager.IsStart = true;
            }
        }
        //버드 대기
        void ReadyBird()
        {
            if (rb2D.linearVelocityY < 0f)
            {
                rb2D.linearVelocity = Vector2.up * readyForce;
            }
        }

        //버드 점프하기
        void JumpBird()
        {
            //힘을 이용하여 오브젝트를 위로 이동
            //rb2D.AddForce(Vector2.up * jumpForce);
            //rb2D.linearVelocity을 이용하여 오브젝트를 위로 이동

            rb2D.linearVelocity = Vector2.up * jumpForce;
        }

        //버드 회전하기
        void RotateBird()
        {
            //-점프해서 올라갈 때 최대 +30도 까지 회전
            //-내려갈 때 최소 - 90도까지 회전
            float degree = 0f;  //멈추어 있을 때
            if(rb2D.linearVelocity.y > 0f)  //올라갈 때
            {
                degree = upRotate;
            }
            else if(rb2D.linearVelocity.y <0f)  //내려갈 때
            {
                degree = downRotate;
            }

            birdRotation = new Vector3(0f, 0f, Mathf.Clamp(birdRotation.z + degree, -90f, 30f));
            transform.eulerAngles = birdRotation;
        }

        //버드 이동
        void MoveBird()
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed, Space.World );
        }
        #endregion
    }
}
