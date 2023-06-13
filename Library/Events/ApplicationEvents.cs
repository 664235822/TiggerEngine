namespace Library.Events;

public class WindowResizeEvent : Library.Events.EventBase
{
    #region Properties

    public int Width { get; }
    public int Height { get; }

    #endregion

    #region Constructor

    public WindowResizeEvent(int width, int height)
    {
        Width = width;
        Height = height;
    }

    #endregion

    #region EventBase

    public override EventCategory CategoryFlags => EventCategory.Application;

    public override EventType EventType => EventType.WindowResize;

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"WindowResizeEvent: ({Width}, {Height})";
    }

    #endregion
}

public class WindowCloseEvent : Library.Events.EventBase
{
    #region EventBase

    public override EventCategory CategoryFlags => EventCategory.Application;

    public override EventType EventType => EventType.WindowClose;

    #endregion
}

public class AppTickEvent : Library.Events.EventBase
{
    #region EventBase

    public override EventCategory CategoryFlags => EventCategory.Application;

    public override EventType EventType => EventType.AppTick;

    #endregion
}

public class AppUpdateEvent : Library.Events.EventBase
{
    #region EventBase

    public override EventCategory CategoryFlags => EventCategory.Application;

    public override EventType EventType => EventType.AppUpdate;

    #endregion
}

public class AppRenderEvent : Library.Events.EventBase
{
    #region EventBase

    public override EventCategory CategoryFlags => EventCategory.Application;

    public override EventType EventType => EventType.AppRender;

    #endregion
}