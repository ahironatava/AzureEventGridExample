using FacadeApi.Models;

namespace FacadeApi.Interfaces
{
    public interface IFacadeService
    {
        (string errMsg, int recordId) ProcessRequest(string value);

        (int statusCode, string statusMessage, StoredRecord record) GetRecord(int id);
    }
}
