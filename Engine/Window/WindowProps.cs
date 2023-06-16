using OpenTK.Windowing.Common;

namespace Engine.Window;

public class WindowProps
{
    public int Width { get; }
    public int Height { get; }
    public string Title { get; }
    public int X { get; }
    public int Y { get; }
    public bool VSync { get; }
    public WindowState WindowInitialState { get; }

    public WindowProps(string title = "Tigger Engine", int width = 1280, int height = 760,
        int x = 0, int y = 0, bool vSync = false, WindowState windowState = WindowState.Normal)
    {
        Height = height;
        Width = width;
        Title = title;
        X = x;
        Y = y;
        VSync = vSync;
        WindowInitialState = windowState;
    }
}