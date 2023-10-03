using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Context;
using api.ViewModel;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace api.Repositories
{
    public class AccountRepository
    {
        private readonly MyContext context;

        public AccountRepository(MyContext context) {
            this.context = context;
        }

        public bool Login(string email, string password) {
            var data = context.Employees.FirstOrDefault(employee => employee.Email == email);
            var account = context.Accounts.Single(account => account.NIK == data.NIK);

            bool isValid = BCrypt.Net.BCrypt.EnhancedVerify(password, account.Password);
            return isValid;
        }

        public bool ForgetPassword(string email) {
            // Cek email ada di database
            var data = context.Employees.FirstOrDefault(employee => employee.Email == email);
            if(data == null) {
                return false;
            }

            // Generate OTP
            Random random = new Random();
            int length = 6; // Panjang string angka
            string OTP = new string(Enumerable.Repeat("0123456789", length).Select(s => s[random.Next(s.Length)]).ToArray());
            
            var account = context.Accounts.Find(data.NIK);
            account.OTP = OTP;
            context.SaveChanges();

            // Kirim email
            SendEmail(email, OTP);
            return true;
        }

        public void SendEmail(string Email, string OTP)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress("Rosa Amalia", "amaliarosaaa15@gmail.com"));
            email.To.Add(new MailboxAddress("Penerima", Email));

            email.Subject = "Kode OTP Lupa Password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = $"Berikut adalah kode OTP Anda {OTP}."
            };
            using (var smtp = new SmtpClient())
            {
                //smtp.CheckCertificateRevocation = false;
                smtp.Connect("smtp.gmail.com", 465 , true);

                // Note: only needed if the SMTP server requires authentication
                //smtp.Authenticate("a14201852661a9", "ecc3f7e0ba79fe");
                smtp.Authenticate("amaliarosaaa15@gmail.com", "modp gvir sugh ajes");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}