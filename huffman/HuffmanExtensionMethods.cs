using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace huffman {
    static class MyExtensionMethods {
        // this is an extension method for BitArray
        // this means it can be called like this {bitArray.Print()}
        // it uses its BitArray, to make a string of 1's and 0's
        public static string Print(this BitArray bitArray) {
            //create the StringBuilder to hold the string that is going to be returned
            StringBuilder str = new StringBuilder();
            // loop thought the BitArray
            foreach (bool b in bitArray) {
                //if b is 1, add "1" otherwise add "0"
                str.Append(b ? "1" : "0");
            }
            // return the string builder as a string
            return str.ToString();
        }
    }
}
