using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// SaelSoft -- SSN_ValidatorClass.cs
// Purpose -- Validates US Social Security Numbers:
// 2008 David Saelman
namespace SaelSoft.RegExPlugIn.SSN_Validator
{
    public class SSN_ValidatorClass
    {
        public bool ValidateSSN(Match matchResult)
        {
            if (matchResult.Groups.Count < 5)
                return false;
            int nResult = 0;

            int group1 = 0;
            if (int.TryParse(matchResult.Groups[1].ToString(), out nResult))
                group1 = nResult;
            else
                return false;
            int group2 = 0;
            if (int.TryParse(matchResult.Groups[3].ToString(), out nResult))
                group2 = nResult;
            else
                return false;

            int group3 = 0;
            if (int.TryParse(matchResult.Groups[4].ToString(), out nResult))
                group3 = nResult;
            else
                return false;

            // every group must be nonzero
            if (group1 == 0 || group2 == 0 || group3 == 0)
                return false;
 
            else
                return true; // Valid ssn
        }

        public string GetSSNMatch(Match matchResult)
        {
            return matchResult.Groups[1].ToString() + "-" + matchResult.Groups[3].ToString() +
                "-" + matchResult.Groups[4].ToString();

        }
    }
}
