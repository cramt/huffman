using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace huffman {
    static class MyExtensionMethods {
        // this is an extension method for BitArray
        // this means it can be called like this {bitArray.Print()}
        // it uses its BitArray, to make a string of 1's and 0's

        // dette er en udvidelses funktion for BitArray
        // dette betyder at den kan blive kaldet sådan her {bitArray.Print()}
        // den bruger bitArray'en til at lave en tekststren ud af 1'ere og 0'ere
        public static string Print(this BitArray bitArray) {
            //create the StringBuilder to hold the string that is going to be returned
            //lav en StringBuilder to at gemme strengen som bliver returnered til sidst
            StringBuilder str = new StringBuilder();
            // loop thought the BitArray
            // loop igennem BitArray'en
            foreach (bool b in bitArray) {
                //if b is 1, add "1" otherwise add "0"
                //hvis b er 1, lig "1" til, ellers lig "0" til
                str.Append(b ? "1" : "0");
            }
            // return the string builder as a string
            // returner tekststrengen
            return str.ToString();
        }
    }
}
