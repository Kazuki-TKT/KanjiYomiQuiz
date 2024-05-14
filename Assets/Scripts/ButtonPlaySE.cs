using UnityEngine;
using UnityEngine.UI;

namespace KanjiYomi
{
    public class ButtonPlaySE : MonoBehaviour
    {
        void Start()
        {
            Button button = this.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => AuidoManager.Instance.PlaySound_SE(AuidoManager.Instance.buttonSE,0.8f));
        }
    }
}
