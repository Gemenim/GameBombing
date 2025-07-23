using UnityEngine;
using YG;

public class RewardMoveBarricade : MonoBehaviour
{

    [SerializeField] private int AdID;
    [SerializeField] private BarrierMover _barrierMover;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Reward;
    }
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Reward;
    }

    private void Reward(int id)
    {
        if (id == AdID)
            _barrierMover.Move();
    }
}
