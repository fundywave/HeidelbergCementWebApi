using System.Collections.Generic;

namespace HeidelbergCement.Data.DTO
{
    public class ResponseRecord
    {
        public List<RecordDTO> records { get; set; }
        public string offset { get; set; }
    }
}
