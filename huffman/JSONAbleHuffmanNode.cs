using System.Collections;
using System;
using Newtonsoft.Json;

namespace huffman {
    // This class is a json friendly version of the HuffmanNode
    // it is essentially the same, except for the code variable, which is a string of 1's and 0's now, not a bit array
    // this is because, when a bit array gets json serialized, it gets converted to an array of boolean values
    // 10101101111 as a bit array, look like this when json serialized [true, false, true, false, true, true, false, true, true, true, true]
    // this takes up a lot of space, and we therefore use a string instead
    [Serializable]
    class JSONAbleHuffmanNode{
        //the same variables as in huffman node
        public string symbol;
        // code is a string
        public string code;
        public JSONAbleHuffmanNode leftTree;
        public JSONAbleHuffmanNode rightTree;
        public bool isLeaf;
        //method to convert a huffman node to the json friendly type
        public static JSONAbleHuffmanNode Parse(HuffmanNode node) {
            //if not null, call the contructor
            if(node != null) {
                return new JSONAbleHuffmanNode(node);
            }
            // return null if the node is null
            return null;
        }
        // the private contructor for the class
        private JSONAbleHuffmanNode(HuffmanNode node) {
            // symbol is symbol, since a string is normal for json
            symbol = node.Symbol;
            // code is a string instead of an bit array, this is more json friendly
            code = node.Code.Print();
            // right and left tree, is converted recursively
            leftTree = Parse(node.LeftChildNode);
            rightTree = Parse(node.RightChildNode);
            //lead is just leaf
            isLeaf = node.IsLeaf;
        }
        // simple method that uses the Newtonsoft.Json package, to json convert this class
        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}