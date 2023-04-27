using ContactManagerApi.Models;
using ContactManagerApi.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ContactManagerApi.Utils
{
    public class ValidateUser
    {
        public void ValidateDataUser(User user, UserMangerContext context)
        {
            if (IsUserNameExist(user.Username, context)) throw new UserIsAlreadyException("El uaurio ya existe.");
            if (ContentInvalidCharacter(user.Username)) throw new InvalidChracaterUserName("El nombre del usuario no pude contener caracteres invalidos");
        }

        public bool IsUserNameExist(string username, UserMangerContext context)
            => context.Users.Any(u => u.Username.Equals(username));

        public bool ContentInvalidCharacter(string username)
        {
            return (username.Contains("@") || username.Contains(":") || username.Contains(";")) ? true : false;
        }
    }
}
