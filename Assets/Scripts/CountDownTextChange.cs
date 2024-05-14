using UnityEngine;
using TMPro;
using DG.Tweening;

namespace KanjiYomi
{
    public class CountDownTextChange : MonoBehaviour
    {
        TextMeshProUGUI countDownText;
        [SerializeField]
        CanvasGroup flashRedCanvas;

        public AudioClip alertClip;
        // Start is called before the first frame update
        void Start()
        {
            countDownText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void DefaultText()
        {
            countDownText.text = 5.ToString();
        }
        public void ChangeText(int num)
        {
            countDownText.text = num.ToString();
        }

        public void Flash()
        {
            flashRedCanvas.DOFade(1, 0.1f).OnComplete(() => flashRedCanvas.DOFade(0, 0.4f));
            AuidoManager.Instance.PlaySound_SE(alertClip);
        }
    }
}
