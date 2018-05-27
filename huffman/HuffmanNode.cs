using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman {
    [Serializable]
    class HuffmanNode : IComparable<HuffmanNode> {
        // For the character that the node represents
        public string Symbol;
        // The amount of times the character as shown up in the text
        public uint Frequency;
        // The binary code that describes its location in the huffman tree
        public BitArray Code;
        // Left child node of current node.
        public HuffmanNode LeftChildNode;
        // Right child node of current node.
        public HuffmanNode RightChildNode;
        // Describes wether or not the node is the final node that descibes a single symbol
        public bool IsLeaf;

        // The constructor for a HuffmanNode that is a leaf
        public HuffmanNode(string value) {
            // Setting the symbol
            Symbol = value;
            // This is creation of Node, so now its count is 1
            Frequency = 1;
            // It will be filled up when the tree is made
            Code = new BitArray(new bool[] { });
            // Because it has a symbol and is therefore a leaf
            IsLeaf = true;
        }

        // The constructor for a HuffmanNode that is not a leaf
        public HuffmanNode(HuffmanNode node1, HuffmanNode node2) {
            // Init the BitArray
            Code = new BitArray(new bool[] { });
            // It has no symbol and is therefore not a leaf
            IsLeaf = false;

            // The node of the highest frequency is placed right, and the symbols and frequncies
            // are added together as the symbol and frequency of this node
            if (node1.Frequency > node2.Frequency) {
                RightChildNode = node1;
                LeftChildNode = node2;
                Symbol = node1.Symbol + node2.Symbol;
                Frequency = node1.Frequency + node2.Frequency;
            }
            else {
                RightChildNode = node2;
                LeftChildNode = node1;
                Symbol = node2.Symbol + node1.Symbol;
                Frequency = node2.Frequency + node1.Frequency;
            }
        }

        // This method is from the IComparable<HuffmanNode> interface
        // It makes sure that when we sort a list or array of this class,
        // it will sort it based on frequency
        public int CompareTo(HuffmanNode otherNode) {
            return this.Frequency.CompareTo(otherNode.Frequency);
        }
    }
}