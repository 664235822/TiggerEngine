namespace Engine.AssetManager;

public interface IAsset : IDisposable
{
    public string FilePath { get; internal set; }
    public string Name { get; }
    public AssetType AssetType { get; }
}