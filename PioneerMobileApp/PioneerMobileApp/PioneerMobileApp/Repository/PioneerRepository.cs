using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using PioneerMobileApp.Models;
using Dapper;

namespace PioneerMobileApp.Repository
{
    public class PioneerRepository
    {
        private readonly IDbConnection _dbConnection;

        public PioneerRepository()
        {
            // Database connection string
            _dbConnection = new SqlConnection("uid=admin;pwd=Pioneer2022!;Persist Security Info=False;Initial Catalog=PioneerDatabase;Data Source=pioneerdatabase.cebyfmmwqqge.us-east-1.rds.amazonaws.com;");
        }

        public PioneerUser GetUser(string email, string password)
        {
            var paramDictionary = new Dictionary<string, object>
            {
                { "Email", email },
                { "Password", password }
            };

            // Query parameters
            var parameters = new DynamicParameters(paramDictionary);

            const string query = @"
SELECT top 1 [Id]
      ,[FirstName]
      ,[LastName]
      ,[Password]
      ,[Email]
      ,[UserTypeId]
  FROM [dbo].[PioneerUser] pu
WHERE pu.[Email] = @Email and pu.[Password] = @Password
";

            // Query execution
            var userEvents = Task.Run(() =>  _dbConnection.QuerySingleAsync<PioneerUser>(query, parameters)).Result;

            // Return data
            return userEvents;
        }

        public async Task<IEnumerable<UserEvent>> GetEventsByUserId(int userId)
        {

            var paramDictionary = new Dictionary<string, object>
            {
                { "UserId", userId }
            };

            // Query parameters
            var parameters = new DynamicParameters(paramDictionary);

            const string query = @"
SELECT pe.[Id]
      ,[UserId]
      ,[EventDate]
      ,[EventTitle]
      ,[EventDescription]
FROM [dbo].[PioneerEvent] pe
INNER JOIN [dbo].[PioneerUser] pu on pe.UserId = pu.Id
WHERE [EventDate] >= GETDATE() AND [UserId] = @UserId
";

            // Query execution
            var userEvents = await _dbConnection.QueryAsync<UserEvent>(query, parameters);

            // Return data
            return userEvents;

        }

    }


}
