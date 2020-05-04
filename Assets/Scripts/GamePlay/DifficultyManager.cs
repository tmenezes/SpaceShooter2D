using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GamePlay
{
    public class DifficultyManager
    {
        private readonly List<EnemyType> _availableTypes;
        private float _lastCreationTime = 0f;
        private float _asteroidLastCreationTime = 0f;
        private readonly Dictionary<DifficultyLevel, float> _spaceshipsCreationRate = new Dictionary<DifficultyLevel, float>
        {
            { DifficultyLevel.Easy, 2f },
            { DifficultyLevel.Normal, 1f },
            { DifficultyLevel.Hard, 0.66f }
        };
        private readonly Dictionary<DifficultyLevel, float> _asteroidsCreationRate = new Dictionary<DifficultyLevel, float>
        {
            { DifficultyLevel.Easy, 7.5f },
            { DifficultyLevel.Normal, 5f },
            { DifficultyLevel.Hard, 3f }
        };

        public DifficultyLevel Difficulty { get; set; }

        public DifficultyManager(DifficultyLevel difficulty, List<EnemyType> availableTypes)
        {
            _availableTypes = availableTypes;
            Difficulty = difficulty;
        }

        public bool CanCreateEnemy(EnemyMode mode = EnemyMode.Default) => Time.time - _lastCreationTime >= GetCreationTimeRate(mode);

        public bool CanCreateAsteroid() => Time.time - _asteroidLastCreationTime >= _asteroidsCreationRate[Difficulty];

        public EnemyType SelectEnemyTypeForCreation()
        {
            _lastCreationTime = Time.time;

            var rndIndex = Random.Range(0, _availableTypes.Count);
            return _availableTypes[rndIndex];
        }

        public void NotifyEnemyTypeSelected(EnemyType type)
        {
            if (type == EnemyType.Asteroid1)
                _asteroidLastCreationTime = Time.time;
            else
                _lastCreationTime = Time.time;
        }

        private float GetCreationTimeRate(EnemyMode mode)
        {
            return mode == EnemyMode.Default
                ? _spaceshipsCreationRate[Difficulty]
                : _spaceshipsCreationRate[Difficulty] / 2;
        }
    }
}