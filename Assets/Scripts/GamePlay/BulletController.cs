using Assets.Scripts.GamePlay;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Speed = 5f;

    private Layers _ownerLayer;

    void Update()
    {
        transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));
    }

    public void SetAsEnemyBullet(float speed = 5)
    {
        _ownerLayer = Layers.Enemy;
        tag = ObjectTags.EnemyBullet;
        Speed = speed;
    }

    public void SetAsPlayerBullet(float speed = 10)
    {
        _ownerLayer = Layers.Player;
        tag = ObjectTags.PlayerBullet;
        Speed = speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var sameTag = CompareTag(other.tag);
        var fromOwner = other.tag == (_ownerLayer == Layers.Player ? ObjectTags.Player : ObjectTags.Enemy);
        if (sameTag || fromOwner)
            return;

        // TODO: trigger some sort bullet hit effect/animation/particle
        Destroy(gameObject);
    }
}
