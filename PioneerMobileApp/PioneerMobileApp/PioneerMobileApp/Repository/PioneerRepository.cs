using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using PioneerMobileApp.Models;
using Dapper;

namespace PioneerMobileApp.Repository
{
    /// <summary>
    /// Author: Cristina Damian - Roehampton University - Faculty of Computing
    /// 
    /// <para>Class gateway as data access layer to MS SQL Server</para>
    /// </summary>
    public class PioneerRepository
    {
        private readonly IDbConnection _dbConnection; // Interface that represents an open connection to a database

        public PioneerRepository()
        {
            // Database connection string
            _dbConnection = 
                new SqlConnection("uid=admin;pwd=Pioneer2022!;Persist Security Info=False;Initial Catalog=PioneerDatabase;Data Source=pioneerdatabase.cebyfmmwqqge.us-east-1.rds.amazonaws.com;");
        }

        /// <summary>
        /// Retrieve a Pioneer User filter by Email and Password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>A reference of PioneerUser</returns>
        public PioneerUser GetUser(string email, string password)
        {
            var paramDictionary = new Dictionary<string, object> // Build parameters to pass to the query
            {
                { "Email", email },
                { "Password", password }
            };

            // Query parameters
            var parameters = new DynamicParameters(paramDictionary);

            // Query to submit 
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

            // Query execution: QuerySingleOrDefaultAsync returns a single row or null if not existing
            var userEvents = Task.Run(() =>  _dbConnection.QuerySingleOrDefaultAsync<PioneerUser>(query, parameters)).Result;

            // Return data
            return userEvents;
        }

        /// <summary>
        /// Retrieve all Pioneer Events
        /// </summary>
        /// <returns>Collection of PioneerEvent</returns>
        public IEnumerable<PioneerEvent> GetAllEvents()
        {
            // Query to submit 
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

            // Query execution: QueryAsync returns a collection of PioneerEvent
            var userEvents = Task.Run(() => _dbConnection.QueryAsync<PioneerEvent>(query)).Result;

            // Return data
            return userEvents;

        }

        /// <summary>
        /// Retrieve a Pioneer Event filter by User ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Collection of PioneerEvent</returns>
        public IEnumerable<PioneerEvent> GetEventsByUserId(int userId)
        {

            var paramDictionary = new Dictionary<string, object> // Build parameters to pass to the query
            {
                { "UserId", userId }
            };

            // Query parameters
            var parameters = new DynamicParameters(paramDictionary);

            // Query to submit
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

            // Query execution: QueryAsync returns a collection of PioneerEvent
            var userEvents = Task.Run(() => _dbConnection.QueryAsync<PioneerEvent>(query, parameters)).Result;

            // Return data
            return userEvents;

        }

    }


}
