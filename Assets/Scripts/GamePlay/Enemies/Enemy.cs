using System.Collections.Generic;
using Assets.Scripts.GamePlay;
using PathCreation;
using UnityEngine;

public abstract class Enemy : GamePlayObject
{
    public int ScoreValue = 10;

    protected EnemyModeOptions ModeOpts = EnemyModeOptions.Default;

    private readonly HashSet<string> _hittableTags = new HashSet<string> { ObjectTags.Player, ObjectTags.PlayerBullet };
    private float _distanceTraveled;

    // unity events
    protected override void Start()
    {
        base.Start();

        // set the default mode
        transform.rotation = new Quaternion(0, 0, 180, 0);
        GetComponent<Rigidbody2D>().velocity = transform.up * Speed;
    }

    protected override void Update()
    {
        base.Update();

        if (ModeOpts.IsValidPathMode)
        {
            _distanceTraveled += Speed * Time.deltaTime;
            transform.position = ModeOpts.Path.path.GetPointAtDistance(_distanceTraveled, ModeOpts.EndOfPathMode);
        }
    }

    // methods
    public virtual void Spawn(DifficultyLevel difficulty, EnemyModeOptions mode)
    {
        Debug.Log($"Spawn triggered on {name}");
        ModeOpts = mode;
    }

    protected override bool IsHittableTag(string otherTag) => _hittableTags.Contains(otherTag);

    protected override void Die()
    {
        // TODO: Improve it to use a messaging systems
        Game.Current.EnemyKilled(this);
        base.Die();
    }
}

public class EnemyModeOptions
{
    public EnemyMode Mode { get; private set; }
    public PathCreator Path { get; private set; }
    public EndOfPathInstruction EndOfPathMode { get; private set; }
    public bool IsValidPathMode => Mode == EnemyMode.Path && Path != null;

    public EnemyModeOptions(EnemyMode mode, GameObject path, EndOfPathInstruction endOfPathMode)
    {
        Mode = mode;
        Path = path?.GetComponent<PathCreator>();
        EndOfPathMode = endOfPathMode;
    }

    public static  EnemyModeOptions Default => new EnemyModeOptions(EnemyMode.Default, null, EndOfPathInstruction.Stop);
}