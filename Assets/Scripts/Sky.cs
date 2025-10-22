using UnityEngine;

public class Sky : MonoBehaviour
{
    public Transform Bombs;
    public Transform Cubes;
    public Transform Bullets;

    public void Clear()
    {
        ClearBombs();
    }

    public void ClearBombs()
    {
        int countObjects = Bombs.childCount;

        for (int i = 0; i < countObjects; i++)
            Destroy(Bombs.GetChild(i).gameObject);
    }

    public void ClearCubes()
    {
        int countObjects = Cubes.childCount;

        for (int i = 0; i < countObjects; i++)
            Destroy(Cubes.GetChild(i).gameObject);
    }

    public void ClearBullets()
    {
        int countObjects = Bullets.childCount;

        for (int i = 0; i < countObjects; i++)
            Bullets.GetChild(i).GetComponent<Bullet>().ReturnInPool();
    }
}
