using System;
using System.Collections.Generic;

namespace huffman {
    class Program {
        static void Main(string[] args) {
            Console.Title = "Huffman Code"; // Setting the Console name.
            List<HuffmanNode> nodeList; // store nodes on List.

            ProcessMethods pMethods = new ProcessMethods();


            while (true) {
                Console.Clear();
                nodeList = pMethods.getListFromFile();
                Console.Clear();
                if (nodeList == null) {
                    ProcessMethods.setColor();
                    Console.WriteLine("File cannot be read!");
                    Console.WriteLine("Pressthe any key to continue or Enter \"E\" to exit the program.");
                    ProcessMethods.setColorDefault();
                    string choise = Console.ReadLine();
                    if (choise.ToLower() == "e")
                        break;
                    else
                        continue;
                }
                else {
                    Console.Clear();
                    ProcessMethods.setColor();
                    Console.WriteLine("#Symbols   -   #Frequency");
                    ProcessMethods.setColorDefault();
                    pMethods.PrintInformation(nodeList);
                    pMethods.getTreeFromList(nodeList);
                    pMethods.setCodeToTheTree("", nodeList[0]);
                    Console.WriteLine("\n\n");
                    ProcessMethods.setColor();
                    Console.WriteLine(" #   Huffman Code Tree   # \n");
                    ProcessMethods.setColorDefault();
                    pMethods.PrintTree(0, nodeList[0]);
                    ProcessMethods.setColor();
                    Console.WriteLine("\n\n#Symbols    -    #Codes\n");
                    ProcessMethods.setColorDefault();
                    pMethods.PrintfLeafAndCodes(nodeList[0]);
                    Console.WriteLine("\n\n");
                    ProcessMethods.setColor();
                    Console.WriteLine("Press the any key to contunie");
                    Console.WriteLine("Enter the\"E\" to exit the program.");
                    ProcessMethods.setColorDefault();
                    string choise = Console.ReadLine();
                    if (choise.ToLower() == "e")
                        break;
                    else
                        continue;

                }
            }
        }
    }
}