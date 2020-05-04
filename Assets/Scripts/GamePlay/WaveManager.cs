using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Factories;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class WaveManager
    {
        private EnemyFactory _enemyFactory;
        private readonly EnemyWave[] _waves;
        private readonly DifficultyManager _difficultyManager;
        private readonly GameObject _defaultSpawnPoint;

        private CurrentWave _currentWave;
        internal CurrentWave CurrentWave => _currentWave;
        public bool Ended { get; private set; } = false;

        public WaveManager(EnemyWave[] waves, DifficultyManager difficultyManager, GameObject defaultSpawnPoint)
        {
            _enemyFactory = EnemyFactory.Instance;

            _waves = waves;
            _difficultyManager = difficultyManager;
            _defaultSpawnPoint = defaultSpawnPoint;

            _currentWave = new CurrentWave(0, _waves.First());
            LoadPaths();
        }

        // public
        public void ExecuteCurrentWave()
        {
            if (Ended) return;
            if (_currentWave.IsFullyCreated) return;

            var enemyGO = CreateEnemy();
            if (enemyGO != null)
            {
                var enemy = enemyGO.GetComponent<Enemy>();
                SetMovementMode(enemy);
                _currentWave.AddEnemyCreate(enemy);
            }
        }

        public void MoveNext()
        {
            var nextWaveIndex = _currentWave.Index + 1;
            Ended = nextWaveIndex >= _waves.Length;
            if (Ended)
                return;

            _currentWave = new CurrentWave(nextWaveIndex, _waves[nextWaveIndex]);
        }

        // helper
        private void LoadPaths()
        {
            //foreach (var wave in _waves.Where(w => w.Type == EnemyMovementType.Path))
            //{
            //    wave.Path = GameObject.Instantiate(wave.Path, wave.Path.GetComponent<PathCreator>().bezierPath.GetPoint(0), Quaternion.identity);
            //}
        }

        private void SetMovementMode(Enemy enemy)
        {
            enemy.Spawn(_difficultyManager.Difficulty, CurrentWave.Definition.Mode, CurrentWave.Definition.Path);
        }

        private GameObject CreateEnemy()
        {
            if (_difficultyManager.CanCreateEnemy(_currentWave.Definition.Mode))
            {
                _difficultyManager.NotifyEnemyTypeSelected(_currentWave.Definition.EnemyType);
                var position = ScreenHelper.GetRandomScreenPoint(y: _defaultSpawnPoint.transform.position.y);
                return _enemyFactory.Create(_currentWave.Definition.EnemyType, position);
            }
            return null;
        }
    }

    internal class CurrentWave
    {
        private readonly List<Enemy> _enemies = new List<Enemy>();

        public int Index { get; }
        public int EnemiesCreated { get; private set; }
        public bool IsFullyCreated => EnemiesCreated >= Definition.EnemyCount;
        public bool Ended => IsFullyCreated && _enemies.All(e => e?.Destroyed ?? true);
        public EnemyWave Definition { get; }

        public CurrentWave(int index, EnemyWave definition)
        {
            Index = index;
            Definition = definition;
        }

        public void AddEnemyCreate(Enemy enemy)
        {
            EnemiesCreated++;
            _enemies.Add(enemy);
        }
    }
}