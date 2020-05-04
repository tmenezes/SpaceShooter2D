using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public abstract class GameObjectFactory<Tkey, TType>
    {
        private readonly Dictionary<Tkey, GameObject> _templates = new Dictionary<Tkey, GameObject>();

        // methods
        public void LoadTemplates(GameObject[] templates)
        {
            _templates.Clear();

            // try to load prefabs automatically
            if (templates == null || !templates.Any())
            {
                templates = Resources.FindObjectsOfTypeAll<GameObject>()
                                     .Where(g => g.GetComponent<TType>() != null)
                                     .ToArray();
            }

            Debug.Log($"found templates: {templates.Length}");
            foreach (var template in templates)
            {
                var key = GetKey(template);

                var isPredefinedEnemy = key != null;
                if (!isPredefinedEnemy)
                    continue;

                if (_templates.ContainsKey(key))
                    return;

                _templates.Add(key, template);
            }

            //// debugger 
            //var expectedTemplatesCount = Enum.GetValues(typeof(EnemyType)).Length;
            //if (_templates.Count != expectedTemplatesCount)
            //{
            //    Debug.LogWarning($"Enemies templates not loaded correctly; Expected {expectedTemplatesCount}, got {_templates.Count}");
            //}
        }

        protected abstract Tkey GetKey(GameObject template);

        public GameObject Create(Tkey type, Vector3 position)
        {
            if (!_templates.TryGetValue(type, out var template))
                return null;

            //Debug.Log($"creating obj {type}/{position}...");
            return GameObject.Instantiate(template, position, Quaternion.identity);
        }

        public IEnumerable<Tkey> AvailableTypes() => _templates.Keys;
    }
}