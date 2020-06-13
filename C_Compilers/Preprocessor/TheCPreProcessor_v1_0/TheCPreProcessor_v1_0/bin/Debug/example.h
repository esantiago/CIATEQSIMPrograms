// This is example.h
#include <stdio.h>
 
void get_number()
{
    int this_is_a_number;
 
    printf( "Please enter a number: " );
    scanf( "%d", &this_is_a_number );
    printf( "You entered %d", this_is_a_number );
}