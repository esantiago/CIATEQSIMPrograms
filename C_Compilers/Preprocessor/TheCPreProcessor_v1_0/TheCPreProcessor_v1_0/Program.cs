using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace TheCPreProcessor_v1_0
{
    class Program
    {

        const string SOURCE_FILE = @"C:\Users\EMMANUEL\source\repos\TheCPreProcessor_v1_0\TheCPreProcessor_v1_0\bin\Debug\class2.c";
        const string DESTINATION_FILE = @"C:\Users\EMMANUEL\source\repos\TheCPreProcessor_v1_0\TheCPreProcessor_v1_0\bin\Debug\class2OUTPUT.c";


        //static string[] DefinesArray = new string[20];
        //static string[] DefinesArrayEquivalence = new string[20];
        static ArrayList DefinesArray = new ArrayList(20);
        static ArrayList DefinesArrayEquivalence = new ArrayList(20);
        static int DefinesCounter = 0;


        private static void AddDefinition(string definition, string definitionValue)
        {
            //DefinesArray[DefinesCounter] = definition;
            DefinesArray.Add(definition);
            //DefinesArrayEquivalence[DefinesCounter] = definitionValue;
            DefinesArrayEquivalence.Add(definitionValue);
            DefinesCounter++;
        }

        private static void RemoveDefinition(string definition)
        {

            //DefinesArray = DefinesArray.Where(val => val != definition).ToArray();
            int DefineToRemoveIndex = DefinesArray.IndexOf(definition);
            DefinesArray.RemoveAt(DefineToRemoveIndex);
            DefinesArrayEquivalence.RemoveAt(DefineToRemoveIndex);
            DefinesCounter--;
        }

        private static string DeleteComments(string inputString, out int resultCode)
        {
            int coincidenceIndex = 0;
            string outputString = inputString;
            resultCode = 0;

            coincidenceIndex = inputString.IndexOf("//");


            if (coincidenceIndex != -1)   //check if string contains comment sequence
            {
                outputString = inputString.Substring(0, coincidenceIndex);  //delete chars from '//' occurrence
                resultCode = 1;
            }


            return outputString;
        }


        

        private static string DeleteStrings(string inputString)
        {
            int coincidenceIndex1 = 0;
            int coincidenceIndex2 = 0;
            int printfindex = 0;
            string outputString = inputString;
            bool Found = false;

            coincidenceIndex1 =  inputString.IndexOf("printf");  //search printf string

            if (coincidenceIndex1 == -1)
                return outputString;

            coincidenceIndex1 = inputString.IndexOf("(",coincidenceIndex1 + "printf".Length);  //search '(' string
            printfindex = coincidenceIndex1+1;

            if (coincidenceIndex1 == -1)
                return outputString;

            coincidenceIndex1 = inputString.IndexOf(")", coincidenceIndex1 + 1);  //search ')' string


            if (coincidenceIndex1 == -1)
                return outputString;


            coincidenceIndex1 = inputString.IndexOf('\"',printfindex);

            if (coincidenceIndex1 != -1)   //check if string contains " opening char 
            {
                coincidenceIndex2 = coincidenceIndex1 + 1 + inputString.Substring(coincidenceIndex1 + 1, inputString.Length - (coincidenceIndex1 + 1)).IndexOf('\"');

                if (coincidenceIndex2 != -1)   //check if string contains " closing char
                {
                    if (coincidenceIndex2 > coincidenceIndex1)
                    {
                        outputString = inputString.Substring(0, coincidenceIndex1) + inputString.Substring(coincidenceIndex2+1, (inputString.Length-(coincidenceIndex2+1)));
                    }

                }
            }

            return outputString;
        }

        private static string GetStringBetweenChar(int startindex, string inputString, char coincidence)
        {
            int coincidenceIndex1 = 0;
            int coincidenceIndex2 = 0;
            string outputString = "";
            bool Found = false;

            coincidenceIndex1 = inputString.IndexOf(coincidence, startindex);

            if (coincidenceIndex1 != -1)   //check if string contains " opening char 
            {
                coincidenceIndex2 = coincidenceIndex1 + 1 + inputString.Substring(coincidenceIndex1 + 1, inputString.Length - (coincidenceIndex1 + 1)).IndexOf(coincidence, startindex);

                if (coincidenceIndex2 != -1)   //check if string contains " closing char
                {
                    if (coincidenceIndex2 > coincidenceIndex1)
                    {
                        outputString = inputString.Substring(coincidenceIndex1+1,coincidenceIndex2 - (coincidenceIndex1+1));
                    }

                }
            }

            return outputString;
        }

        private static string GetStringBetweenChars(int startindex, string inputString, char openChar, char closingChar)
        {
            int coincidenceIndex1 = 0;
            int coincidenceIndex2 = 0;
            string outputString = "";
            bool Found = false;

            coincidenceIndex1 = inputString.IndexOf(openChar, startindex);

            if (coincidenceIndex1 != -1)   //check if string contains " opening char 
            {
                //coincidenceIndex2 = coincidenceIndex1 + 1 + inputString.Substring(coincidenceIndex1 + 1, inputString.Length - (coincidenceIndex1 + 1)).IndexOf(closingChar, startindex);
                coincidenceIndex2 = coincidenceIndex1 + 1 + inputString.Substring(coincidenceIndex1 + 1, inputString.Length - (coincidenceIndex1 + 1)).IndexOf(closingChar);

                if (coincidenceIndex2 != -1)   //check if string contains " closing char
                {
                    if (coincidenceIndex2 > coincidenceIndex1)
                    {
                        outputString = inputString.Substring(coincidenceIndex1 + 1, coincidenceIndex2 - (coincidenceIndex1 + 1));
                    }

                }
            }

            return outputString;
        }



        //check if definition is part of a string or not (true = definition is part of string, false definition is NOT part of a string and return index where string ends)
        private static bool CheckDefineIsString(string line, int currentIndex, string definition ,out int lineIndex)
        {
            int startIndex = 0;
            int finalIndex = 0;
            lineIndex = currentIndex+1;
            
            startIndex = line.IndexOf('\"',currentIndex);
            if (startIndex == -1)  //coincidence not found
                return false; //not " string opening char found

            finalIndex = line.IndexOf('\"',startIndex+1);
            if (finalIndex == -1)
                return false; //not " string closing char found

            //finalIndex += startIndex;//add absolute offset of final coincidence

            //to be here means string found (due to "" chars were found)
            if (line.IndexOf(definition) < finalIndex)  //definition is part of string
            {
                lineIndex = finalIndex+1;
                return true;
            }
            else
            {
                return false;
            }



        }

        private static string DefineReplacement(string inputString, out int resultCode)
        {

            int index = 0;
            int macroStartIndex = 0;
            string outputString = inputString;
            bool Found = false;
            char c = '\0';
            int offset = 0;
            resultCode = 0;
            int quotesSearchIndex = 0;


            for (index = 0; index < DefinesCounter; index++)
            {
                macroStartIndex = 0;
                quotesSearchIndex = 0;

                if (DefinesArray[index] != null)
            do 
            {
                           Found = false; //flag init
                        

                        try
                        {
                            macroStartIndex = inputString.IndexOf(DefinesArray[index].ToString(), macroStartIndex); //get index of macro definition
                        }
                        catch (Exception e)
                        {
                            break;
                        };

                        
                        if (macroStartIndex != -1 && (inputString.IndexOf("#undef") == -1) ) //coincidence found AND not part of a undefintion directive
                        {
                            Found = true;

                            if (CheckDefineIsString(inputString, quotesSearchIndex, DefinesArray[index].ToString(), out int newIndex)) //check if definiton is part of a string 
                            {
                                //defintion is part of a string so it shall not be replaced
                                //macroStartIndex contains index for start searching for definition so get exact position with below line
                                macroStartIndex = newIndex;
                                quotesSearchIndex = newIndex;
                                continue;
                            }

                            offset = DefinesArray[index].ToString().Length;

                                if (macroStartIndex != 0)
                                {
                                    // check previous char were macro definition was found
                                   c = inputString.ToCharArray()[macroStartIndex - 1];

                                    if (c == ' ' || c == '(' || c == ')' || c == ',' || c == ';' || c == '=')
                                    {
                                        // check post char were macro definition was found
                                        c = inputString.ToCharArray()[macroStartIndex + DefinesArray[index].ToString().Length];

                                        if (c == ' ' || c == '(' || c == ')' || c == ',' || c == ';' || c == '=')
                                        {

                                            var theString = inputString;
                                            var aStringBuilder = new StringBuilder(inputString);
                                            aStringBuilder.Remove(macroStartIndex, DefinesArray[index].ToString().Length);
                                            aStringBuilder.Insert(macroStartIndex, DefinesArrayEquivalence[index]);
                                            outputString = aStringBuilder.ToString();
                                            inputString = outputString;
                                            offset = DefinesArrayEquivalence[index].ToString().Length;
                                            resultCode = 1; //set flag for updating file line
                                        //Found = true;

                                    }

                                    }
                                }
                                else
                                {

                                    // check post char were macro definition was found
                                    c = inputString.ToCharArray()[macroStartIndex + DefinesArray[index].ToString().Length];

                                    if (c == ' ' || c == '(' || c == ')' || c == ',' || c == ';')
                                    {

                                        var theString = inputString;
                                        var aStringBuilder = new StringBuilder(inputString);
                                        aStringBuilder.Remove(macroStartIndex, DefinesArray[index].ToString().Length);
                                        aStringBuilder.Insert(macroStartIndex, DefinesArrayEquivalence[index]);
                                        outputString = aStringBuilder.ToString();
                                        inputString = outputString;
                                        offset = DefinesArrayEquivalence[index].ToString().Length;
                                    //Found = true;

                                }
                                }

                            macroStartIndex += offset;

                            if ((DefinesArray[index].ToString().Length) > (inputString.Length - macroStartIndex)) //we may not do another attemtp with this line du to overflow
                                break;
                        }
              } while (Found) ;
            }


            return outputString;
        }

        private static bool ProcessIfDirective(string [] lines, int currentLineIndex)
        {
            int index = 0;

            int ifLineIndex = 0;
            int elseLineIndex = 0;
            int endifLineIndex = 0;

            for (index = currentLineIndex; index < lines.Length; index++)
            {
                if (lines[index].IndexOf("#if") != -1) //look for #if directive
                    ifLineIndex = index;
                else if (lines[index].IndexOf("#else") != -1) //look for #else directive
                    elseLineIndex = index;
                else if (lines[index].IndexOf("#endif") != -1) //look for #endif directive
                {
                    endifLineIndex = index;
                    break;
                }
            }

            if ((endifLineIndex > elseLineIndex) && (elseLineIndex > ifLineIndex))  //check hierarchical order
            {

                //evaluate #if condition

                string[] splittedString;
                string line = lines[ifLineIndex];
                string conditionToCheck = "";
                bool ifResult = false;

                if (line.IndexOf(" ") != -1)
                {
                    splittedString = line.Split(' ');

                    if (splittedString.Length < 2)
                        return false; //incorrect format

                    conditionToCheck =  splittedString[1];
                    
                }
                else //incorrect format
                {
                    return false;
                }

                if (conditionToCheck.IndexOf("==") != -1)
                {
                    splittedString = conditionToCheck.Split(new string[] { "==" }, StringSplitOptions.None);
                    if (splittedString.Length >= 2)  //at least to elements are expected for comparison
                    {
                        if (splittedString[0] == splittedString[1]) //evaluate condition
                            ifResult = true;
                    }
                }
                else if (conditionToCheck.IndexOf("!=") != -1)
                {
                    splittedString = conditionToCheck.Split(new string[] { "!=" }, StringSplitOptions.None);
                    if (splittedString.Length >= 2)  //at least to elements are expected for comparison
                    {
                        if (splittedString[0] != splittedString[1]) //evaluate condition
                            ifResult = true;
                    }
                }

                //check result of evaluation for replacing corresponding lines
                if (ifResult == true)   //evaluation result was true
                {
                    lines[ifLineIndex] = "";
                    for (index = elseLineIndex; index <= endifLineIndex; index++) //delete useless code lines
                        lines[index] = "";

                }
                else //evaluation result was false
                {

                    for (index = ifLineIndex; index <= elseLineIndex; index++) //delete useless code lines
                        lines[index] = "";

                    lines[endifLineIndex] = "";
                    

                }

                File.WriteAllLines(SOURCE_FILE, lines);  //update file 
                return true;  //#if well constructed and evaluated
            }


            return false;

        }

        private static string LookForPreprocessorDirectives(string [] fileLines, int currentLineIndex, string inputString, out int resultcode)
        {
            string token = "";
            int startindex = 0;
            int finalindex = 0;
            string outputString = inputString;
            resultcode = 0;


            //check if line contains '#' token
            startindex = inputString.IndexOf("#");
            if (startindex != -1)
            {
                finalindex = inputString.IndexOf(" ",startindex+1);

                if (finalindex == -1)
                    return inputString;

                finalindex += startindex; //absolute position within string

                token = inputString.Substring(startindex+1, finalindex - (startindex+1));

                //get string until space is found
                switch (token)
                {
                    case "include":
                        string fileName = GetStringBetweenChar(startindex, inputString, '\"');
                        if(fileName == "")
                        fileName =  GetStringBetweenChars(startindex, inputString, '<','>');

                        //if (fileName == "stdio.h")  //for illustrative purposes reject stdio.h due this is a standard header file not included in current source directory
                            //break;

                        string path = Directory.GetCurrentDirectory() + "\\" + fileName;
                        string readText;
                        FileInfo fi = new FileInfo(path);
                        bool flag = false;

                        try
                        {
                            if (File.Exists(path) && fi.Extension == ".c" || fi.Extension == ".h")
                            {
                                readText = File.ReadAllText(path);
                                resultcode = 1;
                                outputString = readText;
                            }
                            else
                            {
                                //file not found
                                outputString = "";
                                resultcode = 1;  //set flag for updating file
                            }
                        }
                        catch (Exception e)
                        {
                            //file not found
                            outputString = "";
                            resultcode = 1;  //set flag for updating file
                        };


                        break;


                    case "define":

                        string [] splittedLine = inputString.Split(' ');

                        if(splittedLine.Length < 3)  //expected at least 3 elements: #define definition defintion_value
                            return outputString;  //error

                        AddDefinition(splittedLine[1], splittedLine[2]);
                        outputString = "";
                        resultcode = 1;  //set flag for updating file


                        break;

                    case "undef":

                        string[] splittedLine2 = inputString.Split(' ');

                        if (splittedLine2.Length < 2)  //expected at least 2 elements: #undef definition
                            return outputString;  //error

                        RemoveDefinition(splittedLine2[1]);
                        outputString = "";
                        resultcode = 1;  //set flag for updating file

                        break;

                    case "if":

                        ProcessIfDirective(fileLines, currentLineIndex);

                        break;
                }
            }

            return outputString;
        }


        static string [] SplitLine(string line)
        {
            Regex r = null;
            //return line.Split(new Char[] { ',', ' ', '(', ')' });
            return r.Split(line, ' ');    // Split on hyphens
        }

        static void Main(string[] args)
        {
            //string line = "";
            string result = "";
            int resultcode = 0;
            int currentlinecounter = 0;


            //DefinesArray.Add("PI");
            //DefinesArrayEquivalence.Add("3.14");
            //DefinesCounter = 1;
            //DefineReplacement(" printf(\"Is PI\", PI);   ", out resultcode);

            do
            {
                currentlinecounter = 0;
                resultcode = 0;  //flag init
                //System.IO.StreamReader file = new System.IO.StreamReader(SOURCE_FILE);
                string[] lines = File.ReadAllLines(SOURCE_FILE);
                //FileStream file = new FileStream(SOURCE_FILE, FileMode.OpenOrCreate,FileAccess.ReadWrite, FileShare.None);
                foreach (string line in lines)//while ((line = file.ReadLine()) != null)
                {
                    currentlinecounter++;

                    //Delete comments from line
                    result = DeleteComments(line, out resultcode);

                    if (1 == resultcode)  //check if include file was found
                    {

                        lines[currentlinecounter - 1] = result;
                        File.WriteAllLines(SOURCE_FILE, lines);
                        break;

                    }

                    //Delete "strings" from line (only if they are part of a non #directives, e.g. print statements)
                    //result = DeleteStrings(result);

                    //Check macro substitution (DefinesArray)
                    result = DefineReplacement(result, out resultcode);
                    if (1 == resultcode)  //check if include file was found
                    {

                        lines[currentlinecounter - 1] = result;
                        File.WriteAllLines(SOURCE_FILE, lines);
                        break;

                    }

                    //Look for preprocessor directives (#) and apply action
                    result = LookForPreprocessorDirectives(lines, currentlinecounter-1,line, out resultcode);

                    if (1 == resultcode)  //check if include file was found
                    {

                        lines[currentlinecounter - 1] = result;
                        File.WriteAllLines(SOURCE_FILE, lines);
                        break;

                    }
                }
            }
            while (1 == resultcode); //include found
            //file.Close();

        }




    }
}




