using System;
using Assets.Scripts.GamePlay;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public class EnemyFactory : GameObjectFactory<EnemyType, Enemy>
    {
        // singleton
        private static EnemyFactory _instance = null;
        public static EnemyFactory Instance => _instance ?? (_instance = new EnemyFactory());
        private EnemyFactory() { }

        protected override EnemyType GetKey(GameObject template)
        {
            Enum.TryParse<EnemyType>(template.name, true, out var type);
            return type;
        }
    }
}
