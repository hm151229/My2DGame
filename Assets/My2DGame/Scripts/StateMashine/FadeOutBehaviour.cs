using UnityEngine;
namespace My2DGame
{
    /// <summary>
    /// 애니메이션 상태 동안 페이드 아웃 후 킬
    /// </summary>
    public class FadeOutBehaviour : StateMachineBehaviour
    {
        #region Variables
        //참조
        private SpriteRenderer spriteRenderer;
        private GameObject removeObject;

        //딜레이 타이머
        public float delayTimer = 1f;
        private float delayCountdown = 0f;
        //페이드 타이머
        public float fadeTimer = 1f;
        private float countdown = 0f;

        private Color startColor;
        #endregion

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //참조
            spriteRenderer = animator.GetComponent<SpriteRenderer>();
            removeObject = animator.gameObject;
            //초기화
            countdown = 0f;
            delayCountdown = 0f;
            startColor = spriteRenderer.color;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //딜레이 체크
            if (delayTimer > delayCountdown)
            {
                delayCountdown += Time.deltaTime;
                return;
            }
            //페이드 아웃 타이머 1 -> 0
            countdown += Time.deltaTime;
            //countdown / fadeTimer => 0 -> 1
            float alphaValue = startColor.a * (1 - (countdown / fadeTimer));
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alphaValue);
            if (countdown >= fadeTimer)
            {
                //타이머 기능

                Destroy(removeObject);
                //타이머 초기화
                countdown = 0f;
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}