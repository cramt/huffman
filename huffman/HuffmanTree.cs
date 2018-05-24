using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace huffman {
    class HuffmanTree {
        // The "root" of the Huffman tree, which is the upmost node
        // It is read only for everything outside the class
        public HuffmanNode Root { get; } = null;
        // The constructor sets the Root property, based on its argument
        public HuffmanTree(HuffmanNode node) {
            Root = node;
        }
        // This method json serialize the tree
        // json is choosen since it is the easiest for other programs to deserialize
        // and for humans to read
        public string JSONEncode() {
            return JSONAbleHuffmanNode.Parse(Root).Serialize();
        }
        // This method is to encode text based on the this tree
        public byte[] Encode(string text) {
            // a local recursive function to convert a character into an array of bits based on the root huffman node
            BitArray convertToBitarray(HuffmanNode n, char c) {
                // if we found a leaf, return the code
                if (n.IsLeaf) {
                    return n.Code;
                }
                else {
                    // Call this function again, but with either the left or the right
                    if (n.LeftChildNode.Symbol.Contains(c)) {
                        return convertToBitarray(n.LeftChildNode, c);
                    }
                    else if (n.RightChildNode.Symbol.Contains(c)) {
                        return convertToBitarray(n.RightChildNode, c);
                    }
                    else {
                        throw new Exception("something went wrong when converting the tree");
                    }
                }
            }
            // Init list for the bits
            BitList bits = new BitList();
            // make the text into a character array, and loop tought it
            text.ToCharArray().ToList().ForEach(x => {
                // Use the recursive function from before to calculate the bits
                // Add the bits to the list of bits
                bits.AddBitArray(convertToBitarray(Root, x));
            });
            // We add a 1 to the list
            bits.Add(true);
            // We fill the list up with 0's, until it is divisible by 8
            while (bits.Count % 8 != 0) {
                bits.Add(false);
            }
            // Create the array of bytes
            byte[] bytes = new byte[bits.Count / 8];
            // Copy the array of bits to the array of bytes
            bits.ToBitArray().CopyTo(bytes, 0);
            // Return the bytes
            return bytes;
        }
        // This method decodes bytes into a string, based on the current huffman tree
        // Essentially, the opposite of the one above
        public string Decode(byte[] encoded) {
            // Turn the bytes into bits, and make a list of them
            BitList bits = BitList.Parse(new BitArray(encoded));
            // a recursive function to remove the ending of the bits
            // the ending is there, only to make sure that the length of the list is divisible by 8
            // this function therefore removes 0 from the end, until it hits a 1, which is then removed, and the function stops
            void clean(BitList list) {
                //if it is a 1
                if (list[list.Count - 1]) {
                    //remove the 1
                    list.RemoveAt(list.Count - 1);
                }
                // if it is a 0
                else {
                    //remove the 0
                    list.RemoveAt(list.Count - 1);
                    //repeat
                    clean(list);
                }
            }
            //call the function just described
            clean(bits);
            //init the string that is going to hold the decoded string
            string str = "";
            //define a variable to hold the current node that we are on
            HuffmanNode currentNode = Root;
            //loop thought all the bits
            for (int i = 0; i < bits.Count; i++) {
                // if the current node is a leaf
                if (currentNode.IsLeaf) {
                    // add the symbol to the str variable
                    str += currentNode.Symbol;
                    // go back to the root of the tree
                    currentNode = Root;
                }
                //if it is a 1, take the right node
                if (bits[i]) {
                    currentNode = currentNode.RightChildNode;
                }
                //if it is a 0, take the left node
                else {
                    currentNode = currentNode.LeftChildNode;
                }
            };
            // return the decoded symbols
            return str;
        }
        // this method creates a tree based on text
        public static HuffmanTree CreateFromText(string text) {
            return GetTreeFromList(GetListFromText(text));
        }
        //this sets the color of the console to cyan
        private static void SetColor() {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        //this sets the color of the console to the default white
        private static void SetColorDefault() {
            Console.ForegroundColor = ConsoleColor.White;
        }
        // Creates a Node List that reading the characters on the file.
        private static List<HuffmanNode> GetListFromText(string text) {
            List<HuffmanNode> nodeList = new List<HuffmanNode>();  // Node List.
            char[] characters = text.ToCharArray();
            for (int i = 0; i < characters.Length; i++) {
                string read = characters[i].ToString();
                // Checking the value that have you created before?
                if (nodeList.Exists(x => x.Symbol == read)) {
                    // If is yes, find the index of the Node and increase the frequency of the Node.
                    nodeList[nodeList.FindIndex(y => y.Symbol == read)].Frequency++;
                }
                else {
                    nodeList.Add(new HuffmanNode(read));   // If is no, create a new node and add to the List of Nodes
                }
            }
            nodeList.Sort();
            return nodeList;
        }


        //  Creates a Tree according to Nodes(frequency, symbol)
        private static HuffmanTree GetTreeFromList(List<HuffmanNode> nodeList) {
            while (nodeList.Count > 1) {  // 1 because a tree need 2 leaf to make a new parent.
                HuffmanNode node1 = nodeList[0];    // Get the node of the first index of List,
                nodeList.RemoveAt(0);               // and delete it.
                HuffmanNode node2 = nodeList[0];    // Get the node of the first index of List,
                nodeList.RemoveAt(0);               // and delete it.
                nodeList.Add(new HuffmanNode(node1, node2));    // Sending the constructor to make a new Node from this nodes.
                nodeList.Sort();        // and sort it again according to frequency.
            }
            HuffmanTree tree = new HuffmanTree(nodeList[0]);
            void SetCodeToTheTree(HuffmanNode Nodes, BitArray code = null) {
                if (code == null) {
                    code = new BitArray(new bool[] { });
                }
                if (Nodes == null)
                    return;
                if (Nodes.LeftChildNode == null && Nodes.RightChildNode == null) {
                    Nodes.Code = code;
                    return;
                }
                BitList left = BitList.Parse(code);
                left.Add(false);
                SetCodeToTheTree(Nodes.LeftChildNode, left.ToBitArray());
                BitList right = BitList.Parse(code);
                right.Add(true);
                SetCodeToTheTree(Nodes.RightChildNode, right.ToBitArray());
            }
            SetCodeToTheTree(tree.Root);
            return tree;
        }


        // Printing all Tree! Recursive method.
        public void PrintTree(int level, HuffmanNode node = null) {
            if (node == null) {
                node = Root;
            }
            for (int i = 0; i < level; i++) {
                Console.Write("\t");
            }
            Console.Write("[" + node.Symbol + "]");
            SetColor();
            Console.WriteLine("(" + node.Code + ")");
            SetColorDefault();
            PrintTree(level + 1, node.RightChildNode);
            PrintTree(level + 1, node.LeftChildNode);
        }


        //  Printing the Node's information on the nodeList
        public void PrintInformation(HuffmanNode node = null) {
            if (node == null) {
                node = Root;
            }
            Console.WriteLine("Symbol : {0} - Frequency : {1}", node.Symbol, node.Frequency);
            if (!node.IsLeaf) {
                PrintInformation(node.LeftChildNode);
                PrintInformation(node.RightChildNode);
            }
        }


        // Printing the symbols and codes of the Nodes on the tree.
        public void PrintfLeafAndCodes(HuffmanNode node = null) {
            if (node == null) {
                node = Root;
            }
            if (node.LeftChildNode == null && node.RightChildNode == null) {
                Console.WriteLine("Symbol : " + node.Symbol + " -  Code : " + node.Code.Print() + " - Frequency : " + node.Frequency);
                return;
            }
            PrintfLeafAndCodes(node.LeftChildNode);
            PrintfLeafAndCodes(node.RightChildNode);
        }
    }
}