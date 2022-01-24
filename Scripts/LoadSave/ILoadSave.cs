public interface ILoadSave
{
    T Load<T>();
    void Save<T>(T t);
}
