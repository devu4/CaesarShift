using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CeasarCipher
{
    class CipherClass
    {
        //method to encrypt and decrypt a caesar ciphered text
        //string value = text that you want to decrypt or encrypt
        //int shift = integer to shift text by, use - integer to encrypt.
        static string DoShift(string value, int shift)
        {
            char[] LetterArray = value.ToCharArray().Where(c => Char.IsLetter(c)).ToArray(); //converts string to array, this function also ignores anything that isn't a letter because we cannot use anything that isn't a letter.
            for (int i = 0; i < LetterArray.Length; i++)
            {
                // Letter.
                char letter = LetterArray[i];
                // Add shift to all.
                letter = (char)(letter + shift);
                // Subtract 26 on overflow.
                // Add 26 on underflow.
                if (letter > 'Z')
                {
                    letter = (char)(letter - 26);
                }
                else if (letter < 'A')
                {
                    letter = (char)(letter + 26);
                }
                // Store.
                LetterArray[i] = letter;
            }
            return new string(LetterArray); //output as a new string instead of array
        }


        //method to use frequency analyse on string and work out shift from char E
        //string value = text that you want to decrypt or encrypt
		//returns int = shift most likely to solve the cipher
        static int FrequencyAnalysis(string value)
        {
           char[] LetterArray = value.ToCharArray().Where(c => Char.IsLetter(c)).ToArray();
           value = new string(LetterArray); //these two functions gets rid of anything that isn't a letter, this messes with the frequency count.

            char mostOccurring = (char)(value.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key); //creates frequency table and orders by decending. Then selects first value in table.
            int ePos = ((int)char.ToUpper('E')) - 64; //find position of E in alphabet
            int mostOccurringPos = ((int)char.ToUpper(mostOccurring)) - 64; //find position of most occurring character in textin the alphabet
            int shiftrequired = mostOccurringPos - ePos; //gives most likely shift needed to the right
            shiftrequired = 26 - shiftrequired; // take away from 26 to give most likely shift needed to the left
            return shiftrequired;
            
        }


        static void Main(string[] args)
        {
            string text = "";
            int cType = 1;
            bool ctypeEntered = false; //bool to see if user has entered correct option
            bool frequencyAllowedEntered = false; // enable frequency analyse
            bool viewModeEntered = false; //bool to see if user has entered correct option
            Console.WriteLine("------------------------------------------------------------------------------------");
            Console.WriteLine("Please place your text in the OrignalText.txt file in the root folder of the executable.");
            Console.WriteLine("------------------------------------------------------------------------------------");

            while (!ctypeEntered)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine("Would you like to encrypt or decrypt the code?");
                Console.WriteLine("1. Decrypt");
                Console.WriteLine("2. Encrypt");
                Console.WriteLine("------------------------------------------------------------------------------------");
                string caesarMode = Console.ReadLine(); // see if user wanted to encrypt or decrypt

                if (caesarMode == "1") //users wants to decrypt
                {
                    cType = 1; //this sets a negative shift thefore allowing for decrypt.
                    ctypeEntered = true;
                }
                else if(caesarMode == "2")//users wants to encrypt
                {
                    cType = -1; //this sets a positive shift thefore allowing for decrypt.
                    ctypeEntered = true;
                }
                else
                    Console.WriteLine("\nYou entered a non-existent option. Please try again!");
            }

            if (cType == 1)
            {
                while (!frequencyAllowedEntered)
                {
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    Console.WriteLine("Would you like to to use frequency analysis to decrypt this text? Else say No to brute force the correct shift!");
                    Console.WriteLine("1. Yes");
                    Console.WriteLine("2. No");
                    Console.WriteLine("------------------------------------------------------------------------------------");
                    string usefrequencyMode = Console.ReadLine(); // see if user wanted to encrypt or decrypt

                    if (usefrequencyMode == "1") //users wants to decrypt
                    {
                        bool keyEntered = false;
                        while (!keyEntered)
                        {
                            try
                            {
                                text = File.ReadAllText(@"OrginalText.txt").ToUpper(); //read text from the text file to be encrypted or decrypted and uppercase text.
                            }
                            catch(System.IO.FileNotFoundException) //if file is not found, handle exception!
                            {
                                Console.WriteLine("File was not found, please make file named OriginalText.txt with your input to continue!");
                                Console.ReadKey();
                                return;
                            }
                            catch (Exception e) //handle generic exceptions
                            {
                                Console.WriteLine("An error occurred: '{0}'", e);
                                Console.ReadKey();
                                return;
                            }

                            int shiftneeded = FrequencyAnalysis(text); //get shift most likely to solve cipher using frequency analysis class
                            Console.WriteLine("\n------------------------------------------------------------------------------------");
                            Console.WriteLine("Shift most likely to solve the cipher is {0}", shiftneeded);
                            Console.WriteLine("{0}", DoShift(text, shiftneeded));
                            Console.WriteLine("------------------------------------------------------------------------------------");
                            Console.WriteLine("\nPlease confirm this is the correct shift you would like to save! Else say No to brute force the correct shift!");
                            Console.WriteLine("1. Yes");
                            Console.WriteLine("2. No");
                            Console.WriteLine("------------------------------------------------------------------------------------");

                            string confirmsaveShift = Console.ReadLine();

                            if (confirmsaveShift == "1")
                            {
                                string[] lines = { "------------------------------------------------------------------------------------", shiftneeded.ToString(), DoShift(text, shiftneeded), "------------------------------------------------------------------------------------" };
                                File.WriteAllLines(@"OutputText.txt", lines);
                                Console.WriteLine("\n------------------------------------------------------------------------------------");
                                Console.WriteLine("Your text has been outputted to OutputText.txt in the root folder!");
                                Console.WriteLine("------------------------------------------------------------------------------------");
                                keyEntered = true;
                                viewModeEntered = true;

                            }
                            else if (confirmsaveShift == "2")
                            {
                                keyEntered = true;
                            }
                            else
                                Console.WriteLine("\nYou entered a non-existent option. Please try again!");
                        }

                        frequencyAllowedEntered = true;
                    }
                    else if (usefrequencyMode == "2")//users wants to encrypt
                    {
                        frequencyAllowedEntered = true;
                    }
                    else
                        Console.WriteLine("\nYou entered a non-existent option. Please try again!");
                }
            }

            while (!viewModeEntered)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine("Would you like to view output as html or in the console?");
                Console.WriteLine("1. HTML");
                Console.WriteLine("2. Console");
                Console.WriteLine("------------------------------------------------------------------------------------");
                string viewMode = Console.ReadLine(); // see if user wanted to see output in html or console

                if (viewMode == "1") //users wants to see html
                {
                    string htmlMiddle = "";

                    try
                    {
                        text = File.ReadAllText(@"OrginalText.txt").ToUpper(); //read text from the text file to be encrypted or decrypted and uppercase text.
                    }
                    catch (System.IO.FileNotFoundException) //if file is not found, handle exception!
                    {
                        Console.WriteLine("File was not found, please make file named OriginalText.txt with your input to continue!");
                        Console.ReadKey();
                        return;
                    }
                    catch (Exception e) //handle generic exceptions
                    {
                        Console.WriteLine("An error occurred: '{0}'", e);
                        Console.ReadKey();
                        return;
                    }

                    string htmlStart = File.ReadAllText(@"Templates\HTMLStartTemplate.txt"); //loads the start of html template
                    string htmlEnd = File.ReadAllText(@"Templates\HTMLEndTemplate.txt"); //loads the end of html template


                    for (int shift = 0; shift < 26; shift++)
                    {
                       
                        string message = String.Format(@"<li>
                                              <a href ='#{0}'> Shift {0} </a>
                                              <div id='{0}' class='accordion'>
                                                   {1}
                                             </div>
                                            </li>", shift, DoShift(text, (cType * shift)));

                        htmlMiddle += message;//append middle html to what was before to create a full length page

                    }


                    string[] lines = { htmlStart, htmlMiddle, htmlEnd }; //array of lines to go into output html page
                    File.WriteAllLines(@"CaesarOutput.html", lines); //insert lines array into output html page

                    System.Diagnostics.Process.Start(@"CaesarOutput.html"); //opens webpage in default application

                    viewModeEntered = true; //set entered to true so the while loop stops.
                }
                else if (viewMode == "2")//users wants to see output in console
                {
                    int KeytoShift = 0;

                    try
                    {
                        text = File.ReadAllText(@"OrginalText.txt").ToUpper(); //read text from the text file to be encrypted or decrypted and uppercase text.
                    }
                    catch (System.IO.FileNotFoundException) //if file is not found, handle exception!
                    {
                        Console.WriteLine("File was not found, please make file named OriginalText.txt with your input to continue!");
                        Console.ReadKey();
                        return;
                    }
                    catch (Exception e) //handle generic exceptions
                    {
                        Console.WriteLine("An error occurred: '{0}'", e);
                        Console.ReadKey();
                        return;
                    }

                    for (int shift = 0; shift < 26; shift++)
                    {
                        Console.WriteLine("------------------------------------------------------------------------------------");
                        Console.WriteLine("Key: {0}", shift);
                        Console.WriteLine("{0}", DoShift(text, (cType*shift)));
                        Console.WriteLine("------------------------------------------------------------------------------------");
                    }

                    bool keyEntered = false; //if user has entered an aprioriate integer for key/shift
                    while (!keyEntered)
                    {
                        Console.WriteLine("\n------------------------------------------------------------------------------------");
                        Console.WriteLine("Enter the Key/Shift you would like to save in OutputText.txt");
                        Console.WriteLine("------------------------------------------------------------------------------------");

                        bool keyEntered2 = false; //if user has entered an aprioriate integer for key/shift
                        while (!keyEntered2)
                        {
                            try
                            {
                                KeytoShift = int.Parse(Console.ReadLine());
                            }
                            catch (System.FormatException)  //handle invalid formats
                            {
                                Console.WriteLine("\nYou entered a non-existent option. Please try again!");
                                continue;
                            }
                            catch (Exception e) //handle generic exceptions
                            {
                                Console.WriteLine("An error occurred: '{0}'", e);
                                continue;
                            }

                            if(KeytoShift < 0 || KeytoShift > 25) //check user has entered value in range
                                Console.WriteLine("\nYou entered a non-existent option. Please try again!");
                            else
                                keyEntered2 = true; //if no exception and value is in range then user has entered an apprioriate value and so exit out of loop
                        }

                        Console.WriteLine("------------------------------------------------------------------------------------");
                        Console.WriteLine("Key: {0}", KeytoShift);
                        Console.WriteLine("{0}", DoShift(text, KeytoShift));
                        Console.WriteLine("------------------------------------------------------------------------------------");
                        Console.WriteLine("\nPlease confirm this is the key you would like to save!");
                        Console.WriteLine("1. Yes");
                        Console.WriteLine("2. No");
                        Console.WriteLine("------------------------------------------------------------------------------------");
                        string confirmSave = Console.ReadLine();

                        if(confirmSave == "1")
                        {
                            string[] lines = { "------------------------------------------------------------------------------------", KeytoShift.ToString(), DoShift(text, KeytoShift), "------------------------------------------------------------------------------------" };
                            File.WriteAllLines(@"OutputText.txt", lines);
                            Console.WriteLine("\n------------------------------------------------------------------------------------");
                            Console.WriteLine("Your text has been output to OutputText.txt in the root folder!");
                            Console.WriteLine("------------------------------------------------------------------------------------");
                            keyEntered = true;

                        }
                        else if (confirmSave == "2")
                        {
                            continue;
                        }
                        else
                            Console.WriteLine("\nYou entered a non-existent option. Please try again!");

                    }
                    viewModeEntered = true;
                }
                else
                    Console.WriteLine("\nYou entered a non-existent option. Please try again!");
            }

            Console.WriteLine("Thank you for using CaesarCipher!");
			Console.ReadKey();
        }
    }
}
