using ContactManagerApi.Models;
using ContactManagerApi.Models.DTO.Response;
using ContactManagerApi.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace ContactManagerApi.Utils
{
    public class ValidateContact
    {
       
        public void ValiateContact(Contact contact, UserMangerContext context)
        {
            if (ContactNameContainNumber(contact.Firstname)) throw new ContactoNombreException("El nombre no puede iniciar con numero", contact.Firstname);
            if (ContactIsMenorTo18Age(contact.DateOfBirth)) throw new ContactAgeException("El contacto tiene menos de 18 annos");
            if (EmailContactExist(contact.Email, context)) throw new EmailAdreesIsAlreadyException($"El correo {contact.Email} ya existe.");
        }

        public bool ContactNameContainNumber(string firstname) {

            return Regex.IsMatch(firstname, @"^\d");
        }

        public bool ContactIsMenorTo18Age(DateTime dateOfBirth)
        {
            int edad = DateTime.Now.Year - dateOfBirth.Year;
            if (edad < 18)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool EmailContactExist(string email, UserMangerContext context) {

            if (context.Contacts.Any(x => x.Email.Equals(email)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}
