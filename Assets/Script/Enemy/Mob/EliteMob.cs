

public class EliteMob : BaseEnemy
{   

    protected override void Update()
    {
        
        Chase();
        CheckDamaged();
    }

    private void FixedUpdate()
    {
        
    }
}
