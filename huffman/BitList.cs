using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace huffman {
    class BitList : List<bool> {
        public static BitList Parse(BitArray bitArray) {
            BitList re = new BitList();
            foreach(bool a in bitArray) {
                re.Add(a);
            }
            return re;
        }
        public BitArray ToBitArray() {
            return new BitArray(ToArray());
        }
    }
}
