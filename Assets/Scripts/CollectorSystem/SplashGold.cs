using UnityEngine;

public class SplashGold : MonoBehaviour
{
    [SerializeField] private ParticleSystem _splash;
    
    private Transform _splashTransform;

    private void Awake()
    {
        _splashTransform = _splash.transform;
    }

    public void Splash(float positionX)
    {
        _splashTransform.position = new Vector3(positionX, _splashTransform.position.y, 0);
        _splash.Play();
    }
}
