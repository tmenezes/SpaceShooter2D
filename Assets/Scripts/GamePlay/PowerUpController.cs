using Assets.Scripts.GamePlay;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private int _speed = 2;
    [SerializeField] private PowerUpType _type = PowerUpType.Shooting;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * _speed * -1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"PowerUp collided to {other.name}");

        if (other.tag != ObjectTags.Player)
            return;

        Game.Current.Player.ApplyPowerUp(_type);
        // TODO: Show effect
        Destroy(gameObject);
    }
}
