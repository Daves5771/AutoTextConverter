using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// SaelSoft -- NumericalDateValidatorClass.cs
// Purpose -- Validates dates in the form of:
// US Date Format:      (month) mm/ (day) dd/ (year) yyyy  or
// Euroean Date Format: (day) dd/ (month) mm/ (year) yyyy)
// 2008 David Saelman

namespace SaelSoft.RegExPlugIn.NumericalDateValidator
{
    public class NumericalDateValidatorClass
    {
        int month = 0;
        int day = 0;
        int year = 0;
        string[] Months = {"January", "February", "March","April", "May", "June",
        "July", "August", "September", "October", "November", "December"};
        public bool ValidateUSDate(Match matchResult)
        {
            if (matchResult.Groups.Count < 3)
                return false;
            int nResult = 0;

            if (int.TryParse(matchResult.Groups[1].ToString(), out nResult))
                month = nResult;
            else
                return false;
            if (int.TryParse(matchResult.Groups[2].ToString(), out nResult))
                day = nResult;
            else
                return false;

            if (int.TryParse(matchResult.Groups[3].ToString(), out nResult))
                year = nResult;
            else
                return false;

            return CommonDateValidation();
        }
        private bool GetDateNumberGroups(Match matchResult, out int month, out int day, out int year)
        {
            month = 0;
            day = 0;
            year = 0;

            if (matchResult.Groups.Count < 3)
                return false;
            int nResult = 0;

            if (int.TryParse(matchResult.Groups[1].ToString(), out nResult))
                month = nResult;
            else
                return false;
            if (int.TryParse(matchResult.Groups[2].ToString(), out nResult))
                day = nResult;
            else
                return false;

            if (int.TryParse(matchResult.Groups[3].ToString(), out nResult))
                year = nResult;
            return true;

        }

        public string GetDateMatch_1(Match matchResult)
        {
            int month = 0;
            int day = 0;
            int year = 0;
            GetDateNumberGroups( matchResult, out  month, out  day, out  year);
            return Months[month -1] + " "+ day.ToString() + ", " + year.ToString();
        }

        public bool ValidateEuropeanDate(Match matchResult)
        {
            if (matchResult.Groups.Count < 3)
                return false;
            int nResult = 0;

            if (int.TryParse(matchResult.Groups[1].ToString(), out nResult))
                month = nResult;
            else
                return false;
            if (int.TryParse(matchResult.Groups[2].ToString(), out nResult))
                day = nResult;
            else
                return false;

            if (int.TryParse(matchResult.Groups[3].ToString(), out nResult))
                year = nResult;
            else
                return false;

            return CommonDateValidation();
        }

        private bool CommonDateValidation()
        {
            // verify that all 30 day months do not contain 31 days eg 4/31/2007
            if (day == 31 && (month == 4 || month == 6 || month == 9 || month == 11))
            {
                return false; // 31st of a month with 30 days
            }
            // February, a special case cannot contain 30 or more days
            else if (day >= 30 && month == 2)
            {
                return false; //  checFebruary 30th or 31st
            }
            // check for February 29 outside a leap year
            else if (month == 2 && day == 29 && !(year % 4 == 0 && (year % 100 != 0 || year % 400 == 0)))
            {
                return false; 
            }
            else
            {
                return true; // Valid date
            }
        }

    }
}
