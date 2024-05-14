using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace KanjiYomi
{
    public class SceneLoader : MonoBehaviour
    {
        //public CanvasGroup fadeCanvasGroup; // フェード用のCanvasGroup
        //public TextMeshProUGUI loadingText; // ローディングテキスト表示用
        //public Button[] sceneChangeButton;
        //public string sceneName;
        //public GameState gameChangeState;

        //private void Start()
        //{
        //    // 初期状態でフェードキャンバスを透明に設定
        //    fadeCanvasGroup.alpha = 0;
        //    foreach(Button button in sceneChangeButton)
        //    {
        //        button.onClick.AddListener(() => LoadSceneAsync(sceneName).Forget());
        //    }
        //}

        //public async UniTaskVoid LoadSceneAsync(string sceneName)
        //{
        //    await FadeOutAsync(); // フェードアウト
        //    loadingText.text = "Loading..."; // ローディングテキスト表示
        //                                     // シーンを非同期でロード
        //    var sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName);

        //    // シーンのロードが完了するまで待機
        //    await sceneLoadOperation;
        //    // シーンのロード後の処理
        //    GameManager.Instance.UpdateGameState(gameChangeState);
        //    //await FadeInAsync(); // フェードイン
        //    //loadingText.text = ""; // ローディングテキストをクリア
        //}

        //private async UniTask FadeOutAsync()
        //{
        //    fadeCanvasGroup.blocksRaycasts = true; // フェード中はUIの操作をブロック

        //    float fadeDuration = 1f; // フェードの時間
        //    float timer = 0f;

        //    while (timer <= fadeDuration)
        //    {
        //        timer += Time.deltaTime;
        //        fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
        //        await UniTask.Yield(PlayerLoopTiming.Update);
        //    }
        //}

        //private async UniTask FadeInAsync()
        //{
        //    float fadeDuration = 1f; // フェードの時間
        //    float timer = 0f;

        //    while (timer <= fadeDuration)
        //    {
        //        timer += Time.deltaTime;
        //        fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
        //        await UniTask.Yield(PlayerLoopTiming.Update);
        //    }

        //    fadeCanvasGroup.blocksRaycasts = false; // フェード完了後はUIの操作を許可
        //}
    }
}
