using Assets.Scripts.GamePlay;
using UnityEngine;

public class BoundaryObjectDetroyer : MonoBehaviour
{
    public bool IsTopBoundary = true;
    public bool IsBottomBoundary = false;

    void Start()
    {
        var factor = IsTopBoundary ? -1 : 1;
        transform.position = new Vector3(0, (Camera.main.orthographicSize + 3) * factor, 0);
        transform.localScale = new Vector3(ScreenHelper.GetOrthographicXRate() * 2 + 1, 1);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ObjectTags.Enemy)
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.Destroyed = true;
        }

        Destroy(other.gameObject);
    }
}
