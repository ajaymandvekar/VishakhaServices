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
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Shipment : System.Web.Services.WebService {

    public Shipment () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public Boolean validateShippingAddress(string streetAddress, string cityName, string zipCode) {
        Boolean street_addr = (Regex.Match(streetAddress, @"^[0-9]+\s+([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+\s[a-zA-Z]+)$")).Success;
        Boolean city = (Regex.Match(cityName, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$")).Success;
        Boolean zip = (Regex.Match(zipCode, @"^\d{5}$")).Success;
        return street_addr && city && zip;
    }

    [WebMethod]
    public double computeShippingCost(int shippingOption)
    {
        double shipping_cost = 0.0;
        switch (shippingOption)
        {
            case 1:
                shipping_cost = 50.0;
                break;

            case 2:
                shipping_cost = 100.0;
                break;

            case 3:
                shipping_cost = 200.0;
                break;
            
            default:
                shipping_cost = 150.0;
                break;
        }
        return shipping_cost;
    }

    [WebMethod]
    public double computeShippingCostForGift(int shippingOption)
    {
        double gift_package = 50;
        double shipping_cost = computeShippingCost(shippingOption);
        shipping_cost += gift_package;
        return shipping_cost;
    }

    [WebMethod]
    public string billToCard(string cardNumber, double shippingCost)
    {
        return "Payment of "+shippingCost+"$ is being processed";
    }

    // Service to validate credit card number
    // Based on length and the Luhn Algorithm.
    [WebMethod]
    public string ValidateCardNumberAndBillToCard(string cardNumber, double shippingCost)
    {
        int length = cardNumber.Length;

        if (length < 13)
            return "Invalid credit card number";
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
        if ((sum % 10) == 0)
            return "Payment of " + shippingCost + "$ is being processed";
        else
            return "Error processing payment";
    }

    [WebMethod]
    public string DisplayConfirmation(string processingResult)
    {
        return "Notification message : " + processingResult;
    }
}
