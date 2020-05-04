using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Factories;
using PathCreation;
using PathCreation.Utility;
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
        }

        // public
        public void ExecuteCurrentWave()
        {
            if (Ended) return;
            if (_currentWave.IsFullyCreated) return;
            if (_currentWave.Delaying) return;

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
        private void SetMovementMode(Enemy enemy)
        {
            enemy.Spawn(_difficultyManager.Difficulty, CurrentWave.Definition.ToEnemyMode());
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
        private readonly float _creationTime = Time.time;

        public int Index { get; }
        public int EnemiesCreated { get; private set; }
        public bool IsFullyCreated => EnemiesCreated >= Definition.EnemyCount;
        public bool Ended => IsFullyCreated && _enemies.All(e => e?.Destroyed ?? true);
        public bool Delaying => _creationTime + Definition.Delay > Time.time;
        public EnemyWave Definition { get; }

        public CurrentWave(int index, EnemyWave definition)
        {
            Index = index;
            Definition = definition;

            if (definition.IsInverted)
                InvertPath();
        }

        private void InvertPath()
        {
            // TODO: Improve to dont create many multiple inverted paths unecessarily

            var invertedPath = GameObject.Instantiate(Definition.Path);
            var creator = invertedPath.GetComponent<PathCreator>();

            var totalPoints = creator.bezierPath.NumPoints;
            var points = totalPoints / 2;
            for (int i = 0; i < points; i++)
            {
                var i2 = totalPoints - i - 1;
                var pt1 = creator.bezierPath[i];
                var pt2 = creator.bezierPath[i2];

                creator.bezierPath.SetPoint(i, pt2, true);
                creator.bezierPath.SetPoint(i2, pt1, true);
            }
            creator.bezierPath.NotifyPathModified();

            Definition.Path = invertedPath;
        }

        public void AddEnemyCreate(Enemy enemy)
        {
            EnemiesCreated++;
            _enemies.Add(enemy);
        }
    }
}