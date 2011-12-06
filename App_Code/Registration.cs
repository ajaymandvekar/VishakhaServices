/*
 * Copyright 2011 
 * Ajay Mandvekar(ajaymandvekar@gmail.com),Mugdha Kolhatkar(himugdha@gmail.com),Vishakha Channapattan(vishakha.vc@gmail.com)
 * 
 * This file is part of VishakhaServices.
 * VishakhaServices is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * VishakhaServices is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with VishakhaServices.  If not, see <http://www.gnu.org/licenses/>.
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class Registration : System.Web.Services.WebService
{
    public Registration () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    // Service to validate username
    // Username must be an email address such that it should contain single '@' followed by '.'
    // which is followed by 3 or less characters. These are the default username validation
    // rules
    [WebMethod]
    public Boolean ValidateUsername(string username)
    {
        int i = 0;
        Boolean flag1 = false;
        Boolean flag2 = false;
        for (i = 0; i < username.Length; i++)
        {
            if (i == 0 && username[i] == '@')
            {
                break;
            }
            if (username[i] == '@')
            {
                flag1 = true;
                break;
            }
        }
        if (flag1)
        {
            i++;
            if (i < username.Length && username[i] == '.')
            {
                flag2 = false;
            }
            else
            {
                while (i < username.Length)
                {
                    if (username[i] == '@')
                        break;
                    if (username[i] == '.')
                    {
                        flag2 = true;
                        break;
                    }
                    i++;
                }
            }
            if (flag2)
            {
                int index_i = i;
                i++;
                Boolean flag3 = true;
                while (i < username.Length)
                {
                    if (username[i] == '@' || username[i] == '.')
                    {
                        flag3 = false;
                        break;
                    }
                    i++;
                }
                if (flag3)
                {
                    if ((((username.Length - 1) - index_i) <= 3) && ((username.Length - 1) - index_i) > 0)
                        return true;
                }
            }
        }

        return false;
    }


    // Service to validate password
    // Password string should contain atleast 1 capital letter, 1 number and minimum 8 characters
    [WebMethod]
    public Boolean ValidatePassword(string password)
    {
        Boolean cap = false;
        Boolean num = false;
        if (password.Length >= 8)
        {
            int i = 0;
            for (i = 0; i < password.Length; i++)
            {
                if (password[i] >= 'A' && password[i] <= 'Z')
                {
                    cap = true;
                    break;
                }
            }
            if (cap)
            {
                for (i = 0; i < password.Length; i++)
                {
                    if (password[i] >= '0' && password[i] <= '9')
                    {
                        num = true;
                        break;
                    }
                }
                if (num)
                    return true;
            
            }
        }
        return false;
    }

    // Service to validate credit card number
    // Based on length and the Luhn Algorithm.
    [WebMethod]
    public Boolean ValidateCreditCardNumber(string cardNumber)
    {
        int length = cardNumber.Length;

        if (length < 13)
            return false;
        int sum = 0;
        int offset = length % 2;
        byte[] digits = new System.Text.ASCIIEncoding().GetBytes(cardNumber);

        for (int i = 0; i < length; i++)
        {
            digits[i] -= 48;
            if (((i + offset) % 2) == 0)
                digits[i] *= 2;

            sum += (digits[i] > 9) ? digits[i] - 9 : digits[i];
        }
        return ((sum % 10) == 0);
    }


    [WebMethod]
    public Boolean ValidateAddress(string streetAddress, string cityName, string zipCode)
    {
        Boolean street_addr = (Regex.Match(streetAddress, @"^[0-9]+\s+([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+\s[a-zA-Z]+)$")).Success;
        Boolean city = (Regex.Match(cityName,@"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$")).Success;
        Boolean zip =  (Regex.Match(zipCode, @"^\d{5}$")).Success;
        return street_addr && city && zip;
    }

    [WebMethod]
    public Boolean ValidateContactNumber(string contactNumber)
    {
        return Regex.Match(contactNumber, @"^[1-9]\d{2}-[1-9]\d{2}-\d{4}$").Success;
    }

    [WebMethod]
    public Boolean CreateCustomerAccount(string username, string password, string cardNumber, string streetAddress, string cityName, string zipCode, string contactNumber)
    {

        localhost.Service serviceObj = new localhost.Service();
        List<String> fieldnames = new List<String>();
        fieldnames.Add("0");
        fieldnames.Add("1");
        fieldnames.Add("2");
        fieldnames.Add("3");
        fieldnames.Add("4");
        fieldnames.Add("5");
        fieldnames.Add("6");

        List<String> valueNames = new List<String>();
        valueNames.Add(username);
        valueNames.Add(password);
        valueNames.Add(cardNumber);
        valueNames.Add(streetAddress);
        valueNames.Add(cityName);
        valueNames.Add(zipCode);
        valueNames.Add(contactNumber);

        bool success = serviceObj.InsertData(11, 32, "CustomerServiceInstance", fieldnames.ToArray(), valueNames.ToArray());
        if (success)
            return true;
        else
            return false;
    }
    
}


    