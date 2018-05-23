using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace huffman {
    static class MyExtensionMethods {
        public static string Print(this BitArray bitArray) {
            StringBuilder str = new StringBuilder();
            foreach (bool b in bitArray) {
                str.Append(b ? "1" : "0");
            }
            return str.ToString();
        }
    }
}
