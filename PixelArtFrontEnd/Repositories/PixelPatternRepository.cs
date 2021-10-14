using Dapper;
using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Repositories
{
    public class PixelPatternRepository: IPixelPatternRepository
    {
        private readonly string connectionString;
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public PixelPatternRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<AvailablePatterns>> GetAvailablePatternsAsync()
        {
            using (var sql = new SqlConnection(this.connectionString))
            {
                var patterns = await sql.QueryAsync<AvailablePatterns>("Get_Patterns", commandType: System.Data.CommandType.StoredProcedure);
                return patterns;
            }
        }

        public async Task<IEnumerable<PixelPatternDetails>> GetPatternDetailsAsync(Guid patternUUID)
        {
            using (var sql = new SqlConnection(this.connectionString))
            {
                var sqlParams = new DynamicParameters();
                sqlParams.Add("@PatternUUID", patternUUID);
                var patternDetailsRow = await sql.QueryAsync<PixelPatternDetailsRow>("Get_PatternDetailsByPatternUUID", sqlParams, commandType: System.Data.CommandType.StoredProcedure);
                var patternDetails = MapPatternDetailsRow(patternDetailsRow);
                return patternDetails;
            }
        }

        public async Task AddPatternDetails(PixelPatternDetailsRow patternDetails)
        {
            using (var sql = new SqlConnection(this.connectionString))
            {
                var sqlParams = new DynamicParameters();
                sqlParams.Add("@PatternUUID", patternDetails.PatternUUID);
                sqlParams.Add("@PatternName", patternDetails.PatternName);
                sqlParams.Add("@PatternSequenceNumber", patternDetails.PatternSequenceNumber);
                sqlParams.Add("@PatternDetails", patternDetails.PatternDetails);
                await sql.ExecuteAsync("Add_PatternDetails", sqlParams, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        private IEnumerable<PixelPatternDetails> MapPatternDetailsRow(IEnumerable<PixelPatternDetailsRow> patternDetailsRow)
        {
            return patternDetailsRow.Select(i => 
            { 
                return new PixelPatternDetails 
                    { 
                        PatternUUID = i.PatternUUID,
                        PatternName = i.PatternName,
                        PatternSequenceNumber = i.PatternSequenceNumber,
                        PatternDetails = JsonSerializer.Deserialize<SortedDictionary<int, string>>(i.PatternDetails, serializerOptions) 
                    }; 
            });
        }
    }
}
