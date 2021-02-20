using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "*Please provide a Name*")]
        //All we are going to provie the class is the properties class member.
        public string Name { get; set; }

        [Required(ErrorMessage = "*Please provide an email*")]
        [DataType(DataType.EmailAddress)] //        [RegularExpression("regex", ErrorMessage = "* Provide a valid email *")] is the same thing as datatype.emailaddress
          //if you want to ensure your validatiocustomize the validation, you can still use a regular expression(regexlib.com).

        public string Email { get; set; }

        [Required(ErrorMessage = "*A subject is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "* Please provide a message *")]
        [UIHint("MultilineText")]//changes the input to a textarea
        public string Message { get; set; }

    }
}