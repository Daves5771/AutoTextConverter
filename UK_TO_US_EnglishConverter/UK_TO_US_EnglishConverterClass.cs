using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UK_TO_US_EnglishConverter
{
    public class UK_TO_US_EnglishConverterClass
    {
        public string Convert_tre_to_ter_Word(Match matchResult)
        {
            string treString, terString;
            string ukWord = matchResult.Groups[1].ToString();
            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                treString = "TRE";
                terString = "TER";
            }
            else
            {
                treString = "tre";
                terString = "ter";
            }

            int treIndex = (ukWord).LastIndexOf(treString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(treString,terString,treIndex,treString.Length);
            return sb.ToString();
        }

        //our to or
        public string Convert_our_to_or_Word(Match matchResult)
        {
            string ourString, orString;
            string ukWord = matchResult.Groups[1].ToString();

            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                ourString = "OUR";
                orString = "OR";
            }
            else
            {
                ourString = "our";
                orString = "or";
            }

            int ourIndex = (ukWord).LastIndexOf(ourString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(ourString, orString, ourIndex, ourString.Length);
            return sb.ToString();
        }

        public bool Validate_OUR_WORD(Match matchResult)
        {
            // http://www.morewords.com/contains/our/
            // check with word table
            string candidate = matchResult.Groups[0].ToString().ToLower();

            // colour, flavour, behaviour, harbour, honour, humour, labour, neighbour, neighbour, splendour
            if (candidate.Contains("colour") || candidate.Contains("flavour") || candidate.Contains("behaviour") ||
                candidate.Contains("harbour") || candidate.Contains("honour") || candidate.Contains("humour") ||
                candidate.Contains("labour") || candidate.Contains("neighbour") || candidate.Contains("splendour") ||
                candidate.Contains("clamour") || candidate.Contains("enamour") || candidate.Contains("endeavour") ||
                candidate.Contains("favour")  || candidate.Contains("harbour") ||
                candidate.Contains("rancour") || candidate.Contains("odour") || candidate.Contains("parlour") ||
                candidate.Contains("rigour") || candidate.Contains("rumour") || candidate.Contains("saviour") ||
                candidate.Contains("tumour") || candidate.Contains("valour") || candidate.Contains("vapour"))

                return true;

            return false;
        }

        //isations
        public string Convert_is_to_iz_Word(Match matchResult)
        {
            string treString, terString;
            string ukWord = matchResult.Groups[1].ToString();
            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                treString = "IS";
                terString = "IZ";
            }
            else
            {
                treString = "is";
                terString = "iz";
            }

            int treIndex = (ukWord).LastIndexOf(treString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(treString, terString, treIndex, treString.Length);
            return sb.ToString();
         }

        public string Convert_ise_to_ize_Word(Match matchResult)
        {
            string treString, terString;
            string ukWord = matchResult.Groups[1].ToString();
            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                treString = "ISE";
                terString = "IZE";
            }
            else
            {
                treString = "ise";
                terString = "ize";
            }

            int treIndex = (ukWord).LastIndexOf(treString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(treString, terString, treIndex, treString.Length);
            return sb.ToString();
        }

        public string Convert_isation_to_ization_Word(Match matchResult)
        {
            string treString, terString;
            string ukWord = matchResult.Groups[1].ToString();
            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                treString = "ISATION";
                terString = "IZATION";
            }
            else
            {
                treString = "isation";
                terString = "ization";
            }

            int treIndex = (ukWord).LastIndexOf(treString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(treString, terString, treIndex, treString.Length);
            return sb.ToString();
        }
        public string Convert_amme_to_am_Word(Match matchResult)
        {
            string ammeString, amString;
            int count = matchResult.Groups.Count;
            if (count <1) 
                throw new Exception("match count = 0");

            string ukWord = matchResult.Groups[count-1].ToString();

            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                ammeString = "AMME";
                amString = "AM";
            }
            else
            {
                ammeString = "amme";
                amString = "am";
            }

            int ammeIndex = (ukWord).LastIndexOf(ammeString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(ammeString, amString, ammeIndex, ammeString.Length);
            return sb.ToString();
        }

        public string Convert_logue_to_log_Word(Match matchResult)
        {
            string logueString, amString;
            int count = matchResult.Groups.Count;
            if (count < 1)
                throw new Exception("match count = 0");

            string ukWord = matchResult.Groups[count - 1].ToString();

            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                logueString = "LOGUE";
                amString = "LOG";
            }
            else
            {
                logueString = "logue";
                amString = "log";
            }

            int ammeIndex = (ukWord).LastIndexOf(logueString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(logueString, amString, ammeIndex, logueString.Length);
            return sb.ToString();
        }

        public string Convert_ence_to_ense_Word(Match matchResult)
        {
            string enceString, enseString;
            string ukWord = matchResult.Groups[1].ToString();
            if (Char.IsUpper(ukWord[ukWord.Length - 1]))
            {
                enceString = "ENCE";
                enseString = "ENSE";
            }
            else
            {
                enceString = "ence";
                enseString = "ense";
            }

            int enceIndex = (ukWord).LastIndexOf(enceString);
            StringBuilder sb = new StringBuilder(ukWord);
            sb.Replace(enceString, enseString, enceIndex, enseString.Length);
            return sb.ToString();
        }
        public string Convert_UK_DATE_to_US_DATE(Match matchResult)
        {
            string month = "";
            int day = 0;
            int year = 0;
            GetDateNumberGroups(matchResult, out  month, out  day, out  year);
            if (year == 0)
                return month + " " + day.ToString();
            else
                return month + " " + day.ToString() + ", " + year.ToString();

        }

        private void GetDateNumberGroups(Match matchResult, out string month, out int day, out int year)
        {
            string[] Months = {"January", "February", "March","April", "May", "June",
        "July", "August", "September", "October", "November", "December"};
            string monthTestStr = "";
          bool validateDate = false;
            day = 0;
            year = 0;
            month = "";
          //  if (matchResult.Groups.Count < 2)
          //      return false;
            int nResult = 0;
   //         monthTestStr = matchResult.Groups[2].ToString();
  //          monthTestStr = matchResult.Groups[3].ToString();
            int j = 0;
            // find day
            for (int i = 0; i < matchResult.Groups.Count - 2; i++)
            {
                if (matchResult.Groups[i].Length < 1)
                    continue;
                if (Char.IsDigit(matchResult.Groups[i].Value[0]))
                {
                    j = i;
                    if (int.TryParse(matchResult.Groups[i].Value, out nResult))
                        day = nResult;
                }
            }
            

            // find date
            for (int i = j+1 ; i < matchResult.Groups.Count; i++)
            {
                if (matchResult.Groups[i].Length < 1)
                    continue;
                if (Char.IsLetter(matchResult.Groups[i].Value[0]))
                {
                    monthTestStr = matchResult.Groups[i].Value;
                    j = i;
                    break;
                }
            }

            foreach (string month_str in Months)
            {
                if (month_str == monthTestStr)
                {
                    validateDate = true;
                    month = month_str;
                    break;
                }
            }


          for (int i = j+1; i < matchResult.Groups.Count; i++)
            {
                if (matchResult.Groups[i].Length < 1)
                    continue;
                if (int.TryParse(matchResult.Groups[i].Value, out nResult))
                    year = nResult;
              break;

            /*    if (Char.IsDigit(matchResult.Groups[i].Value[0]))
                {
                    j = i;
                    if (int.TryParse(matchResult.Groups[i].Value, out nResult))
                        year = nResult;
                }*/
            }
          if (month == "August" && day == 14)
          {
              int x = 9;
                  x++;
          }
        }
    }
}
