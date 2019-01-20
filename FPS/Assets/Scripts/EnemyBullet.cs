public class EnemyBullet : Bullet {
    protected override string OpponentTagIdentifier()
    {
        return "Player";
    }
}
