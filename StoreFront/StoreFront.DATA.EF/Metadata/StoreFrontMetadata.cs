using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.DATA.EF/*.Metadata*///commented out because it needs to match the namespace of the .tt files
{
    //class StoreFrontMetadata
    //{
    //} take out because it is no longer needed
    #region BlendMetaData
   public class BlendMetaData
    {
        [Display(Name = "Blend")]
        [StringLength(25, ErrorMessage = "* The value must be 25 characters or less.")]
        [Required(ErrorMessage = "*Required")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string BlendName { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [StringLength(100, ErrorMessage = "* Value must be 100 characters or less")]
        public string Description { get; set; }
    }
    [MetadataType(typeof(BlendMetaData))]
    public partial class Blend
    {

    }
    #endregion

    #region DepartmentMetaData
    public class DepartmentMetaData
    {
        [Display(Name = "Department")]
        [StringLength(15, ErrorMessage = "* Value must be 15 characters or less")]
        [Required(ErrorMessage = "* Required")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string DepartmentName { get; set; }
    }
    [MetadataType(typeof(DepartmentMetaData))]
    public partial class Department
    {
       
    }
    #endregion

    #region EmployeeMetaData
    public class EmployeeMetaData
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "* Required")]
        [StringLength(15, ErrorMessage = "* First Name must be 15 characters or less")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "* Required")]
        [StringLength(15, ErrorMessage = "* First Name must be 15 characters or less")]
        public string LastName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger.")]
        [DisplayFormat(DataFormatString = "{0:c}", NullDisplayText = "[N/A]")]
        public Nullable<decimal> Salary { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string Position { get; set; }

        [Display(Name = "Direct Report")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public Nullable<int> DirectReportID { get; set; }

        [Display(Name = "Employed")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public Nullable<bool> StillEmployed { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "* City must be 50 characters or less")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string City { get; set; }

        [StringLength(7, MinimumLength = 2, ErrorMessage = "* Value must be 7 characters or less")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string Region { get; set; }

        [StringLength(15, ErrorMessage = "* Value must be 15 characters or less")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string Country { get; set; }

        [Display(Name = "Hire Date")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public Nullable<System.DateTime> HireDate { get; set; }
    }
    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {

    }
    #endregion

    #region ProductMetaData
    public class ProductMetaData
    {
        [Display(Name = "Coffee")]
        [StringLength(15, ErrorMessage = "* Value must be 15 characters or less")]
        public string ProductName { get; set; }

        [Display(Name = "Price per Unit")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Range(0, double.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger.")]
        public Nullable<decimal> UnitPrice { get; set; }

        [Display(Name = "In Stock")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Range(0, int.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger.")]
        public Nullable<short> UnitsInStock { get; set; }

        [Display(Name = "Units Ordered")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        [Range(0, int.MaxValue, ErrorMessage = "* Value must be a valid number, 0 or larger.")]
        public Nullable<short> UnitsOnOrder { get; set; }

        [Display(Name = "Blend Number")]
        public Nullable<int> BlendID { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        [StringLength(100, ErrorMessage = "* Value must be 100 characters or less")]
        public string Description { get; set; }

        [Display(Name = "Product Status")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public Nullable<int> ProductStatusID { get; set; }

        [DisplayFormat(NullDisplayText = "[N/A]")]
        public byte[] Image { get; set; }

        //[Display(Name = "Status")]
        //public virtual ProductStatus ProductStatus { get; set; }

        [Display(Name = "Category Status")]
        public int CategoryID { get; set; }
    }
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {

    }
    #endregion

    #region ProductStatusMetaData
    public class ProductStatusMetaData
    {
        [Display(Name = "Status")]
        [DisplayFormat(NullDisplayText = "[N/A]")]
        public string StatusName { get; set; }
    }
    [MetadataType(typeof(ProductStatusMetaData))]
    public partial class ProductStatus
    {

    }
    #endregion

    #region ShopMetaData
    public class CategoryMetaData
    {
        [Display(Name = "Category")]
        [StringLength(50, ErrorMessage = "* Value must be 50 characters or less")]
        public string CategoryName { get; set; }
    }
    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category
    {

    }
    #endregion
}
