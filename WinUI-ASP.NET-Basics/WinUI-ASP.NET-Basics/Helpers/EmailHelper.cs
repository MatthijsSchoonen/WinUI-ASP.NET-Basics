using MimeKit;


namespace WinUI_ASP.NET_Basics.Helpers
    {
        class EmailHelper
        {
            private const string SenderEmail = "interactivedreamscape@gmail.com";
            private const string SenderName = "DreamScape Interactive";
            private const string SmtpServer = "smtp.gmail.com";
            private const int SmtpPort = 587;
            private const string SmtpPassword = "scvf niad qiea tnwj"; //not secure but for testing purposes only

            public async Task ConfirmAccountCreation(string recipientEmail)
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(SenderName, SenderEmail));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = "Account Creation";

                // Build email body
                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = @"
                    <html>
                        <body>
                            <h1>Welcome to Pizzaria!</h1>
                            <p>Thank you for creating an account on Pizzaria</p>                 
                        </body>
                    </html>",
                    TextBody = "Thank you for creating an account on Pizzaria"
                };

                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
            }
          

            
            public async Task SendEmailAsync(string recipientEmail, string subject, string body)
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(SenderName, SenderEmail));
                message.To.Add(new MailboxAddress("", recipientEmail));
                message.Subject = subject;

                // Build email body
                BodyBuilder bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <html>
                            <body>
                                <p>{body}</p>
                            </body>
                        </html>",
                    TextBody = body
                };

                message.Body = bodyBuilder.ToMessageBody();

                await SendEmailAsync(message);
            }


            private async Task SendEmailAsync(MimeMessage message)
            {
                try
                {
                    using (var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await client.ConnectAsync(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync(SenderEmail, SmtpPassword);
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                    }

                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw;
                }
            }

            public bool IsValidEmail(string email)
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == email;
                }
                catch
                {
                    return false;
                }
            }
        }
    }


