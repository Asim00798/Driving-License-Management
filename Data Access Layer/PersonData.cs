using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Data_Access_Layer
{
    public class clsPersonData
    {

        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref short Gendor, ref string Address, ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    //ThirdName: allows null in database so we should handle null
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }

                    LastName = (string)reader["LastName"];
                    NationalNo = (string)reader["NationalNo"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];


                    //Email: allows null in database so we should handle null
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gendor, ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    //ThirdName: allows null in database so we should handle null
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    //Email: allows null in database so we should handle null
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    //ImagePath: allows null in database so we should handle null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }



        public static int AddNewPerson(string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                                   DateOfBirth,Gendor,Address,Phone, Email, NationalityCountryID,ImagePath)
                             VALUES (@FirstName, @SecondName,@ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth,@Gendor,@Address,@Phone, @Email,@NationalityCountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return PersonID;
        }



        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor=@Gendor,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);


            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }      

        public static bool DeletePerson(int PersonID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete People 
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        /////////*******filtering datatable from database*****////////////

       public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
              @"  SELECT  
                  People.PersonID,
                  People.NationalNo,
                  People.FirstName,
                  People.SecondName,
                  People.ThirdName, 
                  People.LastName,
                  People.DateOfBirth,  
                   CASE
                   WHEN People.Gendor = 0 THEN 'Male'
                   ELSE 'Female'
                   END as GenderCaption,

                   People.Phone, 
                   People.Email, 
                   Countries.CountryName

                   FROM 
                   People 
                   INNER JOIN
                   Countries ON People.NationalityCountryID = Countries.CountryID
                   ORDER BY People.FirstName
                   ";


            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }

       public static DataTable GetPersonByPersonID(int PersonID)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                           @"  SELECT  
                  People.PersonID,
                  People.NationalNo,
                  People.FirstName,
                  People.SecondName,
                  People.ThirdName, 
                  People.LastName,
                  People.DateOfBirth,  
                   CASE
                   WHEN People.Gendor = 0 THEN 'Male'
                   ELSE 'Female'
                   END as GenderCaption,

                   People.Phone, 
                   People.Email, 
                   Countries.CountryName

                   FROM 
                   People 
                   INNER JOIN
                   Countries ON People.NationalityCountryID = Countries.CountryID
                   WHERE PersonID = @PersonID
                   ORDER BY People.FirstName
                   ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetPersonByNationalNo(string NationalNo)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"  SELECT  
                                People.PersonID,
                                People.NationalNo,
                                People.FirstName,
                                People.SecondName,
                                People.ThirdName, 
                                People.LastName,
                                People.DateOfBirth,  
                                CASE
                                    WHEN People.Gendor = 0 THEN 'Male'
                                    ELSE 'Female'
                                END as GenderCaption,
                                People.Phone, 
                                People.Email, 
                                Countries.CountryName
                            FROM 
                                People 
                            INNER JOIN
                                Countries ON People.NationalityCountryID = Countries.CountryID
                            WHERE 
                                People.NationalNo Like @NationalNo + '%'
                            ORDER BY 
                                People.FirstName;

                                               ";
            ;

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetPersonByFirstName(string FirstName)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"  SELECT  
                  People.PersonID,
                  People.NationalNo,
                  People.FirstName,
                  People.SecondName,
                  People.ThirdName, 
                  People.LastName,
                  People.DateOfBirth,  
                   CASE
                   WHEN People.Gendor = 0 THEN 'Male'
                   ELSE 'Female'
                   END as GenderCaption,

                   People.Phone, 
                   People.Email, 
                   Countries.CountryName

                   FROM 
                   People 
                   INNER JOIN
                   Countries ON People.NationalityCountryID = Countries.CountryID
                   WHERE FirstName LIKE @FirstName + '%'
                   ORDER BY People.FirstName
                   ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", FirstName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetPersonBySecondName(string SecondName)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                        @"  SELECT  
                        People.PersonID,
                        People.NationalNo,
                        People.FirstName,
                        People.SecondName,
                        People.ThirdName, 
                        People.LastName,
                        People.DateOfBirth,  
                         CASE
                         WHEN People.Gendor = 0 THEN 'Male'
                         ELSE 'Female'
                         END as GenderCaption,

                         People.Phone, 
                         People.Email, 
                         Countries.CountryName

                         FROM 
                         People 
                         INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                         WHERE SecondName LIKE @SecondName + '%'
                         ORDER BY People.FirstName
                         ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetPersonByThirdName(string ThirdName)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                             @"  SELECT  
                        People.PersonID,
                        People.NationalNo,
                        People.FirstName,
                        People.SecondName,
                        People.ThirdName, 
                        People.LastName,
                        People.DateOfBirth,  
                         CASE
                         WHEN People.Gendor = 0 THEN 'Male'
                         ELSE 'Female'
                         END as GenderCaption,

                         People.Phone, 
                         People.Email, 
                         Countries.CountryName

                         FROM 
                         People 
                         INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                         WHERE ThirdName LIKE @ThirdName + '%'
                         ORDER BY People.FirstName
                         ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ThirdName", ThirdName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetPersonByLastName(string LastName)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"  SELECT  
                        People.PersonID,
                        People.NationalNo,
                        People.FirstName,
                        People.SecondName,
                        People.ThirdName, 
                        People.LastName,
                        People.DateOfBirth,  
                         CASE
                         WHEN People.Gendor = 0 THEN 'Male'
                         ELSE 'Female'
                         END as GenderCaption,

                         People.Phone, 
                         People.Email, 
                         Countries.CountryName

                         FROM 
                         People 
                         INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                         WHERE LastName LIKE @LastName + '%'
                         ORDER BY People.FirstName
                         ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LastName", LastName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetPersonByFullName(string FullName)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                 @"
                         SELECT 
                         People.PersonID, 
                         People.NationalNo,
                         People.FirstName, 
                         People.SecondName, 
                         People.ThirdName, 
                         People.LastName,
                         People.DateOfBirth, 
                         CASE
                             WHEN People.Gendor = 0 THEN 'Male'
                             ELSE 'Female'
                         END as GenderCaption, 
                         People.Phone, 
                         People.Email, 
                         Countries.CountryName, 
                         CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) AS FullName
                     FROM 
                         People 
                     INNER JOIN 
                         Countries ON People.NationalityCountryID = Countries.CountryID
                     WHERE 
                         CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) LIKE '%' + @FullName + '%'
                     ORDER BY 
                         People.FirstName

	             ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", FullName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetAllPeopleByNationality(string CountryName)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"
                           SELECT 
                                People.PersonID, 
                                People.NationalNo,
                                People.FirstName, 
                                People.SecondName, 
                                People.ThirdName, 
                                People.LastName,
                                People.DateOfBirth, 
                                CASE
                                    WHEN People.Gendor = 0 THEN 'Male'
                                    ELSE 'Female'
                                END as GenderCaption, 
                                People.Phone, 
                                People.Email,  
                                Countries.CountryName
                            FROM 
                                People 
                            INNER JOIN 
                                Countries ON People.NationalityCountryID = Countries.CountryID
                            WHERE 
                                Countries.CountryName LIKE @CountryName + '%'
                            ORDER BY 
                                People.FirstName;

	                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

       public static DataTable GetAllPeopleByNationality(int CountryID)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"
                                SELECT 
                                    People.PersonID, 
                                    People.NationalNo,
                                    People.FirstName, 
                                    People.SecondName, 
                                    People.ThirdName, 
                                    People.LastName,
                                    People.DateOfBirth, 
                                    CASE
                                        WHEN People.Gendor = 0 THEN 'Male'
                                        ELSE 'Female'
                                    END as GenderCaption,
                                    People.Phone, 
                                    People.Email, 
                                    Countries.CountryName, 

                                FROM 
                                    People 
                                INNER JOIN 
                                    Countries ON People.NationalityCountryID = Countries.CountryID
                                WHERE 
                                     Countries.CountryID = @CountryID
                                ORDER BY 
                                    People.FirstName
                                ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

       public static DataTable GetAllPeopleByGender(string Gender)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"
                           SELECT 
                            People.PersonID, 
                            People.NationalNo,
                            People.FirstName, 
                            People.SecondName, 
                            People.ThirdName, 
                            People.LastName,
                            People.DateOfBirth,  
                            CASE
                                WHEN People.Gendor = 0 THEN 'Male'
                                ELSE 'Female'
                            END as GenderCaption,
                            People.Phone, 
                            People.Email, 
                            Countries.CountryName
                        FROM 
                            People 
                        INNER JOIN 
                            Countries ON People.NationalityCountryID = Countries.CountryID
                        WHERE 
                            (People.Gendor = 0 AND 'Male' LIKE @Gender + '%') OR
                            (People.Gendor = 1 AND 'Female' LIKE @Gender + '%')
                        ORDER BY 
                            People.FirstName;


                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Gender", Gender);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

       public static DataTable GetPersonByEmail(string Email)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"
                                                    SELECT 
                            People.PersonID, 
                            People.NationalNo,
                            People.FirstName, 
                            People.SecondName, 
                            People.ThirdName, 
                            People.LastName,
                            People.DateOfBirth,  
                            CASE
                                WHEN People.Gendor = 0 THEN 'Male'
                                ELSE 'Female'
                            END as GenderCaption,
                            People.Phone, 
                            People.Email, 
                            Countries.CountryName
                        FROM 
                            People 
                        INNER JOIN 
                            Countries ON People.NationalityCountryID = Countries.CountryID
                        WHERE 
                            People.Email LIKE @Email + '%'
                        ORDER BY 
                            People.FirstName;

	                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", Email);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

       public static DataTable GetPersonByPhone(string Phone)
       {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query =
                            @"
                            SELECT 
                            People.PersonID, 
                            People.NationalNo,
                            People.FirstName, 
                            People.SecondName, 
                            People.ThirdName, 
                            People.LastName,
                            People.DateOfBirth,  
                            CASE
                                WHEN People.Gendor = 0 THEN 'Male'
                                ELSE 'Female'
                            END as GenderCaption,
                            People.Phone, 
                            People.Email, 
                            Countries.CountryName
                        FROM 
                            People 
                        INNER JOIN 
                            Countries ON People.NationalityCountryID = Countries.CountryID
                        WHERE 
                            People.Phone LIKE @Phone + '%'
                        ORDER BY 
                            People.FirstName;
	                            ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Phone", Phone);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
       }

    }
}
