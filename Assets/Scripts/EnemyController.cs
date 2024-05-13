using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System;
using Unity.VisualScripting.Antlr3.Runtime;

namespace KanjiYomi
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        List<MonsterData> monsterDatas;

        //現在のモンスターを扱うデータ
        MonsterData currentMonsterData;
        public MonsterData CurrentMonsterData { get => currentMonsterData; }

        const string ATTACK = "Attack";
        const string DIE = "Die";
        private void Start()
        {

        }
        public void MonsterSpawnAndAttackAnimation(Difficulty difficulty, CancellationToken token)
        {
            currentMonsterData = GetMonster(difficulty);
            SpawnAndAttackAnimation(currentMonsterData.enemySpawn.spawnEffectTime, token).Forget();
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
                currentMonsterData.monsterObject.SetActive(true);
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(ATTACK);
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
                currentMonsterData.monsterObject.GetComponent<Animator>().SetTrigger(DIE);
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token);
                currentMonsterData.enemySpawn.DespawnEffect(currentMonsterData?.monsterObject);
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

        MonsterData GetMonster(Difficulty difficulty)
        {
            var monsterData = monsterDatas.Find(monsterData => monsterData.monsterDifficulty == difficulty);
            return monsterData;
        }
    }

    [System.Serializable]
    public class MonsterData
    {
        public Difficulty monsterDifficulty;
        public GameObject monsterObject;
        public EnemySpawnEffect enemySpawn;
    }
}
