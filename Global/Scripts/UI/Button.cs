using Godot;

public partial class Button : BaseButton
{
    private bool _focused;

    [Export]
    private bool grabFocusOnReady;

    public override void _Ready()
    {
        _focused = false;
        ButtonDown += OnButtonDown;
        ButtonUp += OnButtonUp;
        FocusEntered += OnFocusEntered;
        FocusExited += OnFocusExited;
        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
        if (grabFocusOnReady) GrabFocus();
    }


    private void OnButtonDown()
    {
        Modulate = ControlUtils.ModulatePressed;
    }

    private void OnButtonUp()
    {
        Modulate = _focused
            ? ControlUtils.ModulateFocused
            : ControlUtils.ModulateNormal;
    }

    private void OnFocusEntered()
    {
        _focused = true;
        Modulate = ControlUtils.ModulateFocused;
    }

    private void OnFocusExited()
    {
        _focused = false;
        Modulate = ControlUtils.ModulateNormal;
    }

    private void OnMouseEntered()
    {
        Modulate = ControlUtils.ModulateHovered;
    }
    
    private void OnMouseExited()
    {
        Modulate = _focused
            ? ControlUtils.ModulateFocused
            : ControlUtils.ModulateNormal;
    }
}