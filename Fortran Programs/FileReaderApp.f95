program FILEREADER

   real, dimension (3,3) :: x
   character (len = 2) :: option
   character (len = 100) :: fileName
      
   print *, 'Please select an option' 
   print *, '1: Test Positives'
   print *, '2: Test Negatives'
   print *, '3: Test Fractionals'
   print *, '4: Test Chars'
   print *, '5: Test Long Positives'
   print *, '6: Test Long Negatives'
   print *, '7: Test Long Fractionals'
   print *, '8: Test Long Chars'
      
   
   read *,option

   select case (option)
   
      case ('1') 
      fileName = 'testFilePositives.dat'
      
      case ('2')
      fileName = 'testFileNegatives.dat'
      
      case ('3')
      fileName = 'testFileFractional.dat'

      case ('4')
      fileName = 'testFileChar.dat'
      
      case ('5') 
      fileName = 'testFileLongPositives.dat'
      
      case ('6')
      fileName = 'testFileLongNegatives.dat'
      
      case ('7')
      fileName = 'testFileLongFractional.dat'

      case ('8')
      fileName = 'testFileLongChar.dat'

      case default
         print*, "Invalid option" 
         call EXIT(0)
      
   end select
   
   


   !print input data:
   open (unit=99, file=fileName, status='old', action='read')
   print *, ' '
   print *, 'Input matrix:'


   do I=1,3,1
      read(99,*) x(I,:)  !read file line and stores each element separated by space on corresponding vector position
      write(*,*) x(I,:)  !print rowI col1,2,3
      
      !write(*,*) x(I,1)  !print rowI col1
      !write(*,*) x(I,2)  !print rowI col2
      !write(*,*) x(I,3)  !print rowI col3
      
   enddo

   close(99)



   !print output data:
   open (unit=99, file=fileName, status='old', action='read')
   print *, ' '
   print *, 'Output matrix: '


   do I=1,3,1
      read(99,*) x(I,:)  !read file line and stores each element separated by space on corresponding vector position
      write(*,*) x(I,:)*x(I,:)  !print rowI col1,2,3 square
      
      !write(*,*) x(I,1)*x(I,1)  !print rowI col1 square
      !write(*,*) x(I,2)*x(I,2)  !print rowI col2 square
      !write(*,*) x(I,3)*x(I,3)  !print rowI col3 square
      
   enddo

   close(99)

end