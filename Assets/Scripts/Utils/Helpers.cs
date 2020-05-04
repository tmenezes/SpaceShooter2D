using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public static class TransformExtensions
    {
        public static Vector3? MoveFromAxis(this Transform transform, float speed)
        {
            return null;
        }

        public static Vector2 EnsurePositionInScreenBoundaries(this Transform transform, Vector2 pos)
        {
            var cameraBoundaryX = ScreenHelper.GetOrthographicXRate() - transform.localScale.x / 2;
            var cameraBoundaryY = Camera.main.orthographicSize - transform.localScale.y / 2;

            pos.x = Mathf.Clamp(pos.x, cameraBoundaryX * -1, cameraBoundaryX);
            pos.y = Mathf.Clamp(pos.y, cameraBoundaryY * -1, cameraBoundaryY);
            return pos;
        }
    }

    public class ScreenHelper
    {
        public static float GetOrthographicXRate()
        {
            return Mathf.Abs(Screen.width / (float)Screen.height) * Camera.main.orthographicSize;
        }

        public static float GetRandomBoundaryX()
        {
            return Random.Range(GetOrthographicXRate() * -1, GetOrthographicXRate());
        }

        public static Vector3 GetRandomScreenPoint(float? x = null, float? y = null)
        {
            x = x ?? GetRandomBoundaryX();
            y = y ?? Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);

            return new Vector3((float)x, (float)y, 0);
        }
    }

    public static class GameObjectExtensions
    {
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag) where T : Component
        {
            var t = parent.transform;
            foreach (Transform tr in t)
            {
                if (tr.tag == tag)
                    return tr.GetComponent<T>();
            }
            return null;
        }

        public static GameObject[] FindComponentsInChildWithTag(this GameObject parent, string tag)
        {
            return parent.GetComponentsInChildren<Component>()
                         .Where(c => c.tag == ObjectTags.BulletPoints)
                         .Select(c => c.gameObject)
                         .ToArray();
        }
    }
}