using System.Collections;
using UnityEngine;

public class RanomazeBombs : MonoBehaviour
{
    private Chip[] _chips;
    private Vector3[] _positions;
    private System.Random _random = new System.Random();

    public void Randomaze()
    {
        StartCoroutine(Exchange());
    }

    private IEnumerator Exchange()
    {
        yield return null;

        _chips = GetComponentsInChildren<Chip>();

        if (_chips.Length > 1)
        {
            _positions = new Vector3[_chips.Length];

            for (int i = 0; i < _chips.Length; i++)
                _positions[i] = _chips[i].transform.position;

            for (int i = _positions.Length - 1; i >= 1; i--)
            {
                int j = _random.Next(i + 1);
                Vector3 temp = _positions[j];
                _positions[j] = _positions[i];
                _positions[i] = temp;
            }

            for (int i = 0; i < _chips.Length; i++)
                _chips[i].transform.position = _positions[i];
        }
    }
}
