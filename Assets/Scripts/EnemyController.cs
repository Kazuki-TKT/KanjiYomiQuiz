using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

namespace KanjiYomi
{
    /// <summary>
    /// 敵モンスターの動きを管理するクラス
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        //モンスターのデータ
        [SerializeField]
        List<MonsterData> monsterDatas;

        //現在のモンスターを扱うデータ
        MonsterData currentMonsterData;
        public MonsterData CurrentMonsterData { get => currentMonsterData; }

        //画面効果
        public PostProcessVolume postProcessVolume;
        ChromaticAberration chromaticAberrationLayer;

        //爆発エフェクトとショックウェーブエフェクト
        public ParticleSystem particleExprosion, shockWave;
        public ParticleSystem[] particleTinyExprosion;

        //Animatorのproperty
        const string ATTACK = "Attack";
        const string DIE = "Die";

        //召喚時と爆発音のSE
        public AudioClip summonsClip, explosionClip;

        private void Start()
        {
            postProcessVolume.profile.TryGetSettings(out chromaticAberrationLayer);
        }

        /// <summary>
        /// モンスターを召喚した後攻撃を行うメソッド
        /// </summary>
        /// <param name="difficulty">問題の難易度</param>
        public void MonsterSpawnAndAttack(Difficulty difficulty, CancellationToken token)
        {
            currentMonsterData = GetMonster(difficulty);//現在のモンスターを取得し代入
            SpawnAndAttackAnimation(currentMonsterData.enemySpawn.spawnEffectTime + 0.5f, token).Forget();
        }

        /// <summary>
        /// モンスター死亡時に行うメソッド
        /// </summary>
        public void MonsterDie(CancellationToken token)
        {
            DieAnimation(currentMonsterData.enemySpawn.spawnEffectTime, token).Forget();
        }

        /// <summary>
        /// モンスターが撤退した時に行うメソッド
        /// </summary>
        public void MonsterEscape()
        {
            currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);
        }

        /// <summary>
        /// モンスターを召喚し、攻撃を行うアニメーションメソッド
        /// </summary>
        /// <returns></returns>
        async UniTask SpawnAndAttackAnimation(float time, CancellationToken token)
        {
            try
            {
                currentMonsterData.monsterObject.SetActive(true);//敵を表示
                AuidoManager.Instance.PlaySound_SE(summonsClip);//召喚SE

                //--表示アニメーション時間分待機
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);

                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(ATTACK);//アタック攻撃
                shockWave.Play();//ショックウェーブのパーティクル
                foreach (AudioClip clip in currentMonsterData.screamClip)//SEを鳴らす
                {
                    AuidoManager.Instance.PlaySound_SE(clip);
                }
                _ = DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 1, 0.5f).SetEase(Ease.InSine);//ChromaticAberrationLayerを使用して迫力を出す

                //--1秒待機
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);

                _ = DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 0, 0.5f)
            .SetEase(Ease.Linear);//ChromaticAberrationLayerの値を元に戻す
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("敵の行動がキャンセルされました。");
            }
            catch (Exception ex)
            {
                Debug.LogError($"エラーが発生しました: {ex.Message}");
            }
        }

        async UniTask DieAnimation(float time, CancellationToken token)
        {
            try
            {
                PlayerController.Instance.PlayerAttack(1);//プレイヤーの攻撃
                foreach (ParticleSystem particle in particleTinyExprosion)//攻撃がヒットする演出
                {
                    particle.Play();
                }

                //--1秒待機
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

                AuidoManager.Instance.PlaySound_SE(explosionClip);//爆発音
                particleExprosion.Play();//爆発エフェクト
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(DIE);//モンスターが倒れるアニメーション

                //--倒れるまで指定秒待機
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);

                currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);//敵を非表示
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("敵の行動がキャンセルされました。");
            }
            catch (Exception ex)
            {
                Debug.LogError($"エラーが発生しました: {ex.Message}");
            }
        }

        //MonstaerDataから選んだモンスターを返すメソッド
        MonsterData GetMonster(Difficulty difficulty)
        {
            var monsterData = monsterDatas.Find(monsterData => monsterData.monsterDifficulty == difficulty);
            return monsterData;
        }


        /// <summary>
        /// モンスターの情報をまとめたクラス
        /// </summary>
        [System.Serializable]
        public class MonsterData
        {
            public Difficulty monsterDifficulty;//モンスターの難易度
            public GameObject monsterObject;//モンスターのオブジェクト
            public EnemySpawnEffect enemySpawn;//モンスターの召喚エフェクトを司るクラス
            public AudioClip[] screamClip;//モンスターが出す音
        }
    }
}