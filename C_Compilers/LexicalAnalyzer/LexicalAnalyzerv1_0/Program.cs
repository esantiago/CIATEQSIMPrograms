using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LexicalAnalyzerv1_0
{

    class Program : Constants
    {

        private static string[] LiteralsArray = new string[100];
        private static string[] LiteralsArrayPosition = new string[100];
        private static int LiteralsArrayCounter = 0;

        private static string[] KeywordsArray = new string[100];
        private static string[] KeywordsArrayPosition = new string[100];
        private static int KeywordsArrayCounter = 0;

        private static string[] PunctuationsArray = new string[100];
        private static string[] PunctuationsArrayPosition = new string[100];
        private static int PunctuationsArrayCounter = 0;

        private static string[] WhitespacesArray = new string[100];
        private static string[] WhitespacesArrayPosition = new string[100];
        private static int WhitespacesArrayCounter = 0;

        private static string[] ComparisonsArray = new string[100];
        private static string[] ComparisonsArrayPosition = new string[100];
        private static int ComparisonsArrayCounter = 0;

        private static string[] NumbersArray = new string[100];
        private static string[] NumbersArrayPosition = new string[100];
        private static int NumbersArrayCounter = 0;

        private static string[] IdentifiersArray = new string[100];
        private static string[] IdentifiersArrayPosition = new string[100];
        private static int IdentifiersArrayCounter = 0;

        private static string[] UnknownsArray = new string[100];
        private static string[] UnknownsArrayPosition = new string[100];
        private static int UnknownsArrayCounter = 0;

        private static bool GetStringBetweenChar(int startIndex, string inputString, out string detectedString, out int detectedCol)
        {
            int coincidenceIndex1 = 0;
            int coincidenceIndex2 = 0;
            string outputString = "";
            bool Found = false;

            detectedString = "";
            detectedCol = 0;

            coincidenceIndex1 = inputString.IndexOf('\"', startIndex);

            if (coincidenceIndex1 != -1)   //check if string contains " opening char 
            {
                coincidenceIndex2 = coincidenceIndex1 + 1 + inputString.Substring(coincidenceIndex1 + 1, inputString.Length - (coincidenceIndex1 + 1)).IndexOf('\"', startIndex);

                if (coincidenceIndex2 != -1)   //check if string contains " closing char
                {
                    if (coincidenceIndex2 > coincidenceIndex1)
                    {
                        detectedString = inputString.Substring(coincidenceIndex1 + 1, coincidenceIndex2 - (coincidenceIndex1 + 1));
                        detectedCol = coincidenceIndex1;
                        return true;
                    }

                }
            }

            return false;
        }

        private static bool ReplaceStrings(ArrayList stringIndexes, ArrayList stringLiterals, string line, out string outputLine)
        {
            int col = 1;
            string detectedString = "";
            int detectedCol = 0;
            outputLine = "";
            var theString = "";


            for (col = 0; col < line.Length;)
            {
                if (GetStringBetweenChar(col, line, out detectedString, out detectedCol))  //look for strings within line and gets string and column position if detected
                {
                    stringIndexes.Add(detectedCol+1); //+1 because base 1 on file analisis for row and column
                    stringLiterals.Add(detectedString);
                    col += (detectedCol + detectedString.Length)+2; //update search position for next serching (+2 for skipping '"'opnening and closing chars )

                    //replace string with any char (s for example)
                    string newString = new string('s', detectedString.Length);

                    theString = line;
                    var aStringBuilder = new StringBuilder(theString);
                    aStringBuilder.Remove(detectedCol+1, detectedString.Length);  //+1 for letting " start char
                    aStringBuilder.Insert(detectedCol+1, newString); //+1 for letting " start char
                    theString = aStringBuilder.ToString();
                    line = theString;

                }
                else
                    col++;
            }

            if (stringIndexes.Count > 0)  //at least one string was found
            {
                outputLine = line;
                return true;
            }

            return false;
        }


        static void Main(string[] args)
        {

            //string[] lines = { " x= 2.54-3 ; printf(\"sssssssssssss\",PI)" };//File.ReadAllLines(Constants.SOURCE_FILE);
            //string[] lines = { "printf(\"abcd\",PI);  printf(\"this is string2\",PI); " };//File.ReadAllLines(Constants.SOURCE_FILE);
            string[] lines = File.ReadAllLines(Constants.SOURCE_FILE);
            string[] splittedLine;
            int detectedToken = 0;


            int row = 1, col = 1;
            int breakpoint = 0;

            foreach (string l in lines) //analize each line of file
            {
                //if (row == 33)
                 //   breakpoint++;

                //first step is to do string replacement (excluding strings from lexical analisis)
                string line = l;
                ArrayList stringsIndexes = new ArrayList();
                ArrayList stringsLiterals = new ArrayList();
                int stringsCounter = 0;
                string outputLine = "";
                col = 1;


                /*
                if (line == "") //means \r\n found on this line
                {
                    AddLexemeToWhitespaces("\r\n", row, col);
                    detectedToken = (int)Constants.tokensIDs.WHITESPACE;
                    row++;
                    continue;
                }
                */

                if (ReplaceStrings(stringsIndexes, stringsLiterals, line, out outputLine))  //[0] = 10 means string 0 was found on position 10 of line, similarly  [1] = 12 means string 1 was found on position 12 of line
                {
                    stringsCounter = 0;
                    line = outputLine; //update line after strings replacement
                }

                splittedLine = Regex.Split(line, @"([(),;+\-*=&{}\t\r\n ])");

                foreach (string lexeme in splittedLine)
                {
                    if (lexeme == "")
                        continue;

                    if ( (stringsIndexes.Count > 0) && (stringsCounter < stringsIndexes.Count) && (col == (int)stringsIndexes[stringsCounter]))  //any string detected on line (all strings of line were previously detected)
                    {
                        //if (col == (int)stringsIndexes[stringsCounter]) //this lexeme is a string?
                        {
                            AddLexemeToLiterals((string)stringsLiterals[stringsCounter], row, col);
                            stringsCounter++;
                            detectedToken = (int)Constants.tokensIDs.LITERAL;
                        }
                    }
                    else if (isLexemeKeyword(lexeme))  //is a keyword?
                    {
                        AddLexemeToKeywords(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.KEYWORD;
                    }
                    else if (isLexemePunctuation(lexeme))  //is a punctuation?
                    {
                        AddLexemeToPunctuations(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.PUNCTUATION;
                    }
                    else if (isLexemeWhitespace(lexeme))  //is a whitespace?
                    {
                        AddLexemeToWhitespaces(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.WHITESPACE;
                    }
                    else if (isLexemeComparison(lexeme))  //is a comparison?
                    {
                        AddLexemeToComparisons(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.COMPARISON;
                    }
                    else if (isLexemeNumber(lexeme)) //is a number?
                    {
                        AddLexemeToNumbers(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.NUMBER;
                    }
                    else if (isLexemeIdentifier(lexeme)) //is a identifier
                    {
                        AddLexemeToIdentifiers(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.IDENTIFIER;
                    }
                    else //unknown token
                    {
                        AddLexemeToUnknowns(lexeme, row, col);
                        detectedToken = (int)Constants.tokensIDs.UNKNOWN;
                    }


                    col += lexeme.Length;
                    
                }

                row++;
            }

            PrintSymbolsTable();

            Console.ReadKey();
        }


        private static bool isLexemeKeyword(string lexeme)
        {
            foreach (string keyword in Constants.keywordReferenceArray) //compare against all keywords references
            {

                if (lexeme == keyword)
                    return true;
            }

            return false;
        }

        private static bool isLexemePunctuation(string lexeme)
        {
            foreach (string punctuation in Constants.punctuationReferenceArray) //compare against all keywords references
            {

                if (lexeme == punctuation)
                    return true;
            }

            return false;
        }

        private static bool isLexemeWhitespace(string lexeme)
        {
            foreach (string whitespace in Constants.whitespaceReferenceArray) //compare against all whitespaces references
            {

                if (lexeme == whitespace)
                    return true;
            }

            return false;
        }
        private static bool isLexemeComparison(string lexeme)
        {
            foreach (string comparison in Constants.comparisonReferenceArray) //compare against all whitespaces references
            {

                if (lexeme == comparison)
                    return true;
            }

            return false;
        }

        private static bool isLexemeNumber(string lexeme)
        {
            float result;
            return float.TryParse(lexeme, out result);
        }

        private static bool isLexemeIdentifier(string lexeme)
        {
            char firstCharacter = lexeme.ToCharArray().ElementAt(0);

            if (firstCharacter == '_') 
                return true;

            return char.IsLetter(firstCharacter);
        }

        private static void AddLexemeToLiterals(string lexeme, int row, int col)
        {
            LiteralsArray[LiteralsArrayCounter] = lexeme;
            LiteralsArrayPosition[LiteralsArrayCounter] = row.ToString() + "," + col.ToString();
            LiteralsArrayCounter++;
        }

        private static void AddLexemeToKeywords(string lexeme, int row, int col)
        {
            KeywordsArray[KeywordsArrayCounter] = lexeme;
            KeywordsArrayPosition[KeywordsArrayCounter] = row.ToString() + "," + col.ToString();
            KeywordsArrayCounter++;
        }

        private static void AddLexemeToPunctuations(string lexeme, int row, int col)
        {
            PunctuationsArray[PunctuationsArrayCounter] = lexeme;
            PunctuationsArrayPosition[PunctuationsArrayCounter] = row.ToString() + "," + col.ToString();
            PunctuationsArrayCounter++;
        }

        private static void AddLexemeToWhitespaces(string lexeme, int row, int col)
        {
            if (lexeme == "\t")
                lexeme = "'\\t'";
            else if (lexeme == "\r\n")
                lexeme = "'\\r\\n'";
            else if (lexeme == " ")
                lexeme = " ' ' ";

            WhitespacesArray[WhitespacesArrayCounter] = lexeme;
            WhitespacesArrayPosition[WhitespacesArrayCounter] = row.ToString() + "," + col.ToString();
            WhitespacesArrayCounter++;
        }

        private static void AddLexemeToComparisons(string lexeme, int row, int col)
        {
            ComparisonsArray[ComparisonsArrayCounter] = lexeme;
            ComparisonsArrayPosition[ComparisonsArrayCounter] = row.ToString() + "," + col.ToString();
            ComparisonsArrayCounter++;
        }

        private static void AddLexemeToNumbers(string lexeme, int row, int col)
        {
            NumbersArray[NumbersArrayCounter] = lexeme;
            NumbersArrayPosition[NumbersArrayCounter] = row.ToString() + "," + col.ToString();
            NumbersArrayCounter++;
        }

        private static void AddLexemeToIdentifiers(string lexeme, int row, int col)
        {
            IdentifiersArray[IdentifiersArrayCounter] = lexeme;
            IdentifiersArrayPosition[IdentifiersArrayCounter] = row.ToString() + "," + col.ToString();
            IdentifiersArrayCounter++;
        }

        private static void AddLexemeToUnknowns(string lexeme, int row, int col)
        {
            UnknownsArray[UnknownsArrayCounter] = lexeme;
            UnknownsArrayPosition[UnknownsArrayCounter] = row.ToString() + "," + col.ToString();
            UnknownsArrayCounter++;
        }


        private static void PrintSymbolsTable()
        {

            Console.WriteLine("******************************************************************");
            Console.WriteLine("************************Symbols table*****************************");
            Console.WriteLine("******************************************************************");

            /*
            KEYWORD = 0,
            WHITESPACE,
            COMPARISON,
            IDENTIFIER,
            NUMBER,
            LITERAL,
            UNKNOWN
            */


            int index = 0;

            Console.WriteLine("\r\nKeywords: " + KeywordsArrayCounter.ToString() + " found");

            for(index = 0; index < KeywordsArrayCounter; index++)
            {
                Console.WriteLine(KeywordsArray[index] + " ["+ KeywordsArrayPosition[index] + "]"   );
            }


            Console.WriteLine("\r\nPunctuation: " + PunctuationsArrayCounter.ToString() + " found");

            for (index = 0; index < PunctuationsArrayCounter; index++)
            {
                Console.WriteLine(PunctuationsArray[index] + " [" + PunctuationsArrayPosition[index] + "]");
            }

            Console.WriteLine("\r\nWhitespaces: " + WhitespacesArrayCounter.ToString() + " found");

            for (index = 0; index < WhitespacesArrayCounter; index++)
            {
                Console.WriteLine(WhitespacesArray[index] + " [" + WhitespacesArrayPosition[index] + "]");
            }

            Console.WriteLine("\r\nComparisons: " + ComparisonsArrayCounter.ToString() + " found");

            for (index = 0; index < ComparisonsArrayCounter; index++)
            {
                Console.WriteLine(ComparisonsArray[index] + " [" + ComparisonsArrayPosition[index] + "]");
            }

            Console.WriteLine("\r\nIdentifiers: " + IdentifiersArrayCounter.ToString() + " found");

            for (index = 0; index < IdentifiersArrayCounter; index++)
            {
                Console.WriteLine(IdentifiersArray[index] + " [" + IdentifiersArrayPosition[index] + "]");
            }

            Console.WriteLine("\r\nNumbers: " + NumbersArrayCounter.ToString() + " found");

            for (index = 0; index < NumbersArrayCounter; index++)
            {
                Console.WriteLine(NumbersArray[index] + " [" + NumbersArrayPosition[index] + "]");
            }

            Console.WriteLine("\r\nLiterals: " + LiteralsArrayCounter.ToString() + " found");

            for (index = 0; index < LiteralsArrayCounter; index++)
            {
                Console.WriteLine(LiteralsArray[index] + " [" + LiteralsArrayPosition[index] + "]");
            }

            Console.WriteLine("\r\nUnknowns: " + UnknownsArrayCounter.ToString() + " found");

            for (index = 0; index < UnknownsArrayCounter; index++)
            {
                Console.WriteLine(UnknownsArray[index] + " [" + UnknownsArrayPosition[index] + "]");
            }

        }
    }
}
