public static class GlobalGameState
{
    public const float Tolerance = 0.001F;
    public static bool Initialized = false;

    private static bool _freePlay = false;
    private static bool _hideUi;

    public delegate void HideUiChange(bool hideUi);

    public static bool HideUi
    {
        get => _hideUi;
        set
        {
            _hideUi = value;
            OnHideUiChange?.Invoke(value);
        }
    }

    public static HideUiChange OnHideUiChange;

    public static bool FreePlay
    {
        get => _freePlay;
        set
        {
            if (!Initialized || _freePlay != value)
            {
                Initialized = true;
                _freePlay = value;
                HideUi = value;
            }
        }
    }
}
