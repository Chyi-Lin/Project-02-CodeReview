using MoreMountains.Tools;

public class LocalLoadSave : ILoadSave
{
    private string fileName;

    public LocalLoadSave(string fileName)
    {
        this.fileName = fileName;
    }

    /// <summary>
    /// 載入檔案
    /// </summary>
    public T Load<T>()
    {
        T loadData = (T)MMSaveLoadManager.Load(typeof(T), fileName);
        return loadData;
    }

    /// <summary>
    /// 儲存檔案
    /// </summary>
    public void Save<T>(T t)
    {
        MMSaveLoadManager.Save(t, fileName);
    }

}
