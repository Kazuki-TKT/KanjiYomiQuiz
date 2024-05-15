using UnityEngine;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// モンスターを表示する時の演出などを行うクラス
    /// </summary>
    public class EnemySpawnEffect : MonoBehaviour
    {
        //召喚エフェクトの時間
        public float spawnEffectTime = 1;

        //召喚エフェクト
        [SerializeField]
        ParticleSystem ringParticle;
        //モンスターのレンダー
        [SerializeField]
        Renderer _renderer;

        //シェーダープロパティのIDを格納
        int shaderProperty;

        void Start()
        {
            shaderProperty = Shader.PropertyToID("_cutoff");//CutOffのプロパティのIDを取得
        }
        private void OnEnable()
        {
            SpawnEffect();
        }

        /// <summary>
        /// モンスター召喚演出
        /// </summary>
        void SpawnEffect()
        {
            ringParticle.Play();//リングエフェクト
            DOTween.To(() => 0f, value => _renderer.material.SetFloat(shaderProperty, value), 1f, spawnEffectTime+0.5f).SetEase(Ease.Linear);//モンスターのレンダーのCutOffの値を徐々に上げる

        }
        /// <summary>
        /// モンスター非表示演出
        /// </summary>
        public void DespawnEffect(GameObject monster)
        {
            DOTween.To(() => 1f, value => _renderer.material.SetFloat(shaderProperty, value), 0, spawnEffectTime).SetEase(Ease.Linear).OnComplete(()=>monster.SetActive(false));
            //モンスターのレンダーのCutOffを徐々に0にした後、非表示にする
        }
    }
}
