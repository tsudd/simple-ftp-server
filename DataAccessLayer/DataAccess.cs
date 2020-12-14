using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ConfigProvider;
using DataAccessLayer.Models;
using Model;

namespace DataAccessLayer
{
    public class DataAccess
    {
        const string GET_PERSON_PROC = "GetPerson";
        const string GET_PEOPLE_PROC = "GetPeople";
        const string GET_PEOPLE_RANGE_PROC = "GetPeopleRange";
        const string GET_PEOPLE_JOIN = "GetPeopleJoin";
        const string GET_PASSWORD_PROC = "GetPassword";
        const string GET_EMAIL_PROC = "GetEmail";
        const string GET_PHONE_PROC = "GetPhone";
        const string GET_ADDRESS_PROC = "GetAddress";
        const string GET_MAX_ID = "PersonMaxId";
        private readonly SqlConnection connection;

        private readonly ConfigProvader config;

        public DataAccess(Model.DataManagerOptions.ConnectionOptions options)
        {
            config = new ConfigProvader();

            string connectionString = $"Data Source={options.DataSource};" +
                $" Initial catalog={options.InitialCatalog};" +
                $" User={options.User};" +
                $" Password={options.Password};" +
                $" Integrated Security={options.IntegratedSecurity}";
            using (var scope = new TransactionScope())
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                scope.Complete();
            }
        }

        public Person GetPersonById(int id)
        {
            var command = new SqlCommand(GET_PERSON_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("idArg", id));
            using (var scope = new TransactionScope())
            {
                var ans = Map<Person>(command.ExecuteReader(), config);
                scope.Complete();
                if (ans.Count == 0)
                {
                    return new Person();
                }
                else
                {
                    return ans.First();
                }
            }
        }

        public List<Person> GetPeople()
        {
            List<Person> ans = null;
            var command = new SqlCommand(GET_PEOPLE_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using (var scope = new TransactionScope())
            {
                ans = Map<Person>(command.ExecuteReader(), config);
                scope.Complete();
            }
            return ans;
        }

        public List<Person> GetPeopleRange(int startIndex, int count)
        {
            List<Person> ans = null;
            SqlCommand command = new SqlCommand(GET_PEOPLE_RANGE_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("startInd", startIndex));
            command.Parameters.Add(new SqlParameter("endInd", count));

            using (var scope = new TransactionScope())
            {
                scope.Complete();
                ans = Map<Person>(command.ExecuteReader(), config);
            }
            return ans;
        }

        public Password GetPasswordById(int id)
        {
            Password ans = null;
            var command = new SqlCommand(GET_PASSWORD_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("idArg", id));
            using (var scope = new TransactionScope())
            {
                var list = Map<Password>(command.ExecuteReader(), config);
                scope.Complete();
                if (list.Count == 0)
                {
                    ans = new Password();
                }
                else
                {
                    ans = list.First();
                }
            }
            return ans;
        }

        public Email GetEmailById(int id)
        {
            Email ans = null;
            var command = new SqlCommand(GET_EMAIL_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("intArg", id));
            using (var scope = new TransactionScope())
            {
                var list = Map<Email>(command.ExecuteReader(), config);
                scope.Complete();
                if (list.Count == 0)
                {
                    ans = new Email();
                }
                else
                {
                    ans = list.First();
                }
            }
            return ans;
        }

        public PersonPhone GetPhoneById(int id)
        {
            PersonPhone ans = null;
            var command = new SqlCommand(GET_PHONE_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("idArg", id));
            using (var scope = new TransactionScope())
            {
                var list = Map<PersonPhone>(command.ExecuteReader(), config);
                scope.Complete();
                if (list.Count == 0)
                {
                    ans = new PersonPhone();
                }
                else
                {
                    ans = list.First();
                }
            }
            return ans;
        }

        public Address GetAddressById(int id)
        {
            Address ans = null;
            var command = new SqlCommand(GET_ADDRESS_PROC, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("idArg", id));
            using (var scope = new TransactionScope())
            {
                var list = Map<Address>(command.ExecuteReader(), config);
                scope.Complete();
                if (list.Count == 0)
                {
                    ans = new Address();
                }
                else
                {
                    ans = list.First();
                }
            }
            return ans;
        }

        public List<PersonGeneral> GetPeopleWithJoin()
        {
            var ans = new List<PersonGeneral>();
            var command = new SqlCommand(GET_PEOPLE_JOIN, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandTimeout = 228;
            using (var scope = new TransactionScope())
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var personGen = new PersonGeneral();

                    var dict = new Dictionary<string, object>();
                    for (int i = 0; i < 13; i++)
                    {
                        string name = reader.GetName(i);
                        object val = reader.GetValue(i);
                        dict.Add(name, val);
                    }
                    personGen.Person = config.Map<Person>(dict);

                    dict = new Dictionary<string, object>();
                    for (int i = 13; i < 18; i++)
                    {
                        string name = reader.GetName(i);
                        object val = reader.GetValue(i);
                        dict.Add(name, val);
                    }
                    personGen.Password = config.Map<Password>(dict);

                    dict = new Dictionary<string, object>();
                    for (int i = 18; i < 23; i++)
                    {
                        string name = reader.GetName(i);
                        object val = reader.GetValue(i);
                        dict.Add(name, val);
                    }
                    personGen.Email = config.Map<Email>(dict);

                    dict = new Dictionary<string, object>();
                    for (int i = 23; i < 27; i++)
                    {
                        string name = reader.GetName(i);
                        object val = reader.GetValue(i);
                        dict.Add(name, val);
                    }
                    personGen.PersonPhone = config.Map<PersonPhone>(dict);

                    dict = new Dictionary<string, object>();
                    for (int i = 27; i < 36; i++)
                    {
                        string name = reader.GetName(i);
                        object val = null;
                        try
                        {
                            val = reader.GetValue(i);
                        }
                        catch
                        {
                            continue;
                        }
                        dict.Add(name, val);
                    }
                    personGen.Address = config.Map<Address>(dict);

                    ans.Add(personGen);
                }
                reader.Close();
            }
            return ans;
        }

        public int GetMaxId()
        {
            int ans = 0;
            var command = new SqlCommand(GET_MAX_ID, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using (var scope = new TransactionScope())
            {
                var reader = command.ExecuteReader();
                reader.Read();
                ans = reader.GetInt32(0);
                reader.Close();
                scope.Complete();
            }
            return ans;
        }

        private List<T> Map<T>(SqlDataReader reader, ConfigProvader config)
        {
            var parsed = Parse(reader);
            var ans = new List<T>();
            foreach (var dict in parsed)
            {
                ans.Add(config.Map<T>(dict));
            }
            return ans;
        }

        private List<Dictionary<string, object>> Parse(SqlDataReader reader)
        {
            var ans = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                var dict = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    object val = reader.GetValue(i);
                    dict.Add(name, val);
                }
                ans.Add(dict);
            }
            reader.Close();
            return ans;
        }
    }
}
