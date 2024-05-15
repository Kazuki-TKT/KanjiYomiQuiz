using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KanjiYomi
{
    /// <summary>
    /// �{�^���R���|�[�l���g�̃N���b�N�ƃG���^�[�ɉ���t�^����N���X
    /// </summary>
    public class ButtonPlaySE : MonoBehaviour, IPointerEnterHandler
    {
        void Start()
        {
            Button button = this.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => AuidoManager.Instance.PlaySound_SE(AuidoManager.Instance.buttonClickSE,0.8f));//�N���b�N���̉�
        }
        //�G���^�[���̉�
        public void OnPointerEnter(PointerEventData eventData)
        {
            AuidoManager.Instance.PlaySound_SE(AuidoManager.Instance.buttonEnterSE, 0.8f);
        }
    }
}
