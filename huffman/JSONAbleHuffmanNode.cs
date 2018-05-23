using System.Collections;
using System;
using Newtonsoft.Json;

namespace huffman {
    [Serializable]
    class JSONAbleHuffmanNode{
        public string symbol;
        public string code;
        public JSONAbleHuffmanNode leftTree;
        public JSONAbleHuffmanNode rightTree;
        public bool isLeaf;
        public static JSONAbleHuffmanNode Parse(HuffmanNode node) {
            if(node != null) {
                return new JSONAbleHuffmanNode(node);
            }
            return null;
        }
        private JSONAbleHuffmanNode(HuffmanNode node) {
            symbol = node.Symbol;
            code = node.Code.Print();
            leftTree = node == null ? null : JSONAbleHuffmanNode.Parse(node.LeftTree);
            rightTree = node == null ? null : JSONAbleHuffmanNode.Parse(node.RightTree);
            isLeaf = node.IsLeaf;
        }
        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}