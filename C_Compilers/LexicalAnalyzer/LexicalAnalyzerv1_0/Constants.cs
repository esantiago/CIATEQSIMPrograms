using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalAnalyzerv1_0
{
    class Constants
    {

        protected const string SOURCE_FILE = @"C:\Users\EMMANUEL\Academic\CIATEQ\Master degree\TRIMESTER II\Compilers\Homeworks\Lexical analyzer\LexicalAnalyzerv1_0\LexicalAnalyzerv1_0\bin\Debug\class2.c";
        //protected const string[] colors = new const string[3] { "red", "purple", "green" };
        protected static readonly string[] keywordReferenceArray = { "auto","break","case","char","const","continue","default","do","double","else","enum","extern","float","for","goto","if","int","long", "main",  "printf","register","return", "scanf", "short","signed","sizeof","static","struct","switch","typedef","union","unsigned","void","volatile","while"};
        protected static readonly string[] whitespaceReferenceArray = {" ", "\t", "\r", "\n" };
        protected static readonly string[] comparisonReferenceArray = { "&", "==", "!=", ">", "<", ">=", "<=", "+", "-", "*" };
        protected static readonly string[] punctuationReferenceArray = { "(", ")", ",", ";", "}", "{", "]", "[" };


        protected enum tokensIDs
        {
            KEYWORD = 0,
            PUNCTUATION,
            WHITESPACE,
            COMPARISON,
            IDENTIFIER,
            NUMBER,
            LITERAL,
            UNKNOWN
        };

    }
}
