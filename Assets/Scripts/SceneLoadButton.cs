using UnityEngine;

namespace KanjiYomi
{
    /// <summary>
    /// �V�[�������[�h����{�^���ɓ\��t����
    /// </summary>
    public class SceneLoadButton : MonoBehaviour
    {
        /// <summary>
        /// �Q�[���V�[�������[�h���郁�\�b�h
        /// </summary>
        public void ToGameSceneLoad()
        {
            SceneLoader.Instance.LoadSceneAsync("InGames", GameState.Playing).Forget();
        }

        /// <summary>
        /// �^�C�g���V�[�������[�h���郁�\�b�h
        /// </summary>
        public void ToTitleSceneLoad()
        {
            SceneLoader.Instance.LoadSceneAsync("TitleScene", GameState.Title).Forget();
        }
    }
}
