//@! brief This program reads file content and print it to output console

#include <stdio.h>
#include <string.h>

#define FILE_PATH_POSITIVES        "/home/pi/CIATEQ/CompilersAndLibraries/testFilePositives.dat"
#define FILE_PATH_NEGATIVES        "/home/pi/CIATEQ/CompilersAndLibraries/testFileNegatives.dat"
#define FILE_PATH_FRACTIONAL       "/home/pi/CIATEQ/CompilersAndLibraries/testFileFractional.dat"
#define FILE_PATH_CHAR             "/home/pi/CIATEQ/CompilersAndLibraries/testFileChar.dat"
#define FILE_PATH_LONG_POSITIVES   "/home/pi/CIATEQ/CompilersAndLibraries/testFileLongPositives.dat"
#define FILE_PATH_LONG_NEGATIVES   "/home/pi/CIATEQ/CompilersAndLibraries/testFileLongNegatives.dat"
#define FILE_PATH_LONG_FRACTIONAL  "/home/pi/CIATEQ/CompilersAndLibraries/testFileLongFractional.dat"
#define FILE_PATH_LONG_CHAR        "/home/pi/CIATEQ/CompilersAndLibraries/testFileLongChar.dat"


int readmatrix(size_t rows, size_t cols, float (*a)[cols], const char* filename)
{

    FILE *pf;
    pf = fopen (filename, "r");
    if (pf == NULL)
        return 0;

    for(size_t i = 0; i < rows; ++i)
    {
        for(size_t j = 0; j < cols; ++j)
            fscanf(pf, "%f", a[i] + j);            
    }


    fclose (pf); 
    return 1; 
}

int main(void)
{
    int option = 0;
    float matrix[3][3];
    char lu8FileName[100]= {};
    
    printf("Please select an option: \r\n"); 
    printf("1: Test Positives \r\n");
    printf("2: Test Negatives \r\n");
    printf("3: Test Fractionals \r\n");
    printf("4: Test Chars \r\n");
    printf("5: Test Long Positives \r\n");
    printf("6: Test Long Negatives \r\n");
    printf("7: Test Long Fractionals \r\n");
    printf("8: Test Long Chars \r\n");
    
    scanf("%c", &option);
    
    switch(option)
    {
     case '1':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_POSITIVES);
     break;
     
     case '2':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_NEGATIVES);
     break;

     case '3':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_FRACTIONAL);
     break;
     
     case '4':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_CHAR);
     break;
     
     case '5':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_LONG_POSITIVES);
     break;
     
     case '6':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_LONG_NEGATIVES);
     break;

     case '7':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_LONG_FRACTIONAL);
     break;
     
     case '8':
      strcpy((char*)lu8FileName, (const char*)FILE_PATH_LONG_CHAR);
     break;
     
     default: 
      printf("Invalid option \r\n"); 
      return 0;
     break;
    
    }

    readmatrix(3, 3, matrix, (const char *)lu8FileName);
    
    printf("\r\n Input data: \r\n");
    for(size_t i = 0; i < 3; ++i)
    {
        for(size_t j = 0; j < 3; ++j)
            printf("%-3f ", matrix[i][j]);
        puts("");
    }

    printf("\r\n Output data: \r\n");
    for(size_t i = 0; i < 3; ++i)
    {
        for(size_t j = 0; j < 3; ++j)
            printf("%-3f ", (matrix[i][j]) * (matrix[i][j]) );
        puts("");
    }

    return 0;
}