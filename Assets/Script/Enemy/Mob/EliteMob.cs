

public class EliteMob : BaseEnemy
{   

    protected override void Update()
    {
        base.Update();
        Chase();
        CheckDamaged();
    }

    private void FixedUpdate()
    {
        
    }
}
