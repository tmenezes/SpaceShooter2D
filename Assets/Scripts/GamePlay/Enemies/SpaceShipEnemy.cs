using Assets.Scripts.GamePlay;
using PathCreation;
using UnityEngine;

public class SpaceShipEnemy : Enemy
{
    private float _nextShootTime;
    private GameObject _bulletPoint;
    private AudioSource _bulletAudio;

    public bool CanShoot = true;
    public float ShootRate = 1;
    public GameObject BulletTemplate;

    public SpaceShipEnemy()
    {
        Health = 2;
        ScoreValue = 20;
    }


    protected override void Start()
    {
        base.Start();
        _bulletPoint = gameObject.FindComponentInChildWithTag<Component>(ObjectTags.BulletPoints).gameObject;
        _bulletAudio = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        base.Update();

        var isTimeToShoot = Time.time >= _nextShootTime;
        if (isTimeToShoot && CanShoot)
            Shoot();
    }


    public void Shoot()
    {
        var bullet = Instantiate(BulletTemplate, _bulletPoint.transform.position, _bulletPoint.transform.rotation);
        bullet.GetComponent<BulletController>().SetAsEnemyBullet();
        _bulletAudio.Play();

        _nextShootTime = Time.time + (1f / ShootRate) + Random.Range(0f, 0.5f); // add some randomness time while shooting
    }

    public override void Spawn(DifficultyLevel difficulty, EnemyModeOptions mode)
    {
        base.Spawn(difficulty, mode);
        //transform.localPosition =  transform.EnsurePositionInScreenBoundaries(transform.position);

        ShootRate = difficulty == DifficultyLevel.Easy ? 0.5f :
                    difficulty == DifficultyLevel.Normal ? ShootRate : 3;

        if (mode.IsValidPathMode)
        {
            Speed *= 2;
        }
    }
}