using System.Collections.Generic;
using Assets.Scripts.GamePlay;
using PathCreation;
using UnityEngine;

public abstract class Enemy : GamePlayObject
{
    public int ScoreValue = 10;

    protected PathCreator Path;
    protected EnemyMode Mode = EnemyMode.Default;

    private readonly HashSet<string> _hittableTags = new HashSet<string> { ObjectTags.Player, ObjectTags.PlayerBullet };
    private float _distanceTraveled;

    protected override void Start()
    {
        base.Start();

        SetEnemyMode();
    }

    protected override void Update()
    {
        base.Update();

        if (Path != null)
        {
            _distanceTraveled += Speed * Time.deltaTime;
            transform.position = Path.path.GetPointAtDistance(_distanceTraveled, EndOfPathInstruction.Reverse);
        }
    }


    protected override bool IsHittableTag(string otherTag) => _hittableTags.Contains(otherTag);

    protected override void Die()
    {
        // TODO: Improve it to use a messaging systems
        Game.Current.EnemyKilled(this);
        base.Die();
    }


    public virtual void Spawn(DifficultyLevel difficulty, EnemyMode mode = EnemyMode.Default, GameObject path = null)
    {
        Debug.Log($"Spawn triggered on {name}");
        SetEnemyMode(mode, path);
    }

    private void SetEnemyMode(EnemyMode mode = EnemyMode.Default, GameObject path = null)
    {
        Mode = mode;

        switch (Mode)
        {
            case EnemyMode.Default:
                transform.rotation = new Quaternion(0, 0, 180, 0);
                GetComponent<Rigidbody2D>().velocity = transform.up * Speed;
                break;

            case EnemyMode.Path:
                Path = path.GetComponent<PathCreator>();
                break;
        }
    }
}