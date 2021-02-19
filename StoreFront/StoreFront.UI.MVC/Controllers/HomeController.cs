using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFront.UI.MVC.Models; //entered using statement to have access to Family Member View Model and Contact View Model
//added below using statements for contact form functionality
using System.Net; //for network creds
using System.Net.Mail;

namespace StoreFront.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }
        public ActionResult Blends()
        {

            return View();
        }
        //Post Contact Action 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            //when a class has validation attributes, that validation should be checked BEFORE attempting to process any data.
            if (!ModelState.IsValid)
            {
                //send them back to the form, passing their inputs back to the form with the HTML form
                return View(cvm); //cvm object populates in this return populates the form with what the user input 
            }//end if
            //build the message - what we see when we receive the email
            string message = $"You have received an email from: {cvm.Name} with a subject of: {cvm.Subject}. Please respond to: {cvm.Email} with your response to the following message: <br/> {cvm.Message}";


            //MailMessage object (what sends the email from ASP.NET application- ADD USING STATEMENT FOR SYSTEM.NET.MAIL
            MailMessage mm = new MailMessage(
                //FROM 
                "webadmin@rachelmantei.com",
                //TO - this assumes forwarding from the host
                "webdevrach@gmail.com", //hardcoded forward to this email address
                                        //SUBJECT
                cvm.Subject,
                //BODY
                message
                );

            //MailMessage object properties
            //Allow HTML formatting in the email message
            mm.IsBodyHtml = true;
            //if you want to mark these emails with high priority
            mm.Priority = MailPriority.High;
            //respond to the senders email instead of our own SmarterAsp email address
            mm.ReplyToList.Add(cvm.Email);

            //StmpClient - Information from the host(smarterasp.net) that allows the email to actually be sent
            SmtpClient client = new SmtpClient("mail.rachelmantei.com");

            //client creds
            client.Credentials = new NetworkCredential("webadmin@rachelmantei.com", "P@ssw0rd");

            client.Port = 587;
            //client.UseDefaultCredentials = false;
            //client.EnableSsl = false;
            //it is possible that the mailserver is down or we may have configuration issues, so we want to encapsulate our code in a try catch statement
            try
            {
                //attmpt to send email
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"We're sorry your request could not be sent at this time. Please try again later. <br/> Error Message:<br/> {ex.StackTrace}";
                return View(cvm);//returns the view WITH the entire message so that users can copy and paste it for later
            }
            //if all goes well, we will return the user to a view that confirms that their message has been sent.
            return View("EmailConfirmation", cvm);

        }//end action
    }

}

