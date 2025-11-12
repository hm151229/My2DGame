using TMPro;
using UnityEngine;
namespace My2DGame
{
    /// <summary>
    /// 데미지 효과: 캐릭터 머리 위에 텍스트 띄우기
    /// 위로 이동하기, 이동하면서 페이드 아웃, 페이드 아웃이 끝나면 킬
    /// </summary>
    public class DamageText : MonoBehaviour
    {
        #region Variables
        //참조
        private RectTransform rectTransform;
        private TextMeshProUGUI damageText;

        //이동
        [SerializeField]
        private float moveSpeed = 10f;

        //페이드 효과
        private Color startColor;

        [SerializeField]
        private float fadeTimer= 1f;
        private float countdown = 0f;

        //페이드 지연
        [SerializeField]
        private float delayTimer = 1f;
        private float delayCountdown = 0f;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            rectTransform = GetComponent<RectTransform>();
            damageText = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            //초기화
            countdown = 0f;
            delayCountdown = 0f;
            startColor = damageText.color;
        }

        private void Update()
        {
            //위로 이동
            rectTransform.position += Vector3.up * Time.deltaTime * moveSpeed;

            if (delayCountdown < delayTimer)
            {
                delayCountdown += Time.deltaTime;
                return;
            }

            //페이드 효과
            countdown += Time.deltaTime;

            float alphaValue = startColor.a * (1- (countdown/fadeTimer));
            damageText.color = new Color(startColor.r, startColor.g, startColor.b, alphaValue);

            //페이드 효과 완료 후 킬
            if (countdown >= fadeTimer)
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }
}