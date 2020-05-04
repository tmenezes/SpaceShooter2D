namespace Assets.Scripts.GamePlay
{
    public static class ObjectTags
    {
        public static readonly string Player       = "Player";
        public static readonly string PlayerBullet = "PlayerBullet";
        public static readonly string Enemy        = "Enemy";
        public static readonly string EnemyBullet  = "EnemyBullet";
        public static readonly string BulletPoints = "BulletPoints";
        public static readonly string Bullet       = "Bullet";
        public static readonly string PowerUp       = "PowerUp";
    }

    public  enum EnemyType
    {
        Asteroid1,
        SpaceShip1,
        SpaceShip2,
        SpaceShip3,
    }

    public enum EnemyMode
    {
        Default,
        Path
    }

    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard
    }

    public enum Layers
    {
        Player = 8,
        Enemy = 9,
        Invulnerable = 10,
        //PlayerLaser = 11,
        //EnemyLaser = 12,
    }

    public enum PowerUpType
    {
        Shooting,
        Shield,
        Health
    }
}