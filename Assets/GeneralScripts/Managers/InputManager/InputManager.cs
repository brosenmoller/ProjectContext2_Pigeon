
public class InputManager : Manager
{
    public Controls controls;
    
    public override void Setup()
    {
        controls = new Controls();
        controls.Enable();
    }

    public void Disable()
    {
        controls.Disable();
    }
}

