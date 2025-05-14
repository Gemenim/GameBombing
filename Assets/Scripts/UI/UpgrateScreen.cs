using System;

public class UpgrateScreen : Window
{
    public event Action OnReturnButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

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
