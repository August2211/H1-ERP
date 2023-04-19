using H1_ERP.DomainModel;
using System.Data.SqlClient;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        public Customer GetCustomerFromID(int id)
        {
            //for a customer obejct we're gonna need an adress aswell for the customer 
            Customer result = new Customer();
            result.CustomerId = id;
            Address address = new Address();
            //Get the details of the Customer from the person table
            var SelectedCustomer = GetdataFastFromJoinsWithouttheKeyvalueparoftheId($"SELECT " +
                $"[Customer.Customers].CustomerID, " +
                $"[Customers.Person].FirstName, " +
                $"[Customers.Person].LastName, " +
                $"[Customers.Person].Email, " +
                $"[Customers.Person].PhoneNumber, " +
                $"[Customer.Adress].AdressID, " +
                $"[Customer.Adress].RoadName, " +
                $"[Customer.Adress].StreetNumber, " +
                $"[Customer.Adress].ZipCode, " +
                $"[Customer.Adress].City, " +
                $"[Customer.Adress].Country, " +
                $"[Customers.Person].PersonID " +
                $"FROM [Customer.Customers] " +
                $"INNER JOIN [Customers.Person] on " +
                $"[Customers.Person].PersonID = [Customer.Customers].PersonID " +
                $"INNER JOIN [Customer.Adress] on " +
                $"[Customer.Adress].AdressID = [Customers.Person].AdressID " +
                $"WHERE CustomerID = {id}");
            foreach (var row in SelectedCustomer.Values)
            {
                result.CustomerId = Convert.ToInt32(SelectedCustomer.ElementAt(0).Value[0]);
                result.FirstName = row[1].ToString();
                result.LastName = row[2].ToString();
                result.Email = row[3].ToString();
                result.PhoneNumber = row[4].ToString();
                result.PersonID = Convert.ToUInt32(row[11]);
            }
            address.AdressID = Convert.ToUInt32(SelectedCustomer.ElementAt(0).Value[5]);
            address.RoadName = SelectedCustomer.ElementAt(0).Value[6].ToString();
            address.StreetNumber = SelectedCustomer.ElementAt(0).Value[7].ToString();
            address.ZipCode = SelectedCustomer.ElementAt(0).Value[8].ToString();
            address.City = SelectedCustomer.ElementAt(0).Value[9].ToString();
            address.Country = SelectedCustomer.ElementAt(0).Value[10].ToString();
            

            //Only set the address if the id is not 0
            if (address.AdressID != 0)
            {
                result.Address = address;
            }
            return result;
        }

        public List<Customer> GetAllCustomers()
        {
            SqlConnection connection = getConnection();
            List<Customer> result = new List<Customer>();
            var Allcustomers = GetDatafast("SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers] INNER JOIN " +
                "[dbo].[Customers.Person] ON [dbo].[Customers.Person].PersonID = [dbo].[Customer.Customers].PersonID INNER JOIN " +
                "[dbo].[Customer.Adress] ON [dbo].[Customers.Person].AdressID = [dbo].[Customer.Adress].AdressID");

            foreach (var Customerrow in Allcustomers.Values)
            {
                Customer tempcustomer = new Customer();
                tempcustomer.CustomerId = Convert.ToInt32(Customerrow[0]);
                if (Customerrow[1].GetType() != typeof(DBNull))
                {
                    tempcustomer.LastPurchaseDate = Convert.ToDateTime(Customerrow[1]);
                }
                else { tempcustomer.LastPurchaseDate = null; }
                tempcustomer.PersonID = Convert.ToUInt32(Customerrow[2]);
                tempcustomer.FirstName = Customerrow[4].ToString();
                tempcustomer.LastName = Customerrow[5].ToString();
                tempcustomer.Email = Customerrow[6].ToString();
                tempcustomer.PhoneNumber = Customerrow[7].ToString();
                Address adress = new Address();
                adress.AdressID = Convert.ToUInt32(Customerrow[8]);
                adress.RoadName = Customerrow[10].ToString();
                adress.StreetNumber = Customerrow[11].ToString();
                adress.ZipCode = Customerrow[12].ToString();
                adress.City = Customerrow[13].ToString();
                adress.Country = Customerrow[14].ToString();
                tempcustomer.Address = adress;
                result.Add(tempcustomer);
            }
            connection.Close();
            return result;
        }


        /// <summary>
        /// Inserts a Customer to our DB table
        /// </summary>
        /// <param name="input"></param>
        public void InsertCustomer(Customer input)
        {
            //findes a the adress which has just been created
            uint adressid = InsertAddress(input.Address);
            //finds the current person that has been created for the customer
            uint personID = InsertPerson(input, adressid);
            DateTime? purchasenull = null;

            if (input.LastPurchaseDate == null || input.LastPurchaseDate.ToString() == "")
            {
                purchasenull = null;
            }
            else { purchasenull = DateTime.Parse(input.LastPurchaseDate.ToString()); }
            string SwitchThedatestring = purchasenull.ToString();

            var splitted = SwitchThedatestring.Split('-', ' ');
            string TempDate = splitted[0];
            splitted[0] = splitted[2] +"-";
            splitted[1] += "-";
            splitted[2] = TempDate + " ";
            var ez = string.Join("",splitted);

            Exec_SQL_Command($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Customers] (LastPurchaseDate,PersonID) VALUES(CONVERT(datetime,'{ez}',120),{personID})");
        }
        /// <summary>
        /// Updates a Customer 
        /// </summary>
        /// <param name="input"></param>
        public void UpdateCustomer(Customer input)
        {
            string nullhandelstring = input.LastPurchaseDate.ToString();
            if (nullhandelstring == null || nullhandelstring == "")
            {
                nullhandelstring = "NULL";
            }
            Exec_SQL_Command($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Adress] SET RoadName = '{input.Address.RoadName}',StreetNumber = '{input.Address.StreetNumber}',ZipCode = '{input.Address.ZipCode}',City = '{input.Address.City}', Country = '{input.Address.Country}' WHERE AdressID = {input.Address.AdressID}");
            Exec_SQL_Command($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customers.Person] SET FirstName = '{input.FirstName}', LastName = '{input.LastName}', Email = '{input.Email}', PhoneNumber = '{input.PhoneNumber}' WHERE PersonID = {input.PersonID}");
            Exec_SQL_Command($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Customers] SET LastPurchaseDate = {nullhandelstring} WHERE CustomerID = {input.CustomerId}");
        }

        /// <summary>
        /// DeleteSalesOrderHeaderFromID's a customer by an ID from the DB
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteCustomer(int ID)
        {
            Customer customercustomer = GetCustomerFromID(ID);


            var FindcustmertoDelete = GetData($"SELECT OrderID FROM [dbo].[Sales.Orders] WHERE CustomerID = {customercustomer.CustomerId}");
            //it means they dont have an order then we look them up in some other tables
            if (FindcustmertoDelete != null)
            {
                int OrderID = 0;
                foreach (var id in FindcustmertoDelete.Values)
                {
                    OrderID = Convert.ToInt32(id[0]);
                    Exec_SQL_Command($"DELETE FROM [dbo].[Sales.Orderlines] WHERE OrderID = {OrderID}");
                }
                Exec_SQL_Command($"DELETE FROM [dbo].[Sales.Orders] WHERE CustomerID = {ID}");
            }
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers] WHERE CustomerID ={customercustomer.CustomerId}");
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] WHERE PersonID = {customercustomer.PersonID}");
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {customercustomer.Address.AdressID}");
        }
        /// <summary>
        /// Finds the ID of a newly inserted address 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private uint InsertAddress(Address address)
        {
            uint AddressID = 0;
            Exec_SQL_Command($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Adress] (RoadName, StreetNumber, ZipCode, City, Country) VALUES  ('{address.RoadName}','{address.StreetNumber}','{address.ZipCode}','{address.City}','{address.Country}')");
            var GetAdressID = GetData("SELECT TOP (1) [AdressID] FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] ORDER BY [AdressID] desc;");

            foreach (var id in GetAdressID.Values)
            {
                AddressID = Convert.ToUInt32(id[0]);
            }
            return AddressID;
        }
        /// <summary>
        /// Finds the ID of a newly Inserted Person 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="adressid"></param>
        /// <returns></returns>
        private uint InsertPerson(Customer input, uint adressid)
        {
            uint PersonID = 0;
            Exec_SQL_Command($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customers.Person] (FirstName,LastName,Email,PhoneNumber,AdressID) VALUES('{input.FirstName}','{input.LastName}','{input.Email}','{input.PhoneNumber}',{adressid})");
            var PersonIDdata = GetData("SELECT TOP (1) [PersonID] FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] ORDER BY [PersonID] desc;");
            foreach (var id in PersonIDdata.Values)
            {
                PersonID = Convert.ToUInt32(id[0]);
            }
            return PersonID;
        }
    }
}
