using Engine;
using Engine.Window;

namespace Game;

internal class App : Application
{
    public App(WindowProps props) : base(props)
    {
    }

    static void Main(string[] args)
    {
        using App app = new App(new WindowProps());
        app.Run();
        app.Close();
    }
}