using UnityEngine;

public class PopupHint : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private MessageBox _messageBox;

    public void Expand()
    {
        if (!_messageBox.IsOn)
            _messageBox.Switch(_id);
    }

    public void Collapse()
    {
        if (_messageBox.IsOn)
            _messageBox.Switch(_id);
    }
}
