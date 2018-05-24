using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace huffman {
    //This class describes a linked list of bits (ones and zeros or true and false)
    //It inheritances List<bool> because List<T> already has all the functionality of a linked list
    class BitList : List<bool> {
        //This static method parses a BitArray into a BitList
        public static BitList Parse(BitArray bitArray) {
            //create a new BitList
            BitList re = new BitList();
            //go thought the BitArray
            foreach(bool a in bitArray) {
                //add the individual bit
                re.Add(a);
            }
            //return the BitList
            return re;
        }
        //This method is the opposite of the one before, it converts the current BitList to a BitArray
        public BitArray ToBitArray() {
            //create a new BitArray based on List<boo>.ToArray(), which turns the linked list, 
            //into an array and return it
            return new BitArray(ToArray());
        }
        //This method adds a BitArray to the current BitList
        public void AddBitArray(BitArray bitArray) {
            //go thought the BitList
            foreach(bool b in bitArray) {
                //add the individual bit
                Add(b);
            }
        }
    }
}
