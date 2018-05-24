using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace huffman {
    class Program {
        static void Main(string[] args) {
            Console.Title = "Huffman Code";
            string filename = @"E:\Libraries\Documents\Visual Studio 2017\Projects\huffman\huffman\lyrics.txt";
            string text = File.ReadAllText(filename);
            HuffmanTree tree = HuffmanTree.CreateFromText(text);
            byte[] encoded = tree.Encode(text);
            //Console.WriteLine(encoded);

            Console.WriteLine("text size: " + (text.Length * 8) + " bytes");
            Console.WriteLine("encoded size: " + (encoded.Length * 8) + " bytes");

            string decoded = tree.Decode(encoded);
            Console.WriteLine(decoded);
            
            
            Console.WriteLine("IT DONE");
            Thread.Sleep(-1);
        }
    }
}