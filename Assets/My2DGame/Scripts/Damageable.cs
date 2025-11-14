using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace My2DGame
{
    /// <summary>
    /// Health를 관리하는 클래스
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        #region Variables
        //참조
        private Animator animator;
        private Renderer renderer;

        [SerializeField]
        private float currentHealth;
        [SerializeField]
        private float maxHealth = 100f;
        //죽음 체크
        private bool isDeath = false;

        //무적모드
        private bool isInvincible = false;
        //무적 모드 타이머
        [SerializeField]
        private float invincibleTimer = 3f;
        private float countdown = 0f;
        //무적 모드 효과
        public Material invincibleMaterial; //무적모드 메터리얼
        private Material oringinMaterial;   //오리지널 메터리얼

        //데미지 입을 때 호출되는 이벤트 함수
        public UnityAction<float, Vector2> hitAction;
        //힐 할 때 호출되는 이벤트 함수
        public UnityAction<float> healAction;

        #endregion

        #region Property
        public float CurrentHealth
        {
            get { return currentHealth; }
            private set
            {
                currentHealth = value;

                if (currentHealth <= 0f)
                {
                    IsDeath = true;
                }
            }
        }

        public bool IsDeath
        {
            get { return isDeath; }
            private set
            {
                isDeath = value;
                animator.SetBool(AnimationString.IsDeath, value);
            }
        }
        public float MaxHealth
        { 
            get { return maxHealth; }
            private set
            {
                maxHealth = value;
            }
        }
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            animator = GetComponent<Animator>();
            renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            //초기화
            CurrentHealth = MaxHealth;
            oringinMaterial = renderer.material;
        }
        private void Update()
        {
            //죽음 체크
            if (IsDeath)
                return;
            //무적 타이머 - 무적 모드일 때
            if (isInvincible)
            {
                countdown += Time.deltaTime;
                if (countdown >= invincibleTimer)
                {
                    //타이머 구현 - 무적 모드 해제
                    isInvincible = false ;
                    renderer.material = oringinMaterial;

                    //타이머 초기화
                    countdown = 0f;
                }
            }
        }
        #endregion

        #region Custom Method
        public void TakeDamage(float damage, Vector2 knockback)
        {
            //죽음 체크, 무적 체크
            if (isDeath || isInvincible)
                return;
                
            CurrentHealth -= damage;
            Debug.Log(CurrentHealth);

            isInvincible = true;
            //무적 모드 효과
            renderer.material = invincibleMaterial != null ? invincibleMaterial : oringinMaterial;

            //애니메이션
            animator.SetTrigger(AnimationString.HitTrigger);

            //데미지효과(knockback)
            //hitAction 이벤트에 등록된 함수 호출
            hitAction?.Invoke(damage, knockback);
            //데미지 텍스트 연출 효과
            CharacterEvents.characterDamaged?.Invoke(this.transform, damage);
        }
        //힐 하기
        public bool Heal(float healAmount)
        {
            //죽음 체크, 체력 만땅 체크
            if(CurrentHealth >= MaxHealth || isDeath)
                return false;
            //리얼 힐 값 구하기
            float maxHeal = MaxHealth - CurrentHealth;  //최대로 힐 할 수 있는 값
            float realHeal = (maxHeal<healAmount)? maxHeal : healAmount;    //실제로 힐 하는 값

            CurrentHealth += realHeal;
            Debug.Log(CurrentHealth);

            //healAction null체크 :healAction에 등록된 함수가 있는지 여부 체크
            /*if (healAmount != null)
            {
                healAction.Invoke(realHeal);
            }*/
            //healAction?.Invoke(realHeal);

            CharacterEvents.characterHeal.Invoke(this.transform, realHeal);
            return true;
        }
        #endregion

    }
}
