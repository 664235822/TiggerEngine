namespace Engine.Events;

public abstract class EventBase
{
    #region DEBUG only

#if DEBUG
    public virtual string GetName()
    {
        return GetType().Name;
    }
#endif

    #endregion

    #region Properties

    public virtual EventType EventType => 0;

    public abstract EventCategory CategoryFlags { get; }

    // 当前事件是否处理完成，否的话继续执行
    public bool Handled { get; set; }

    #endregion

    #region Methods

    public bool IsInCategory(EventCategory category)
    {
        return (CategoryFlags & category) != 0;
    }

    #endregion
}