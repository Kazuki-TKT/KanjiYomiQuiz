using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// シーンをロードするボタンに貼り付ける
    /// </summary>
    public class SceneLoadButton : MonoBehaviour
    {
        /// <summary>
        /// ゲームシーンをロードするメソッド
        /// </summary>
        public void ToGameSceneLoad()
        {
            SceneLoader.Instance.LoadSceneAsync("InGames", GameState.Playing).Forget();
        }

        /// <summary>
        /// タイトルシーンをロードするメソッド
        /// </summary>
        public void ToTitleSceneLoad()
        {
            SceneLoader.Instance.LoadSceneAsync("TitleScene", GameState.Title).Forget();
        }
    }
}
