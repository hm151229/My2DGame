using UnityEditor.Tilemaps;
using UnityEngine;
namespace My2DGame
{
    /// <summary>
    /// Enemy를 관리하는 클래스
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
    public class EnemyController : MonoBehaviour
    {
        #region Variabels
        //참조
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirections touchingDirections;
        //이동
        //이동 속도
        [SerializeField] private float runSpeed = 4f;
        //이동 방향
        private Vector2 directionVector = Vector2.right;

        //이동 가능한 방향 정의
        public enum WalkableDirection
        {
            Left,
            Right
        }

        //현재 이동 방향
        private WalkableDirection walkDirection = WalkableDirection.Right;
        #endregion

        #region Property
        public WalkableDirection WalkDirection
        {
            get { return walkDirection; }
            private set
            {
                //방향 전환이 일어난 시점
                if (walkDirection != value)
                {
                    //이미지 플립
                    transform.localScale *= new Vector2(-1, 1);

                    //value값에 따라 이동 방향 설정
                    if (value == WalkableDirection.Left)
                    {
                        directionVector = Vector2.left;
                    }
                    else if (value == WalkableDirection.Right)
                    {
                        directionVector = Vector2.right;
                    }
                }

                walkDirection = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();
        }

        private void FixedUpdate()
        {
            //벽 체크
            if (touchingDirections.IsWall && touchingDirections.IsGround)
            {
                Flip();
            }

            //이동하기
            rb2D.linearVelocity = new Vector2(directionVector.x * runSpeed, rb2D.linearVelocityY);
        }
        #endregion

        #region 
        //방향 전환
        void Flip()
        {
            if (WalkDirection == WalkableDirection.Left)
            {
                WalkDirection = WalkableDirection.Right;
            }
            else if (WalkDirection == WalkableDirection.Right)
            {
                WalkDirection = WalkableDirection.Left;
            }
            else
            {
                Debug.Log("Erro Flip Direction");
            }
        }
        #endregion
    }
}
