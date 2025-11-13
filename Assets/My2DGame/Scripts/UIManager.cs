using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
namespace My2DGame
{
    /// <summary>
    /// 플레이 씬 UI를 관리하는 클래스
    /// 데미지 텍스트 연출,
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        #region Variables
        //참조
        private Canvas gameCanvas;
        public GameObject damageTextPrefab;
        public GameObject HealTextPrefab;
        #endregion

        #region Unity Event Method
        private void Awake()
        {
            //참조
            gameCanvas = FindFirstObjectByType<Canvas>();
        }

        private void OnEnable()
        {
            //이벤트 함수 등록
            CharacterEvents.characterDamaged += CharacterTakeDamage;
            CharacterEvents.characterHeal += CharacterHeal;
        }

        private void OnDisable()
        {
            //이벤트 함수 해제
            CharacterEvents.characterDamaged -= CharacterTakeDamage;
            CharacterEvents.characterHeal -= CharacterHeal;
        }
        #endregion

        #region Custom Method
        //캐릭터가 데미지 입을 때 호출되는 함수 - 데미지 텍스트 연출
        //매개변수로 캐릭터의 오브젝트, 데미지량을 입력 받아 처리
        public void CharacterTakeDamage(Transform character, float damageReceived)
        {
            //캐릭터 머리 위 위치 가져오기
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.position);

            GameObject textGo = Instantiate(damageTextPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 70f, spawnPosition.z), 
                Quaternion.identity, gameCanvas.transform);

            //데미지 값 셋팅
            TextMeshProUGUI damageText = textGo.GetComponent<TextMeshProUGUI>();
            damageText.text = damageReceived.ToString();
        }
        //캐릭터가 힐 할 때 호출되는 함수 - 힐 텍스트 연출
        //매개변수로 캐릭터의 오브젝트, 힐량을 입력 받아 처리
        public void CharacterHeal(Transform character, float healAmount)
        {
            //캐릭터 머리 위 위치 가져오기
            Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.position);

            GameObject textGo = Instantiate(HealTextPrefab, new Vector3(spawnPosition.x, spawnPosition.y + 70f, spawnPosition.z),
                Quaternion.identity, gameCanvas.transform);

            //힐 값 셋팅
            TextMeshProUGUI HealText = textGo.GetComponent<TextMeshProUGUI>();
            HealText.text = healAmount.ToString();
        }
        #endregion
    }
}