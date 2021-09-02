using HeidelbergCement.Data.DTO;

namespace HeidelbergCement.Service.Interface
{
    public interface IAirTableDataProvider
    {
        ResponseRecord GetRecords(string url);
        ResponseRecord PostRecord(string url, RequestRecord data);
    }
}
