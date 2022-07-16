namespace Fps.Controller
{
    public class EnemyBullet : Bullet
    {
        protected override string OpponentTagIdentifier => "Player";
    }
}