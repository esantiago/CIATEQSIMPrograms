//@! brief This program reads file content and print it to output console

#include <stdio.h>

int main () {

vfnReadFileChars();
   
   return(0);

}

void vfnReadFileString()
{
   FILE *fp;
   char str[60];

   /* opening file for reading */
   fp = fopen("/home/pi/CIATEQ/CompilersAndLibraries/testFile.txt" , "r");
   if(fp == NULL) {
      perror("Error opening file");
   }
   if( fgets (str, 60, fp)!=NULL ) {
      /* writing content to stdout */
      puts(str);
   }
   fclose(fp);
}

void vfnReadFileChars()
{
   FILE *fp;
   int c;
   int n = 0;
  
   fp = fopen("/home/pi/CIATEQ/CompilersAndLibraries/testFile.txt","r");
   if(fp == NULL) {
      perror("Error in opening file");
   } 
   
      do {
      c = fgetc(fp);
      
      if( feof(fp) ) {
         break ;
      }
      printf("%c \r\n", c);
      printf("%d \r\n", (int)c - 0x30);
      
   } while(1);

   fclose(fp);
}