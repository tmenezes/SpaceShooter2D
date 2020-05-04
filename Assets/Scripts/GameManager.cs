using System;
using System.Linq;
using Assets.Scripts.Factories;
using Assets.Scripts.GamePlay;
using PathCreation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class EnemyWave
{
    public EnemyMode Mode = EnemyMode.Default;
    public int EnemyCount = 3;
    public float Delay = 2;
    public GameObject Path;
    public EnemyType EnemyType = EnemyType.SpaceShip1;
    public bool IsInverted;
    public EndOfPathInstruction EndOfPathMode = EndOfPathInstruction.Reverse;
    public bool HasPowerUp;
    public PowerUpType PowerUp = PowerUpType.Shooting;

    public EnemyModeOptions ToEnemyMode() => new EnemyModeOptions(Mode, Path, EndOfPathMode);
}

public class GameManager : MonoBehaviour
{
    public DifficultyLevel Difficulty = DifficultyLevel.Normal;
    //public bool IncreaseDifficultyOverTime = true;
    public GameObject EnemySpawnPoint;
    public GameObject[] EnemiesTemplates;
    public GameObject[] PowerUpsTemplates;
    public EnemyWave[] EnemyWaves;
    public Text ScoreText;
    public Text ShieldText;
    public Text HealthText;

    private EnemyFactory _enemyFactory;
    private PowerUpFactory _powerUpFactory;
    private DifficultyManager _difficultyManager;
    private WaveManager _waveManager;

    void Awake()
    {
        _enemyFactory = EnemyFactory.Instance;
        _enemyFactory.LoadTemplates(EnemiesTemplates);

        _powerUpFactory = PowerUpFactory.Instance;
        _powerUpFactory.LoadTemplates(PowerUpsTemplates);

        _difficultyManager = new DifficultyManager(Difficulty, _enemyFactory.AvailableTypes().ToList());
        _waveManager = new WaveManager(EnemyWaves, _difficultyManager, EnemySpawnPoint);

        Effetcs.Load();
        Game.Current.StartNew();
    }

    void Update()
    {
        UpdateUI();

        if (Input.GetKey("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        _waveManager.ExecuteCurrentWave();
        if (_waveManager.CurrentWave.Ended)
        {
            // handle wave powerUp if present
            if (_waveManager.CurrentWave.Definition.HasPowerUp)
            {
                var pos = ScreenHelper.GetRandomScreenPoint(y: EnemySpawnPoint.transform.position.y);
                var powerUpType = _waveManager.CurrentWave.Definition.PowerUp;
                _powerUpFactory.Create(powerUpType, pos);
            }

            _waveManager.MoveNext();
        }

        if (_difficultyManager.CanCreateAsteroid())
        {
            _difficultyManager.NotifyEnemyTypeSelected(EnemyType.Asteroid1);
            var pos = ScreenHelper.GetRandomScreenPoint(y: EnemySpawnPoint.transform.position.y);
            _enemyFactory.Create(EnemyType.Asteroid1, pos);
        }
    }

    private void UpdateUI()
    {
        ScoreText.text = Game.Current.Score.ToString();
        ShieldText.text = Game.Current.Player.ShieldPower.ToString();
        HealthText.text = Game.Current.Player.Health.ToString();

    }
}
