using System.Collections.Generic;
using System.Threading.Tasks;

namespace Search.Cli.Services
{
    public interface ISearchService
    {
         Task<List<string>> SearchAsync(string column, string keyword, Operator op);
    }
}