
public class EliteMob : BaseEnemy
{   

    protected override void Update()
    {
        if (!CanUpdate())
            return;

        Chase();
        CheckDamaged();
    }

    private void FixedUpdate()
    {
        
    }
   
}
