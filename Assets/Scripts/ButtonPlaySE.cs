using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KanjiYomi
{
    /// <summary>
    /// ボタンコンポーネントのクリックとエンターに音を付与するクラス
    /// </summary>
    public class ButtonPlaySE : MonoBehaviour, IPointerEnterHandler
    {
        void Start()
        {
            Button button = this.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => AuidoManager.Instance.PlaySound_SE(AuidoManager.Instance.buttonClickSE,0.8f));//クリック時の音
        }
        //エンター時の音
        public void OnPointerEnter(PointerEventData eventData)
        {
            AuidoManager.Instance.PlaySound_SE(AuidoManager.Instance.buttonEnterSE, 0.8f);
        }
    }
}
