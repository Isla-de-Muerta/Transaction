using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Transaction
{
    public class AeroDal : IDisposable
    {
        private const string ConnectionString = "Data Source=N205-10;Initial Catalog=aero;Integrated Security=True";
        private SqlConnection _sqlConnection;
        public AeroDal()
        {
            _sqlConnection = new SqlConnection(ConnectionString);
            _sqlConnection.Open();
        }

        public string RegisterPassenger(int trip_no, string passengerName, int passengerId, string place)
        {
            try
            {
                var addPassenger = string.Format("insert into Passenger (ID_psg, name) values ({0}, '{1}')",
                    passengerId,
                    passengerName);

                var addTripPassenger = string
                    .Format("insert into Pass_in_trip (trip_no, date, ID_psg, place)" + 
                    "values({0}, GETDATE(), {1}, '{2}')", trip_no, passengerId, place);

                var addPassengerCommand = new SqlCommand(addPassenger, _sqlConnection);
                var addTripPassengerCommand = new SqlCommand(addTripPassenger, _sqlConnection);

                SqlTransaction sqlTransaction = _sqlConnection.BeginTransaction();

                try
                {
                    addPassengerCommand.Transaction = sqlTransaction;
                    addTripPassengerCommand.Transaction = sqlTransaction;

                    addPassengerCommand.ExecuteNonQuery();
                    addTripPassengerCommand.ExecuteNonQuery();

                    sqlTransaction.Commit();
                    return "Passenger successfully registered";
                }
                catch (SqlException)
                {
                    sqlTransaction.Rollback();
                    return "Passenger hasn't been registered";
                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public string GetPassengersByTripName(string tripName)
        {
            try
            {
                var sqlCommand = "[getByTripName]";
                var command = new SqlCommand(sqlCommand, _sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@name", tripName));

                var sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Console.WriteLine(string.Format("{0}", sqlDataReader[1]));
                }
                return "Successfully ";
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
        }

        public void Dispose()
        {
            _sqlConnection.Close();
        }
    }
}
