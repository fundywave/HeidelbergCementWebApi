using HeidelbergCement.Data.DTO;
using HeidelbergCement.Service.Interface;

namespace HeidelbergCement.Service.Provider
{
    public class AirTableDataProvider : IAirTableDataProvider
    {
        readonly IDataProvider<ResponseRecord> _dataProvider;
        public AirTableDataProvider(IDataProvider<ResponseRecord> dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public ResponseRecord GetRecords(string url)
        {
            var records=_dataProvider.Get(url);
            return records;
        }

        public ResponseRecord PostRecord(string url, RequestRecord data)
        {
            var records = _dataProvider.Post(url, data);
            return records;
        }
    }
}
