namespace HeidelbergCement.Service.Interface
{
    public interface IDataProvider<T>
    {
        T Get(string url);
        T Post<A>(string url,A data);
    }
}
