using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GamePlay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Enemies
{
    public class DifficultyManager
    {
        private float _nextCreationTime = 0f;
        private readonly Dictionary<DifficultyLevel, float> _enemyCreationRate = new Dictionary<DifficultyLevel, float>()
        {
            { DifficultyLevel.Easy, 2f },
            { DifficultyLevel.Normal, 1.3f },
            { DifficultyLevel.Hard, 0.75f }
        };
        //private readonly Dictionary<EnemyType, float> _enemyTypeRate = new Dictionary<EnemyType, float>()
        //{
        //    { EnemyType.Asteroid1, 2f },
        //    { EnemyType.SpaceShip1, 0.75f }
        //};

        public DifficultyLevel Difficulty { get; set; }

        public DifficultyManager(DifficultyLevel difficulty)
        {
            Difficulty = difficulty;
        }

        public bool CanCreateEnemy() => Time.time >= _nextCreationTime;

        public EnemyType SelectEnemyTypeForCreation()
        {
            var possibleEnemies = Enum.GetValues(typeof(EnemyType)).OfType<EnemyType>().ToArray();
            var rndIndex = Random.Range(0, possibleEnemies.Length);

            _nextCreationTime = Time.time + 1f * _enemyCreationRate[Difficulty];
            Debug.Log($"Selected enemy type: {possibleEnemies[rndIndex]}. Next creation time: {_nextCreationTime}");

            return possibleEnemies[rndIndex];
        }
    }
}