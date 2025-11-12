using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
namespace My2DGame
{
    /// <summary>
    /// 트리거 충돌체에 들어오는 모든 충돌체 감지해서 수집
    /// </summary>
    public class DetectionZone : MonoBehaviour
    {
        #region Variables
        //감지된 충돌체 리스트
        public List<Collider2D> detectedColliders = new List<Collider2D>();

        //모든 충돌체의 갯수가 0이 되는 순간 호출되는 이벤트 함수
        public UnityAction noRemainColliders;
        #endregion

        #region Unity Event Method
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //충돌체가 들어오면 리스트에 추가
            detectedColliders.Add(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //충돌체에서 나가면 리스트에서 제거
            detectedColliders.Remove(collision);

            //더이상 충돌체 리스트에 아무것도 없을 때 등록된 함수 호출
            noRemainColliders?.Invoke();
        }
        #endregion
    }
}
