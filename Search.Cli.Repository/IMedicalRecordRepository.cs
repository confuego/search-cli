using System.Data;
using System.Threading.Tasks;

namespace Search.Cli.Repository
{
    public interface IMedicalRecordRepository
    {
         Task<DataTable> ReadAsync(string filePath);
    }
}