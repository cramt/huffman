using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huffman {
    [Serializable]
    class HuffmanNode : IComparable<HuffmanNode> {
        // For the character(s) that the node represents
        // For symbolet/symbolerne som noden repræsentere
        public string Symbol;
        // The amount of times the character as shown up in the text
        // Antalet af gange som symbolet opstår i teksten
        public uint Frequency;
        // The binary code that describes its location in the huffman tree
        // Den binære kode som beskriver noden lokation i huffman træet
        public BitArray Code;
        // Left child node of current node.
        // Venstre node for den nuværende node
        public HuffmanNode LeftChildNode;
        // Right child node of current node.
        // Højere node for den nuværende node
        public HuffmanNode RightChildNode;
        // Describes wether or not the node is the final node that descibes a single symbol
        // Beskriver om denne node er et blad eller en gren
        public bool IsLeaf;

        // The constructor for a HuffmanNode that is a leaf
        // contructor'en for huffman noden hvis det er et blad
        public HuffmanNode(string value) {
            // Setting the symbol
            // set symbolet
            Symbol = value;
            // This is creation of Node, so now its count is 1
            // set frekvensen, som er 1 fra starten.
            Frequency = 1;
            // It will be filled up when the tree is made
            // array'en for bitsne
            Code = new BitArray(new bool[] { });
            // Because it has a symbol and is therefore a leaf
            // dette er contructor'en for et blad, derfor er den true
            IsLeaf = true;
        }

        // The constructor for a HuffmanNode that is not a leaf
        // contructor'en for en huffman node hvis at det er en gren
        public HuffmanNode(HuffmanNode node1, HuffmanNode node2) {
            // Init the BitArray
            // array'en for bitsne
            Code = new BitArray(new bool[] { });
            // It has no symbol and is therefore not a leaf
            // dette er constructor'en for en gren, derfor er IsLeaf false
            IsLeaf = false;

            // The node of the highest frequency is placed right, and the symbols and frequncies
            // are added together as the symbol and frequency of this node
            // noden med den højeste frekvens bliver placeret til højre, og symbolerne og frekvenserne
            // bliver lagt sammen, og det samme sker for noden med den minste frekvens som bliver lagt til venstre
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
        // Denne funktion er fra interface'en IComparable<HuffmanNode>
        // den gør det muligt at sortere en liste eller en array af denne klasse
        // den sammenligner (og derfor også sortere) baserede på nodernes frekvens
        public int CompareTo(HuffmanNode otherNode) {
            return this.Frequency.CompareTo(otherNode.Frequency);
        }
    }
}