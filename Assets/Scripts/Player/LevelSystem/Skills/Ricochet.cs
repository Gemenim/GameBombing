public class Ricochet : Ability
{
    private const int c_defoltRicochet = 5;

    public int Count { get; private set; }

    protected override void UpdateStatus()
    {
        Count = c_defoltRicochet + Level;
    }
}