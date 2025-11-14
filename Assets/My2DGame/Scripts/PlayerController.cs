using UnityEngine;
using UnityEngine.InputSystem;
namespace My2DGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //이동 속도
        [SerializeField] private float walkSpeed = 5f;   //걷는 속도
        [SerializeField] private float runSpeed = 7f;    //달리는 속도
        private float airSpeed = 2f;                     //공중 속도

        //점프
        [SerializeField] private float jumpForce = 7f;  // 점프 힘

        //참조
        private Rigidbody2D rb2D;
        private Animator animator;
        private TouchingDirections touchingDirections;
        private Damageable damageable;
        //잔상 효과
        private TrailEffect trailEffect;

        //입력값
        private Vector2 inputMove = Vector2.zero;
        //반전
        private bool isFacingRight = true;
        //걷기
        private bool isMove = false;
        private bool isRun = false;
        private bool isGrounded = false;
        private bool isAttack = false;

        #endregion

        #region Property
        public bool IsFacingRight
        {
            get { return isFacingRight; }
            set 
            {
                //반전 구현
                if(isFacingRight != value)
                {
                    this.transform.localScale *= new Vector2(-1, 1);
                }
                isFacingRight = value; 
            }
        }
        public bool IsMove
        {
            get { return isMove; }
            private set 
            {

                isMove = value;
                animator.SetBool(AnimationString.IsMove, value);
            }
        }

        public bool IsRun
        {
            get { return isRun; }
            private set
            {
                isRun = value;
                animator.SetBool(AnimationString.IsRun, value);
            }
        }

        public bool IsGrounded
        {
            get { return isGrounded; }
            set
            {
                isGrounded = value;
                animator.SetBool(AnimationString.IsGrounded, value);
            }
        }
        //현재 이동 속도 - 읽기 전용
        public float CurrentMoveSpeed
        {
            get 
            {
                if(CannotMove)  //애니메이터 파라미터 값 읽어오기
                    return 0f;

                if (IsMove &&touchingDirections.IsWall == false) //이동가능
                {
                    if (touchingDirections.IsGround)
                    {
                        if (IsRun)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airSpeed;
                    }
                }
                else //이동 불가
                {
                    return 0f;
                }
            }
        }

        //애니메이션의 파라미터 값(CannotMove) 읽어오기
        public bool CannotMove
        {
            get
            {
                return animator.GetBool(AnimationString.CannotMove);
            }
        }

        public bool LockVelocity
        {
            get
            {
                return animator.GetBool(AnimationString.LockVelocity);
            }
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
            trailEffect = GetComponent<TrailEffect>();

            //이벤트 함수 등록
            damageable.hitAction += OnHit;
        }
        private void FixedUpdate()
        {
            if (LockVelocity == false)
            {
                // 좌우 이동
                rb2D.linearVelocity = new Vector2(inputMove.x * CurrentMoveSpeed, rb2D.linearVelocity.y);
            }
            //점프 애니메이션
            animator.SetFloat(AnimationString.YVelocity, rb2D.linearVelocity.y);
        }
        #endregion

        #region Cudtom Method
        //방향 전환 
        void SetFacingDirection(Vector2 moveInput)
        {
            if(moveInput.x > 0f && IsFacingRight == false)  //오른쪽으로 이동 
            {
                IsFacingRight = true;
            }
            else if (moveInput.x < 0f && IsFacingRight == true) //왼쪽으로 이동
            {
                IsFacingRight = false;
            }
        }
        //이동 입력 처리
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
            if (damageable.IsDeath == false)
            {
                IsMove = inputMove != Vector2.zero;
                //방향 전환
                SetFacingDirection(inputMove);
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)    //버튼을 눌렀을 때 
            {
                IsRun = true;

                //잔상 효과
                if (trailEffect != null)
                {
                    trailEffect.StartTrailEffect();
                }
            }
            else if (context.canceled)  //버튼을 땔 때
            {
                IsRun = false;
            }
            //방향 전환
            SetFacingDirection(inputMove);
        }
        //점프 입력 처리
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started && touchingDirections.IsGround && CannotMove == false)
            {
                animator.SetTrigger(AnimationString.JumpTrigger);

                rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);

                //잔상 효과
                if (trailEffect != null)
                {
                    trailEffect.StartTrailEffect();
                }
            }
        }
        //공격 입력 처리
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.started && touchingDirections.IsGround)
            {
                animator.SetTrigger(AnimationString.AttackTrigger);
            }
        }

        public void OnBowAttack(InputAction.CallbackContext context)
        {
            //Debug.Log("활");
            if (context.started && touchingDirections.IsGround)
            {
                animator.SetTrigger(AnimationString.BowAttackTrigger);
            }
        }

        //데미지 이벤트에 등록되는 함수
        public void OnHit(float damage, Vector2 knockback)
        {
            rb2D.linearVelocity = new Vector2 (knockback.x, rb2D.linearVelocity.y + knockback.y);
        }
        #endregion
    }
}