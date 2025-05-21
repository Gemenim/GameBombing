using System;

public class UpgrateScreen : Window
{
    public event Action OnReturnButtonClicked;

    public override void Close()
    {
        WindowGroup.alpha = 0;
        WindowGroup.interactable = false;
        WindowGroup.blocksRaycasts = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1.0f;
        WindowGroup.interactable = true;
        WindowGroup.blocksRaycasts = true;
    }

    protected override void OnButtonClick()
    {
        OnReturnButtonClicked?.Invoke();
        Close();
    }
}
