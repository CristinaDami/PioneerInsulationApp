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
            var userEvents = Task.Run(() =>  _dbConnection.QuerySingleOrDefaultAsync<PioneerUser>(query, parameters)).Result;

            // Return data
            return userEvents;
        }

        public IEnumerable<PioneerEvent> GetAllEvents()
        {
            const string query = @"
SELECT pe.[Id]
      ,[UserId]
      ,[EventDate]
      ,[EventTitle]
      ,[EventDescription]
      ,pu.[FirstName]
      ,pu.[LastName]
      ,pu.[Password]
      ,pu.[Email]
      ,pu.[UserTypeId]
FROM [dbo].[PioneerEvent] pe
INNER JOIN [dbo].[PioneerUser] pu on pe.UserId = pu.Id
WHERE [EventDate] >= GETDATE()
";

            // Query execution
            var userEvents = Task.Run(() => _dbConnection.QueryAsync<PioneerEvent>(query)).Result;

            // Return data
            return userEvents;

        }

        public IEnumerable<PioneerEvent> GetEventsByUserId(int userId)
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
      ,pu.[FirstName]
      ,pu.[LastName]
      ,pu.[Password]
      ,pu.[Email]
      ,pu.[UserTypeId]
FROM [dbo].[PioneerEvent] pe
INNER JOIN [dbo].[PioneerUser] pu on pe.UserId = pu.Id
WHERE [EventDate] >= GETDATE() AND [UserId] = @UserId
";

            // Query execution
            var userEvents = Task.Run(() => _dbConnection.QueryAsync<PioneerEvent>(query, parameters)).Result;

            // Return data
            return userEvents;

        }

    }


}
