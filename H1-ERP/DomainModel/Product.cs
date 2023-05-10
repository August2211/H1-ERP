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
        ErrorHandling.ErrorHandling errorHandling = new(); //obj for ErrorHandling Class

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


        public int ProductID
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

        public string ProductName
        {

            get
            {
                return errorHandling.IsNull(_Name);
            }
            set
            {
                _Name = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/

        public string ProductDescription
        {
            get
            {
                return errorHandling.IsNull(_description);
            }
            set
            {
                _description = errorHandling.IsNull(value);
            }
        }
        /*---------------------------------*/

        public decimal ProductSalePrice
        {
            get
            {
                return errorHandling.IsNegative(_sellingPrice);
            }
            set
            {
                _sellingPrice = errorHandling.IsNegative(value);
            }
        }
        /*---------------------------------*/

        public decimal ProductPurchasePrice
        {
            get
            {
                return errorHandling.IsNegative(_purchasePrice);
            }
            set
            {
                _purchasePrice = errorHandling.IsNegative(value);
            }
        }
        /*---------------------------------*/

        public string ProductLocation
        {
            get
            {
                return errorHandling.IsNull(_location);
            }
            set
            {
                _location = errorHandling.IsNull(value);
            }
        }
        /*---------------------------------*/

        public int ProductQuantity
        {
            get
            {
                return errorHandling.IsNull(_productquantity);
            }
            set
            {
                _productquantity = errorHandling.IsNull(value);
            }
        }
        /*---------------------------------*/

        public int ProductUnit
        {
            get
            {
                return errorHandling.IsNull(_unit);
            }
            set
            {
                _unit = errorHandling.IsNull(value);
            }
        }
        /*---------------------------------*/

        /// <summary>
        /// Calculates the profit of the product.
        /// </summary>
        /// <returns>The profit as decimal</returns>
        public decimal CalculateProfit()
        {
            return ProductSalePrice - ProductPurchasePrice;
        }

        /// <summary>
        /// Calculates the profit percentage of the product.
        /// </summary>
        /// <returns>The profit percentage as decimal</returns>
        public decimal CalculateProfitPercentage()
        {
            return (CalculateProfit() / ProductSalePrice) * 100;
        }
    }
}