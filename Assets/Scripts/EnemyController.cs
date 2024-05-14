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
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        List<MonsterData> monsterDatas;

        //現在のモンスターを扱うデータ
        MonsterData currentMonsterData;
        public MonsterData CurrentMonsterData { get => currentMonsterData; }

        //画面効果
        public PostProcessVolume postProcessVolume;
        ChromaticAberration chromaticAberrationLayer;

        //爆発エフェクト
        public ParticleSystem particleExprosion,shockWave;
        public ParticleSystem[] particleTinyExprosion;

        //Animatorのproperty
        const string ATTACK = "Attack";
        const string DIE = "Die";

        public AudioClip summonsClip, explosionClip;
        private void Start()
        {
            postProcessVolume.profile.TryGetSettings(out chromaticAberrationLayer);
        }
        public void MonsterSpawnAndAttackAnimation(Difficulty difficulty, CancellationToken token)
        {
            currentMonsterData = GetMonster(difficulty);
            SpawnAndAttackAnimation(currentMonsterData.enemySpawn.spawnEffectTime+0.5f, token).Forget();
        }
        public void MonsterDie(CancellationToken token)
        {
            DieAnimation(currentMonsterData.enemySpawn.spawnEffectTime, token).Forget();
        }

        public void MonsterEscape()
        {
            currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);
        }

        async UniTask SpawnAndAttackAnimation(float time,  CancellationToken token)
        {
            try
            {
                currentMonsterData.monsterObject.SetActive(true);//敵を表示
                AuidoManager.Instance.PlaySound_SE(summonsClip);
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);//表示アニメーション時間分待機
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(ATTACK);//アタック攻撃
                shockWave.Play();
                foreach (AudioClip clip in currentMonsterData.screamClip)//SEを鳴らす
                {
                    AuidoManager.Instance.PlaySound_SE(clip);
                }
                DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 1, 0.5f).SetEase(Ease.InSine);//ポストプロセスを使用して迫力を出す
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
                DOTween.To(() => chromaticAberrationLayer.intensity.value, x => chromaticAberrationLayer.intensity.value = x, 0, 0.5f)
            .SetEase(Ease.Linear);
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
                foreach(ParticleSystem particle in particleTinyExprosion)
                {
                    particle.Play();
                }

                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                AuidoManager.Instance.PlaySound_SE(explosionClip);//爆発音
                particleExprosion.Play();//爆発
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(DIE);//倒れる
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


        [System.Serializable]
    public class MonsterData
    {
        public Difficulty monsterDifficulty;
        public GameObject monsterObject;
        public EnemySpawnEffect enemySpawn;
            public AudioClip[] screamClip;
    }
    }
}