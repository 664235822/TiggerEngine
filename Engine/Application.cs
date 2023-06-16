using System.Globalization;
using Engine.Events;
using Engine.Logging;
using Engine.Window;

namespace Engine;

public abstract class Application : IDisposable
{
    #region Properties

    public static Application App { get; private set; }
    public Window.Window Window { get; }
    private bool _shouldDispose;

    #endregion

    protected Application(WindowProps props)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        Logger.Init();
        App = this;
        Window = Engine.Window.Window.CreateWindow(props);
        Window.EventCallback = OnEvent;
    }


    #region Public Methods

    public void Run()
    {
        Window.MakeCurrent();
        while (Window.Exists)
        {
            if (!Window.Minimized)
            {
                Window.OnUpdate();
            }

            if (Window.Exists)
            {
                Window.SwapBuffers();
            }

            if (_shouldDispose)
                Dispose();
        }
    }

    public void OnEvent(EventBase @event)
    {
        Logger.CoreInfo(@event);
    }

    public void Close()
    {
        _shouldDispose = true;
    }

    #endregion


    public void Dispose()
    {
        Window.Dispose();
    }
}