using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XF.Atividade3.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserRepository
    {
        private UserRepository() { }

        private static readonly UserRepository instance = new UserRepository();
        public static UserRepository Instance {
            get{return instance;}
        }

        public static bool IsAutorizado(User paramLogin) {
            XElement xmlUsuarios = XElement.Parse(App.UserVM.Stream);
            var users = new List<User>();
            foreach (var item in xmlUsuarios.Elements("usuario"))
            {
                User user = new User()
                {
                    Username = item.Element("username").Value,
                    Password = item.Element("password").Value
                };
                users.Add(user);
            }
            return users.Any(user => user.Username == paramLogin.Username && user.Password == paramLogin.Password);
        }
    }

}
