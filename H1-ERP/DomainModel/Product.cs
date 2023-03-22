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

        private string _Name;

        public string Name
        {

            get 
            { 
                return errorHandling.IsNull(_Name);
            }
            set { _Name = errorHandling.IsNull(value);  
            
            }
        }

        private string _description;

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

        private decimal _salePrice;

        public decimal SalePrice
        {
            get
            { 
                return errorHandling.IsNull (_salePrice); 
            }
            set 
            {
                _salePrice = errorHandling.IsNull (_salePrice);
            }
        }
        /*---------------------------------*/

        private decimal _purchasePrice;

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

        private string _location;

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

        private int _productquantity;

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

        private int _unit;

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
    }
}