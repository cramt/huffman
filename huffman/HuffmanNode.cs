using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman {
    class HuffmanNode : IComparable<HuffmanNode> {
        public string Symbol;   // For the character of char value. Public because Process class use it.
        public int Frequency;          // Number of the count on file, string, text.
        public BitArray Code;            // Getting from a tree for making a huffman tree.
        public HuffmanNode ParentNode; // Parent Node of current Node.
        public HuffmanNode LeftTree;   // Left Node of current Node.
        public HuffmanNode RightTree;  // Right Node of current Node.
        public bool IsLeaf;            // Shows it is a leaf.


        public HuffmanNode(string value) {    // Creating a Node with given value(character).
            Symbol = value;     // Setting the symbol.
            Frequency = 1;      // This is creation of Node, so now its count is 1.

            RightTree = LeftTree = ParentNode = null;       // Does not have a left or right tree and a parent.

            Code = new BitArray(new bool[] { });          // It will be Assigned on the making Tree. Now it is empty.
            IsLeaf = true;      // Because all Node we create first does not have a parent Node.
        }


        public HuffmanNode(HuffmanNode node1, HuffmanNode node2) { // Join the 2 Node to make Node.

            // Firsly we are adding this 2 Nodes' variables. Except the new Node's left and right tree.
            Code = new BitArray(new bool[] { });
            IsLeaf = false;
            ParentNode = null;

            // Now the new Node need leaf. They are node1 and node2. if node1's frequency is bigger than or equal to node2's frequency. It is right tree. Otherwise left tree. The controllers are below:
            if (node1.Frequency >= node2.Frequency) {
                RightTree = node1;
                LeftTree = node2;
                RightTree.ParentNode = LeftTree.ParentNode = this;     // "this" means the new Node!
                Symbol = node1.Symbol + node2.Symbol;
                Frequency = node1.Frequency + node2.Frequency;
            }
            else if (node1.Frequency < node2.Frequency) {
                RightTree = node2;
                LeftTree = node1;
                LeftTree.ParentNode = RightTree.ParentNode = this;     // "this" means the new Node!
                Symbol = node2.Symbol + node1.Symbol;
                Frequency = node2.Frequency + node1.Frequency;
            }
        }


        public int CompareTo(HuffmanNode otherNode) { // We just override the CompareTo method. Because when we compare two Node, it must be according to frequencies of the Nodes.

            return this.Frequency.CompareTo(otherNode.Frequency);
        }


        public void FrequencyIncrease() {             // When facing a same value on the Node list, it is increasing the frequency of the Node.

            Frequency++;
        }
    }
}