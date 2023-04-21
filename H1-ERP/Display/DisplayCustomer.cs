using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static H1_ERP.Display.DisplayCompany;

namespace H1_ERP.Display
{
    public class DisplayCustomer : Screen
    {
        public override string Title { get; set; } = "Customers "; 

        private class CustomerDisplay
        {
            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public string Title3 { get; set; }
            public string Title4 { get; set; }
            public string Title5 { get; set; }
            public string Title6 { get; set; }
            public int IntboxOutput { get; set; } = -1; 

            public CustomerDisplay(int referenceNumber, string fullname, string phoneNumber, string email)
            {
                Title1 = referenceNumber.ToString();      
                Title2= fullname.Replace("\"","");
                Title3 = phoneNumber;   
                Title4 = email;                
            }
            public CustomerDisplay(string fullname,string Roadname, string Country,string City,string Postalcode,DateTime? LastPruchaseTime)
            {
                Title1 = fullname;
                Title2 = Roadname;
                Title3 = Country;
                Title4= City;
                Title5 = Postalcode;
                Title6 = LastPruchaseTime.ToString();
            }
            public CustomerDisplay(string title1)
            {
                Title1 = title1;
            }
        } 
        /// <summary>
        /// main way of displaying customers
        /// </summary>
        protected override void Draw()
        {
            Clear(this); 
            ListPage<CustomerDisplay> listPage= new ListPage<CustomerDisplay>();
            
            
            DataBase.DataBase data = new DataBase.DataBase();
            List<Customer> customers = data.GetAllCustomers(); 
            foreach(var customer in customers)
            {
                listPage.Add(new CustomerDisplay(customer.CustomerId, customer.FullName(), customer.PhoneNumber, customer.Email)); 
            }
            //we add the length of the columns by taking the longest possible string and then we size the column by that number 
            listPage.AddColumn("ReferenceNumber", "Title1", 15);
            listPage.AddColumn("Fullname", "Title2", customers.Select(x => x.FullName().Length).Max());
            listPage.AddColumn("PhoneNumber", "Title3", customers.Select(x => x.PhoneNumber.Length).Max());
            listPage.AddColumn("Email", "Title4",customers.Select(x => x.Email.Length).Max());
            
            Form<CustomerDisplay> form = new Form<CustomerDisplay>();
            //User get's to press enter on 1 of our lines and afterwards they can se the details with a new customerdisplay of one customer.
            //Takes a customerdisplay and afterwards does nothing with it becuase they create a customer from scratch
            Action<CustomerDisplay> AddcustomerFunction = delegate (CustomerDisplay customer)
            {
                Clear(this);
                listPage = new ListPage<CustomerDisplay>();
                listPage.AddColumn("Add Customer Details", "Title1",40);
                listPage.Add(new CustomerDisplay("Firstname"));
                listPage.Add(new CustomerDisplay("Lastname"));
                listPage.Add(new CustomerDisplay("Email"));
                listPage.Add(new CustomerDisplay("Roadname"));
                listPage.Add(new CustomerDisplay("Zipcode"));
                listPage.Add(new CustomerDisplay("StreetNumber"));
                listPage.Add(new CustomerDisplay("City"));
                listPage.Add(new CustomerDisplay("PhoneNumber"));
                listPage.Add(new CustomerDisplay("Country"));
                form = new Form<CustomerDisplay>();
              
                Hashtable Columnsandvalues = new Hashtable();
                var chosenvalue = listPage.Select();
                
                bool NotDoneEditing = true;
                if (chosenvalue == null)
                {
                    NotDoneEditing= false;
                }
                while (NotDoneEditing == true)
                {

                    if (chosenvalue != null)
                    {
                        var oldpos = Console.GetCursorPosition();

                        Console.SetCursorPosition(oldpos.Left + 1, oldpos.Top + 4);

                        Console.Write("                                        ");
                        Console.SetCursorPosition(oldpos.Left + 1, oldpos.Top + 4);

                        var newval = Console.ReadLine();
                        if (newval != chosenvalue.Title1)
                        {
                            chosenvalue.Title1 = newval;
                            if (Columnsandvalues.ContainsKey(oldpos.Top))
                            {
                                Columnsandvalues[oldpos.Top] = newval;

                            }
                            if (!Columnsandvalues.ContainsKey(oldpos.Top))
                            {
                                Columnsandvalues.Add(oldpos.Top, newval);
                            }
                        }
                        Clear(this);
                    }
                    chosenvalue = listPage.Select();


                    if (chosenvalue == null)
                    {
                        NotDoneEditing = false;
                        Address adress1 = new Address();
                        Customer customer1 = new Customer();
                        // if the user whishes to exit our add customer screen they have to have all the columns filled otherwise we throw an error and the display it to the customer 
                        try
                        {
                            if (Columnsandvalues.ContainsKey(0))
                            {
                                customer1.FirstName = Columnsandvalues[0].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the firstname field");
                            }
                            if (Columnsandvalues.ContainsKey(1))
                            {
                                customer1.LastName = Columnsandvalues[1].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the lastname field");
                            }
                            if (Columnsandvalues.ContainsKey(2))
                            {
                                customer1.Email = Columnsandvalues[2].ToString();
                            }
                            else { throw new DataException(@"You need to fill the Email field"); }
                            if (Columnsandvalues.ContainsKey(3))
                            {

                                adress1.RoadName = Columnsandvalues[3].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the roadname field");
                            }

                            if (Columnsandvalues.ContainsKey(4))
                            {
                                adress1.ZipCode = Columnsandvalues[4].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the Zipcode field");
                            }

                            if (Columnsandvalues.ContainsKey(5))
                            {
                                adress1.StreetNumber = Columnsandvalues[5].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the Streetnumber field");
                            }

                            if (Columnsandvalues.ContainsKey(6))
                            {
                                adress1.City = Columnsandvalues[6].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the City field");
                            }

                            if (Columnsandvalues.ContainsKey(7))
                            {
                                customer1.PhoneNumber = Columnsandvalues[7].ToString();
                            }
                            else
                            {
                                
                                throw new DataException(@"You need to fill the Phonenumber field");
                            }

                            if (Columnsandvalues.ContainsKey(8))
                            {
                                adress1.Country = Columnsandvalues[8].ToString();
                            }
                            else
                            {
                                throw new DataException(@"You need to fill the country field");
                            }
                        } catch(DataException e)
                        {
                            Console.Clear();
                            Console.WriteLine("Y" + e.Message + " Press any key to countinue");
                            Console.ReadKey(); 
                            DisplayCustomer display = new DisplayCustomer();
                            Screen.Display(display); 
                        }
                        customer1.Address = adress1;
                        data.InsertCustomer(customer1);
                        Console.Clear();
                        Console.WriteLine("you have sucessfully Created a customer! :) press any key to countinue");
                        Console.ReadKey();
                        DisplayCustomer updatedScreen = new DisplayCustomer();
                        Screen.Display(updatedScreen);
                    }
                }
                Console.Clear();
                DisplayCustomer updatedScreen1 = new DisplayCustomer();
                Screen.Display(updatedScreen1);
            };

            Action<CustomerDisplay> GoBackFunction = delegate (CustomerDisplay display)
            {
                MenuDisplay menuDisplay = new MenuDisplay();
                Screen.Display(menuDisplay);
            };
            listPage.AddKey(ConsoleKey.Q, GoBackFunction);

            //when this action is fired we take the current customerdisplay ID and delete it in the database 
            Action<CustomerDisplay> Deletefunction = delegate (CustomerDisplay customer)
            {
                int idnumber = Convert.ToInt32(customer.Title1);              
                data.DeleteCustomer(idnumber);           
                Draw(); 
            };
            //Take's the current ID on display and edits the chosen values and update's it in the database 
            Action<CustomerDisplay> Editfunction = delegate (CustomerDisplay customer)
            {
                Console.Clear();    
                int idcustomer = Convert.ToInt32(customer.Title1); 
                var tempcustomer =  data.GetCustomerFromID(idcustomer);
                listPage = new ListPage<CustomerDisplay>();
                listPage.AddColumn("Edit", "Title1", 30);
                listPage.Add(new CustomerDisplay(tempcustomer.FirstName));
                listPage.Add(new CustomerDisplay(tempcustomer.LastName));
                listPage.Add(new CustomerDisplay(tempcustomer.Email));
                listPage.Add(new CustomerDisplay(tempcustomer.Address.RoadName));
                listPage.Add(new CustomerDisplay(tempcustomer.Address.ZipCode));
                listPage.Add(new CustomerDisplay(tempcustomer.Address.StreetNumber));
                listPage.Add(new CustomerDisplay(tempcustomer.Address.City));
                listPage.Add(new CustomerDisplay(tempcustomer.PhoneNumber)); 
                form = new Form<CustomerDisplay>();
                listPage.Draw();
                Console.WriteLine("Choose a row with the arrow keys and then press enter for going to change a row write a value then press enter"); 
                var chosenvalue = listPage.Select();
                bool NotDoneEditing = true; 
                if(chosenvalue == null)
                {
                    NotDoneEditing= false;
                }
                Hashtable Columnsandvalues = new Hashtable();
                while (NotDoneEditing == true)
                {

                    if (chosenvalue != null)
                    {
                        var oldpos = Console.GetCursorPosition();
                          
                        Console.SetCursorPosition(oldpos.Left + 1, oldpos.Top + 4);

                        Console.Write("                              ");
                        Console.SetCursorPosition(oldpos.Left + 1, oldpos.Top + 4);

                        var newval = Console.ReadLine();
                        if(newval != chosenvalue.Title1)
                        {
                            chosenvalue.Title1 = newval;
                            if (Columnsandvalues.ContainsKey(oldpos.Top))
                            {
                                Columnsandvalues[oldpos.Top] = newval;

                            }
                            if (!Columnsandvalues.ContainsKey(oldpos.Top))
                            {
                               Columnsandvalues.Add(oldpos.Top, newval);
                            }
                        }
                        Clear(this); 
                    }
                    chosenvalue = listPage.Select();
                    
                    
                    if (chosenvalue == null)
                    {
                        NotDoneEditing= false;
                        Address adress1 = new Address(); 
                        Customer customer1 = new Customer();
                        //if the customer in the edit screen wishes to edit something we look at what we have as changed values
                        if (Columnsandvalues.ContainsKey(0))
                        {
                            customer1.FirstName = Columnsandvalues[0].ToString();
                        }
                        else
                        {
                            customer1.FirstName = tempcustomer.FirstName;
                        }
                        if (Columnsandvalues.ContainsKey(1))
                        {
                           customer1.LastName = Columnsandvalues[1].ToString();
                        }
                        else 
                        {
                            customer1.LastName = tempcustomer.LastName; 
                        }
                        if(Columnsandvalues.ContainsKey(2)) 
                        { 
                            customer1.Email = Columnsandvalues[2].ToString();
                        }
                        else { customer1.Email = tempcustomer.Email;}
                        if (Columnsandvalues.ContainsKey(3)) 
                        {
                           
                            adress1.RoadName = Columnsandvalues[3].ToString();
                        }
                        else 
                        { 
                            adress1.RoadName = tempcustomer.Address.RoadName;
                        }
                        
                        if (Columnsandvalues.ContainsKey(4)) 
                        { 
                             adress1.ZipCode = Columnsandvalues[4].ToString(); 
                        }
                        else 
                        {
                            adress1.ZipCode = tempcustomer.Address.ZipCode;
                        }  
                        
                        if (Columnsandvalues.ContainsKey(5))
                        { 
                            adress1.StreetNumber = Columnsandvalues[5].ToString();
                        }
                        else 
                        {
                            adress1.AdressID = tempcustomer.Address.AdressID;
                            adress1.StreetNumber= tempcustomer.Address.StreetNumber;
                        }

                        if (Columnsandvalues.ContainsKey(6))
                        {
                            adress1.City = Columnsandvalues[6].ToString();
                        }
                        else
                        {
                            adress1.City = tempcustomer.Address.City;
                        }

                        if (Columnsandvalues.ContainsKey(7))
                        {
                             customer1.PhoneNumber = Columnsandvalues[7].ToString();
                        } 
                        else 
                        {
                            customer1.PhoneNumber = tempcustomer.PhoneNumber;
                        }
                        // we take the remaning info and update it with the new 1 
                        customer1.PersonID = tempcustomer.PersonID;
                        customer1.CustomerId = tempcustomer.CustomerId;
                        adress1.Country= tempcustomer.Address.Country;
                        
                        customer1.Address = adress1;

                        data.UpdateCustomer(customer1);
                        Console.Clear();
                        Console.WriteLine("you have sucessfully updated a customer! :) press any key to countinue");
                        Console.ReadKey();
                    }
                }

                Console.Clear();
                DisplayCustomer updatedScreen = new DisplayCustomer();
                Screen.Display(updatedScreen);
            };
            //adds all of the functionality and the keys accordingly 
            listPage.AddKey(ConsoleKey.F3,Editfunction);
            listPage.AddKey(ConsoleKey.F5, Deletefunction);
            listPage.AddKey(ConsoleKey.F2,AddcustomerFunction);
            Console.BufferHeight = 200; 

            var SelectedRow= listPage.Select(); 
           
            if(SelectedRow == null)
            {
                Clear(this);
                MenuDisplay menu = new MenuDisplay();
                Screen.Display(menu); 
            }

            if (SelectedRow != null)
            {

                if (SelectedRow.Title1 != null)
                {
                    Clear(this);
                    Console.Clear();
                    ListPage<CustomerDisplay> SelectedCustomerDisplay = new ListPage<CustomerDisplay>();
                    SelectedCustomerDisplay.AddColumn("Fullname", "Title1");
                    SelectedCustomerDisplay.AddColumn("RoadName", "Title2");
                    SelectedCustomerDisplay.AddColumn("Country", "Title3");
                    SelectedCustomerDisplay.AddColumn("City", "Title4");
                    SelectedCustomerDisplay.AddColumn("PostalCode", "Title5");
                    Customer SelectedCustomer = customers.Select(x => x).Where(x => x.CustomerId == Convert.ToInt32(SelectedRow.Title1)).FirstOrDefault();
                    SelectedCustomerDisplay.Add(new CustomerDisplay(SelectedCustomer.FullName(), SelectedCustomer.Address.RoadName, SelectedCustomer.Address.Country, SelectedCustomer.Address.City, SelectedCustomer.Address.ZipCode, SelectedCustomer.LastPurchaseDate));
                    SelectedCustomerDisplay.Draw();
                }
                var alleheaders = data.GetAll();
                if (SelectedRow.Title1 == null)
                {
                    listPage.Draw();
                }
            }
        } 
    }
}
