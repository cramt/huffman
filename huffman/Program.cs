using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace huffman {
    class Program {
        static void Main(string[] args) {
            Console.Title = "Huffman Code";
            String filename = @"C:\Users\madsc\Documents\Visual Studio 2017\Projects\huffman\huffman\lyrics.txt";
            List<HuffmanNode> nodeList = ProcessMethods.GetListFromFile(filename);
            HuffmanTree tree = ProcessMethods.GetTreeFromList(nodeList);
            ProcessMethods.PrintfLeafAndCodes(tree.Root);
            Console.WriteLine(tree.JSONEncode());

            Console.WriteLine("IT DONE");
            Thread.Sleep(-1);
        }
    }
}