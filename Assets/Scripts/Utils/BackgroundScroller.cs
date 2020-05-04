using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float Speed = 1;

    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _renderer.material.mainTextureOffset = new Vector2(0, Time.time * Speed);
    }
}
