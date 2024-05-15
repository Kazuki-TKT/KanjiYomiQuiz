using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// シーン移動用のシングルトンパターンのクラス
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;

        // フェード用のCanvasGroup
        public CanvasGroup fadeCanvasGroup;

        // ローディングテキスト表示用
        TextMeshProUGUI loadingText;

        //Fadeが終わったかどうかの真偽地
        public bool fadeOut;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            //初期化
            fadeCanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
            fadeCanvasGroup.alpha = 0;
            loadingText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }

        public async UniTaskVoid LoadSceneAsync(string sceneName,GameState gameState)
        {
            
            await FadeOutAsync(1); // フェードアウト
            loadingText.text = "Loading..."; // ローディングテキスト表示
            var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);// シーンを非同期でロード
            await sceneLoadOperation;// シーンのロードが完了するまで待機
            await FadeInAsync(1);// フェードイン
            GameManager.Instance.UpdateGameState(gameState);// 指定のステートにチェンジ
        }

        private async UniTask FadeOutAsync(float duration)
        {
            fadeCanvasGroup.blocksRaycasts = true; // フェード中はUIの操作をブロック

            //--FadeOutが完了するまで待機
            await fadeCanvasGroup.DOFade(1, duration).SetEase(Ease.Linear);
        }

        private async UniTask FadeInAsync(float duration)
        {
            //--FadeInが完了するまで待機
            await fadeCanvasGroup.DOFade(0, duration).SetEase(Ease.Linear);

            fadeCanvasGroup.blocksRaycasts = false; // フェード完了後はUIの操作を許可

        }
    }
}
