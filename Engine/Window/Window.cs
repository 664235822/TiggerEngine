using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Engine.Events;

namespace Engine.Window;

public class Window : IDisposable
{
    private readonly NativeWindow _window;
    internal bool Exists => !_window.IsExiting;
    internal Action<EventBase> EventCallback { get; set; } = null!;
    public bool Minimized => _window.WindowState == WindowState.Minimized;

    protected bool VSync
    {
        get => _window.VSync == VSyncMode.On;
        set => _window.VSync = value ? VSyncMode.On : VSyncMode.Off;
    }

    private Window(WindowProps props)
    {
        NativeWindowSettings windowSettings = new NativeWindowSettings()
        {
            Title = props.Title,
            Vsync = props.VSync ? VSyncMode.On : VSyncMode.Off,
            Size = new Vector2i(props.Width, props.Height),
            Location = new Vector2i(props.X, props.Y),
            WindowState = props.WindowInitialState
        };
        _window = new NativeWindow(windowSettings);
        if (props.X == 0 || props.Y == 0) _window.CenterWindow();

        _window.Resize += e => EventCallback(new WindowResizeEvent(e.Width, e.Height));
        _window.Closed += () => EventCallback(new WindowCloseEvent());
        _window.KeyUp += (keyEvent) => EventCallback(new KeyReleasedEvent((int)keyEvent.Key));
        _window.KeyDown += (keyEvent) => EventCallback(new KeyPressedEvent((int)keyEvent.Key, 1));
        _window.MouseDown += (mouseEvent) => EventCallback(new MouseButtonPressedEvent((int)mouseEvent.Button));
        _window.MouseUp += (mouseEvent) => EventCallback(new MouseButtonReleasedEvent((int)mouseEvent.Button));
        _window.MouseWheel += (mouseEvent) => EventCallback(new MouseScrolledEvent(mouseEvent.OffsetY));
        _window.MouseMove += (mouseEvent) =>
            EventCallback(new MouseMovedEvent(mouseEvent.Position.X, mouseEvent.Position.Y));
    }

    public void SwapBuffers() => _window.Context.SwapBuffers();
    public void MakeCurrent() => _window.MakeCurrent();
    public static Window CreateWindow(WindowProps props) => new(props);

    internal void OnUpdate()
    {
        NativeWindow.ProcessWindowEvents(false);
        _window.ProcessInputEvents();
    }

    public void Dispose()
    {
        _window.Dispose();
    }
}