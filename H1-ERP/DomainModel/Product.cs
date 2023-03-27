using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Product

    {
        ErrorHandling.ErrorHandling errorHandling =new(); //obj for ErrorHandling Class
       
        private int _productId;
        private string _Name;
        private string _description;
        private decimal _sellingPrice;
        private decimal _purchasePrice;
        private string _location;
        private int _productQuantity;
        private int _productquantity;
        private int _unit;
        /*---------------------------------*/


        public int ProductId        
        {
            get
            {
                return errorHandling.IsNull(_productId);
               
            }
            
            set 
            {
                _productId = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/

        public string Name
        {

            get 
            { 
                return errorHandling.IsNull(_Name);
            }
            set { _Name = errorHandling.IsNull(value);  
            
            }
        }
        /*---------------------------------*/

        public string Description
        {
            get 
            {
                return errorHandling.IsNull (_description);
            }
            set
            {
                _description =errorHandling.IsNull (value);
            }
        }
        /*---------------------------------*/

        public decimal SellingPrice
        {
            get
            { 
                return errorHandling.IsNull (_sellingPrice); 
            }
            set 
            {
                _sellingPrice = errorHandling.IsNull (value);
            }
        }
        /*---------------------------------*/

        public decimal PurchasePrice
        {
            get
            {
                return errorHandling.IsNull(_purchasePrice);
            }
            set 
            {
                _purchasePrice = errorHandling.IsNull (value); 
            }
        }
        /*---------------------------------*/

        public string Location
        {
            get 
            { 
                return errorHandling.IsNull (_location);
            }
            set
            {
                _location = errorHandling.IsNull (value); 
            }
        }
        /*---------------------------------*/

        public int ProductQuantity
        {
            get 
            {
                return errorHandling.IsNull (_productquantity);
            }
            set
            {
                _productquantity = errorHandling.IsNull (value);
            }
        }
        /*---------------------------------*/

        public int Unit
        {
            get 
            {
                return errorHandling.IsNull (_unit);
            }
            set
            {
                _unit = errorHandling.IsNull (value); 
            }
        }
        /*---------------------------------*/
        public decimal CalculateProfit()
        {
            return SellingPrice - PurchasePrice;
        }

        public decimal CalculateProfitPercentage()
        {
            return (CalculateProfit() / SellingPrice) * 100;
        }
    }
}