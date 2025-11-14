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

        //공격 데미지 이펙트 프리팹
        public GameObject damageEffectPrefab;
        [SerializeField]
        private Vector3 effectOffset = Vector3.zero;
        #endregion

        #region Unity Event Method
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log($"적에게 {attackDamage} 데미지를 주었다");
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                //넉백 효과 방향 설정
                Transform directionTrans = this.transform.parent != null? this.transform.parent : this.transform;
                Vector2 deliverednockback = directionTrans.localScale.x > 0f ?
                    knockback : new Vector2(-knockback.x, knockback.y);
                damageable.TakeDamage(attackDamage, deliverednockback);

                //데미지 이펙트 처리
                if (damageEffectPrefab)
                {
                    Vector3 offeset = directionTrans.localScale.x > 0f ?
                    effectOffset : new Vector3(-effectOffset.x, effectOffset.y, effectOffset.z);

                    GameObject effectGo = Instantiate(damageEffectPrefab, this.transform.position, Quaternion.identity);
                    Destroy(effectGo, 0.3f);
                }
            }
        }
        #endregion

    }
}