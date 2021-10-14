using Dapper;
using PixelArtFrontEnd.Models;
using PixelArtFrontEnd.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PixelArtFrontEnd.Repositories
{
    public class PixelPatternRepository: IPixelPatternRepository
    {
        private readonly string connectionString;

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
                var patternDetails = await sql.QueryAsync<PixelPatternDetails>("Get_PatternDetailsByPatternUUID", sqlParams, commandType: System.Data.CommandType.StoredProcedure);
                return patternDetails;
            }
        }

        public async Task AddPatternDetails(PixelPatternDetails patternDetails)
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
    }
}
