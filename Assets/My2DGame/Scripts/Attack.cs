using UnityEngine;
namespace My2DGame
{
    /// <summary>
    /// HitBox에 충돌한 적에게 데미지를 주는 클래스
    /// </summary>
    public class Attack : MonoBehaviour
    {
        #region Variables
        //공격 시 적에게 주는 데미지 양
        [SerializeField]
        private float attackDamage = 10f;

        //공격 시 넉백 효과
        [SerializeField]
        public Vector2 knockback = Vector2.zero;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"적에게 {attackDamage} 데미지를 주었다");
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                //넉백 효과 방향 설정
                Vector2 deliverednockback = this.transform.parent.localScale.x > 0f ?
                    knockback : new Vector2(-knockback.x, knockback.y);
                damageable.TakeDamage(attackDamage, deliverednockback);
            }
        }
        #endregion

    }
}