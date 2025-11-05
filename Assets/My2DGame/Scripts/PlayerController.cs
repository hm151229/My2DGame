using UnityEngine;
using UnityEngine.InputSystem;
namespace My2DGame
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        //이동 속도
        [SerializeField]
        private float walkSpeed = 5f;   //걷는 속도
        //참조
        private Rigidbody2D rb2D;
        //입력값
        private Vector2 inputMove = Vector2.zero;

        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rb2D = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            // 물리 이동
            rb2D.linearVelocity = new Vector2(inputMove.x * walkSpeed, rb2D.linearVelocity.y);

        }
        #endregion

        #region Cudtom Method
        public void OnMove(InputAction.CallbackContext context)
        {
            inputMove = context.ReadValue<Vector2>();
        }
        #endregion
    }
}