import numpy as np
import time
import traceback
import sys


try:

 option = input("Please enter an option: \r\n"
 +"1: Test Positives\r\n"
 +"2: Test Negatives\r\n"
 +"3: Test Fractionals\r\n"
 +"4: Test Chars\r\n"
 +"5: Test Long Positives\r\n"
 +"6: Test Long Negatives\r\n"
 +"7: Test Long Fractionals\r\n"
 +"8: Test Long Chars\r\n"
 )
 
 if option == '1': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFilePositives.dat"
 elif option == '2': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileNegatives.dat"
 elif option == '3': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileFractional.dat"
 elif option == '4': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileChar.dat"
 elif option == '5': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileLongPositives.dat"
 elif option == '6': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileLongNegatives.dat"
 elif option == '7': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileLongFractional.dat"
 elif option == '8': 
  filePath = "C:\\Users\\EMMANUEL\\Academic\\CIATEQ\\Master degree\\TRIMESTER II\\Compilers\\Fortran programs\\testFileChar.dat"
 else:
  print("Invalid option") 
  sys.exit(0)
  
 #Get data from file
 np.fromfile(filePath, dtype=float)
 matrix = np.loadtxt(filePath, usecols=range(3))
 
 print ("Input data: \r\n")       
 print (matrix)
 
 print ("Output data: \r\n")        
 print (matrix*matrix)
 
 
 
 input("Press any key to continue")
 
except:
    traceback.print_exc()
    input("Press any key to continue")
