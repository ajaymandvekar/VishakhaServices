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
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Login : System.Web.Services.WebService {

   public Login () {

        //Uncomment the following line if using designed components 
        //InitializeComponent();
    }

    [WebMethod]
    public Boolean authenticateUserNamePassword(string username, string password) {
        string uname = null, pword = null;
        localhost.Service serviceObj = new localhost.Service();
        localhost.TenantTableInfo obj = serviceObj.ReadData(11, 32);
        string[] arr = obj.FieldNamesProperty;
        List<string> array = new List<string>(arr);
        string[] values = obj.FieldValuesProperty;
        List<string> valuearr = new List<string>(values);
        int countRow = valuearr.Count / array.Count;
        int counter = 0;
        for (int i = 0; i < countRow; i++)
        {
            for (int j = 0; j < array.Count; j++)
            {
                if (arr[j].ToString() == "password")
                {
                    pword = valuearr[counter].ToString();
                    if (username == uname && pword == password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                if (arr[j].ToString() == "username")
                {
                    uname = valuearr[counter].ToString();
                }
                counter++;
            }
        }
        return false;
    }

    [WebMethod]
    public string notifyUserAboutAuthentication(Boolean notifyType)
    {
        string result = "";
        if (notifyType == true)
            result = "You will be directed to login page shortly";
        else
            result = "Incorrect username/password";
        return result;
    }

    [WebMethod]
    public Boolean authenticateUsernamePasswordWithSiteKey(string username,string password, Boolean isSiteKey)
    {
        return (authenticateUserNamePassword(username, password) && verifySiteKey(isSiteKey));
    }

    public Boolean verifySiteKey(Boolean isSitekey)
    {
        if(isSitekey)
            return true;
        return false;
    }

   
}
