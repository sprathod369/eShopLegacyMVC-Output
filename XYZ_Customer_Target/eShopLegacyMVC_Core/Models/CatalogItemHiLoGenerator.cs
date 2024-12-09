using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging; 
using System; 
using System.Linq; 
using System.Threading.Tasks; 
namespace eShopLegacyMVC_Core.Models 
{ 
    public class CatalogItemHiLoGenerator 
    { 
        private const int HiLoIncrement = 10; 
        private int sequenceId = -1; 
        private int remainingLoIds = 0; 
        private readonly object sequenceLock = new object(); 
        private readonly ILogger<CatalogItemHiLoGenerator> _logger; 
        public CatalogItemHiLoGenerator(ILogger<CatalogItemHiLoGenerator> logger) 
        { 
            _logger = logger; 
        } 
        public async Task<int> GetNextSequenceValueAsync(CatalogDBContext db) 
        { 
            lock (sequenceLock) 
            { 
                try 
                { 
                    if (remainingLoIds == 0) 
                    { 
                        _logger.LogInformation("Fetching next HiLo sequence value from database asynchronously."); 
                        var rawQuery = await db.Database.ExecuteSqlRawAsync("SELECT NEXT VALUE FOR catalog_hilo;"); 
                        sequenceId = Convert.ToInt32(rawQuery); 
                        remainingLoIds = HiLoIncrement - 1; 
                        _logger.LogInformation("New sequence ID obtained: {SequenceId}", sequenceId); 
                        return sequenceId; 
                    } 
                    else 
                    { 
                        remainingLoIds--; 
                        _logger.LogInformation("Using cached sequence ID: {SequenceId}", sequenceId + 1); 
                        return ++sequenceId; 
                    } 
                } 
                catch (Exception ex) 
                { 
                    _logger.LogError(ex, "Error occurred while generating next sequence value asynchronously."); 
                    throw; 
                } 
            } 
        } 
    } 
}