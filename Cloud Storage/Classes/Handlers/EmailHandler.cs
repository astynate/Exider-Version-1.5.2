﻿using System.Net.Mail;
using System.Net;

static class EmailHandler
{
    public static void Send(string email, string template)
    {

        var corporate_email = "zixe.company@gmail.com";
        var corporate_password = "ybhbhvhnjvixhehd";

        string input_html = template;

        MailAddress sender = new MailAddress(email, "ZIXE COMPANY");
        MailAddress recipient = new MailAddress(email);
        MailMessage mail = new MailMessage(sender, recipient);

        mail.Subject = "Please confirm your email address";
        mail.Body = input_html;
        mail.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

        smtp.Credentials = new NetworkCredential(corporate_email, corporate_password);
        smtp.EnableSsl = true;
        smtp.Send(mail);

    }
    public static void SendEmailConfirmation(string email, string name, string code)
    {

        string output_html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">\r\n    <link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>\r\n    <link rel=\"stylesheet\" href=\"1.css\">\r\n    <link href=\"https://fonts.googleapis.com/css2?family=Lato:ital,wght@0,100;0,300;0,400;0,700;0,900;1,100;1,300;1,400;1,700&family=Open+Sans:ital,wght@0,300;0,400;0,500;0,600;0,700;0,800;1,300;1,400;1,500;1,600;1,700;1,800&display=swap\" rel=\"stylesheet\">\r\n\r\n    <style>\r\n\r\n        * {{\r\n            margin: 0;\r\n            padding: 0;\r\n            box-sizing: border-box;\r\n        }}\r\n\r\n        body {{\r\n            font-family: 'Open Sans', sans-serif;\r\n            color: black;\r\n        }}\r\n\r\n        h1 {{\r\n            margin-left: 20px;\r\n            margin-right: 20px;\r\n            margin-top: 30px;\r\n            font-size: 19px;\r\n        }}\r\n\r\n        h2 {{\r\n            display: flex;\r\n            margin-top: 20px;\r\n            margin-left: 20px;\r\n            margin-right: 20px;\r\n            font-weight: 400;\r\n            color: rgb(90, 90, 90);\r\n        }}\r\n\r\n        span {{\r\n            margin-left: 10px;\r\n            border: none;\r\n            box-shadow: 1px 1px 10px #12121820, -1px -1px 10px #fff;\r\n            background: rgb(252, 253, 255);\r\n            border-radius: 15px;\r\n            padding: 15px;\r\n        }}\r\n\r\n            span:nth-child(1) {{\r\n                margin-left: 0;\r\n            }}\r\n\r\n        p {{\r\n            font-family: 'Open Sans', sans-serif;\r\n            margin-left: 20px;\r\n            margin-right: 20px;\r\n            font-size: 18px;\r\n            text-align: justify;\r\n            margin-top: 8px;\r\n        }}\r\n\r\n        .Main-Wrapper {{\r\n            display: block;\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n            background: rgb(247, 247, 247);\r\n            background: rgb(240, 240, 241);\r\n            height: auto;\r\n            width: 700px;\r\n            border-radius: 20px;\r\n            margin-top: 30px;\r\n            padding: 4px;\r\n            padding-bottom: 50px;\r\n        }}\r\n\r\n        .Main-Wrapper-Header {{\r\n            height: auto;\r\n            width: auto;\r\n            background: white;\r\n            border-radius: 20px;\r\n        }}\r\n\r\n            .Main-Wrapper-Header img {{\r\n                display: block;\r\n                margin-left: auto;\r\n                margin-right: auto;\r\n                height: 70px;\r\n            }}\r\n\r\n        footer {{\r\n            display: block;\r\n            margin-left: auto;\r\n            margin-right: auto;\r\n            width: 700px;\r\n            margin-top: 10px;\r\n            margin-bottom: 10px;\r\n        }}\r\n\r\n            footer p {{\r\n                display: block;\r\n                margin-left: auto;\r\n                margin-right: auto;\r\n                text-align: center;\r\n            }}\r\n\r\n    </style>\r\n\r\n  </head>\r\n<body>\r\n  <div class=\"Main-Wrapper\">\r\n    <div class=\"Main-Wrapper-Header\">\r\n      <img draggable=\"false\" src=\"https://lh3.googleusercontent.com/pw/AIL4fc9hbpBR59WX0qr4h-HEWgv8UhUYBBHNNiWTiEvNFRLZ1WkJhNSmT1UJoduFqS5evA4GJslLtGk6_yBswfPDnEtNLBwlaAGZzvc5Qz131GrPhPuFFKsz2E8ZkV-BTXsv-AXGN6bKUi7uQ8SuE8EEetdP=w955-h955-s-no?authuser=0\" alt=\"logo\">\r\n    </div>\r\n    <h1>Подтверждение электронной почты</h1>\r\n    <p>\r\n      Здравствуйте, {name}. Мы благодарны Вам за решение зарегистрировать у нас аккаунт. Для завершения регистрации, пожалуйста, подтвердите свой адрес электронной почты. Надеемся, что вы прочитали пользовательское соглашение и согласны с условиями регистрации. Чтобы заполнить адрес электронной почты, введите этот код в регистрационную форму.</p>\r\n    <h2><span>{code[0]}</span><span> {code[1]} </span><span>{code[2]}</span><span>{code[3]}</span><span>{code[4]}</span><span>{code[5]}</span></h2>\r\n    <h1>Пользовательское соглашение</h1>\r\n    <p>Добро пожаловать в наше облачное хранилище! Перед использованием нашей платформы, пожалуйста, внимательно ознакомьтесь с настоящим пользовательским соглашением.\r\n\r\n      Регистрация и аккаунт <br>1.1. Для использования нашего облачного хранилища, необходимо создать аккаунт. Вы несете полную ответственность за сохранность своего аккаунта и пароля. Не делитесь своими учетными данными с третьими лицами. <br>1.2. Вы соглашаетесь предоставить верные и актуальные личные данные при регистрации.\r\n      \r\n      Использование хранилища <br>2.1. Вы обязуетесь использовать наше облачное хранилище только в законных целях и соблюдать применимое законодательство. <br>2.2. Запрещено хранить, загружать или распространять незаконный контент, включая, но не ограничиваясь, материалами, нарушающими авторские права, порнографическими материалами или материалами, распространяющими ненависть и насилие.\r\n      \r\n      Ответственность и ограничения <br>3.1. Мы не несем ответственности за любые убытки, прямые или косвенные, возникающие из использования нашего облачного хранилища. <br>3.2. Мы оставляем за собой право приостановить или прекратить предоставление услуги без предварительного уведомления, если вы нарушаете настоящее пользовательское соглашение.\r\n      \r\n      Изменения в пользовательском соглашении <br>4.1. Мы оставляем за собой право вносить изменения в настоящее пользовательское соглашение. Изменения вступают в силу с момента публикации на нашем веб-сайте.\r\n      \r\n      Пожалуйста, помните, что использование нашего облачного хранилища подразумевает ваше согласие с данным пользовательским соглашением. Если у вас возникли вопросы или несогласия, пожалуйста, обратитесь к нашей службе поддержки.\r\n      \r\n      Спасибо, что выбрали наше облачное хранилище!</p>\r\n  </div>\r\n  <footer>\r\n    <p>&#169; Andreev Svyatoslav, 2023</p>\r\n  </footer>\r\n</body>\r\n</html>\r\n";
        Send(email, output_html);

    }
}