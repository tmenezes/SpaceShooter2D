using System;
using Assets.Scripts.GamePlay;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public class PowerUpFactory : GameObjectFactory<PowerUpType, PowerUpController>
    {
        // singleton
        private static PowerUpFactory _instance = null;
        public static PowerUpFactory Instance => _instance ?? (_instance = new PowerUpFactory());
        private PowerUpFactory() { }

        protected override PowerUpType GetKey(GameObject template)
        {
            Enum.TryParse<PowerUpType>(template.name, true, out var type);
            return type;
        }
    }
}