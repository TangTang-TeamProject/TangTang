

public class EliteMob : BaseEnemy
{   

    void Update()
    {        
        CheckDamaged();
    }

    private void FixedUpdate()
    {
        Chase();
    }
}
