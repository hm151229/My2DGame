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
        private Damageable damageable;

        //적 감지 
        public DetectionZone detectionZone;
        public DetectionZone detectionGround;

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

        //감속 Lerp 계수
        [SerializeField] private float stopRate = 0.2f;

        //적 감지 - 타겟이 있다
        private bool hasTarget = false; 
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
        //애니메이터의 파라미터 값(CannotMove) 읽어오기
        public bool CannotMove
        {
            get
            {
                return animator.GetBool(AnimationString.CannotMove);
            }
        }
        //적 감지
        public bool HasTarget
        {
            get { return hasTarget; }
            set 
            { 
                hasTarget = value; 
                animator.SetBool(AnimationString.HasTarget, value);
            }
        }
        //공격 쿨 타임
        public float Cooldowntime
        {
            get { return animator.GetFloat(AnimationString.CooldownTime); }
            set { animator.SetFloat(AnimationString.CooldownTime, value); }
        }

        //속도 잠금 - 애니메이션 파라미터 값 읽기
        public bool LockVelocity
        {
            get { return animator.GetBool(AnimationString.LockVelocity); }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            touchingDirections = GetComponent<TouchingDirections>();
            damageable = GetComponent<Damageable>();

            //이벤트 함수 등록
            damageable.hitAction += OnHit;
            //DetectionZone 이벤트 함수 등록
            detectionGround.noRemainColliders += OnCliffDetection;
        }

        private void Update()
        {
            //적감지
            HasTarget = detectionZone.detectedColliders.Count > 0;
            //공격 쿨 다운 타이머
            if (Cooldowntime > 0f)
            {
                Cooldowntime -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            //벽 체크
            if (touchingDirections.IsWall && touchingDirections.IsGround || detectionGround.detectedColliders.Count <0)
            {
                Flip();
            }

            //이동하기
            if(LockVelocity == false)
            {
                if (CannotMove)
                {
                    //감속 rb2D.linearVelocityX -> 0
                    rb2D.linearVelocity = new Vector2(Mathf.Lerp(rb2D.linearVelocityX, 0f, stopRate), rb2D.linearVelocityY);
                }
                else
                {
                    rb2D.linearVelocity = new Vector2(directionVector.x * runSpeed, rb2D.linearVelocityY);
                }
            }
            
        }
        #endregion

        #region Custom Method
        
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

        //데미지 이벤트에 등록되는 함수
        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.linearVelocity = new Vector2(knockback.x, rb2D.linearVelocity.y + knockback.y);
        }

        //디텍션 이벤트에 등록되는 함수
        public void OnCliffDetection()
        {
            if (touchingDirections.IsGround)
            {
                Flip();
            }
        }
        #endregion
    }
}
