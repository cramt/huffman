using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace huffman {
    [Serializable]
    class HuffmanTree {

        // This is a static property for the type of encoding that the text should use
        // The standart is UTF-8 because we are danish and æøå does not exist in ASCII
        public static Encoding EncodingType = Encoding.UTF8;
        
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
                //if it is a 1, take the right node
                if (bits[i]) {
                    currentNode = currentNode.RightChildNode;
                }
                //if it is a 0, take the left node
                else {
                    currentNode = currentNode.LeftChildNode;
                }
                // if the current node is a leaf
                if (currentNode.IsLeaf) {
                    // add the symbol to the str variable
                    str += currentNode.Symbol;
                    // go back to the root of the tree
                    currentNode = Root;
                }
            };
            // return the decoded symbols
            return str;
        }
        // this method creates a tree based on text
        public static HuffmanTree CreateFromText(string text) {
            // the list that we wanna fill up with nodes
            List<HuffmanNode> nodeList = new List<HuffmanNode>();
            // all the characters from the text
            char[] characters = text.ToCharArray();
            // loop thought the characters
            for (int i = 0; i < characters.Length; i++) {
                // the character as a string
                string read = characters[i].ToString();
                // has the node already been created?
                if (nodeList.Exists(x => x.Symbol == read)) {
                    // If is yes, find the index of the Node and increase the frequency of the Node.
                    nodeList[nodeList.FindIndex(y => y.Symbol == read)].Frequency++;
                }
                else {
                    // If is no, create a new node and add to the List of Nodes
                    nodeList.Add(new HuffmanNode(read));   
                }
            }
            // sort them, this is done based on frequency because of IComparable<HuffmanNode>.CompareTo
            nodeList.Sort();
            // loop thought them, until only one is left
            while (nodeList.Count > 1) {
                // Get the node of the first index of List, this is the one with the lowest frequency
                HuffmanNode node1 = nodeList[0];
                // and delete it.
                nodeList.RemoveAt(0);           
                // do the same thing again
                HuffmanNode node2 = nodeList[0];
                nodeList.RemoveAt(0);
                // make a parant node with node1 and node2 and the left and right child nodes
                nodeList.Add(new HuffmanNode(node1, node2));
                // and sort it again according to frequency.
                nodeList.Sort();        
            }
            // create a tree based on the remaining root node
            HuffmanTree tree = new HuffmanTree(nodeList[0]);
            // this is a recursive function to set the binary code of every leaf node
            void setCodeToTheTree(HuffmanNode Nodes, BitArray code = null) {
                // if the current code is not set, set it to an empty BitArray
                if (code == null) {
                    code = new BitArray(new bool[] { });
                }
                // if the code is empty do nothing
                if (Nodes == null) {
                    return;
                }
                // if there is no left node and right node, then set the code based on the current code
                if (Nodes.LeftChildNode == null && Nodes.RightChildNode == null) {
                    Nodes.Code = code;
                    return;
                }
                // create a bitlist for the left node
                BitList left = BitList.Parse(code);
                // add false for the left side
                left.Add(false);
                // call this function recursively, with the left bitlist and the left side node
                setCodeToTheTree(Nodes.LeftChildNode, left.ToBitArray());
                // create a bitlist for the right node
                BitList right = BitList.Parse(code);
                // add true for the right side
                right.Add(true);
                // call the function recursively, with the right bitlist and the right side node
                setCodeToTheTree(Nodes.RightChildNode, right.ToBitArray());
            }
            // call the recursive function
            setCodeToTheTree(tree.Root);
            // the tree
            return tree;
        }
        //this sets the color of the console to cyan
        private static void SetColor() {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        //this sets the color of the console to the default white
        private static void SetColorDefault() {
            Console.ForegroundColor = ConsoleColor.White;
        }


        // this is a recursive method to print the tree
        public void PrintTree(int level, HuffmanNode node = null) {
            // if node is null use the root
            if (node == null) {
                node = Root;
            }
            // print a tab for every level
            for (int i = 0; i < level; i++) {
                Console.Write("\t");
            }
            // print the symbol
            Console.Write("[" + node.Symbol + "]");
            // set color
            SetColor();
            // print the code
            Console.WriteLine("(" + node.Code.Print() + ")");
            // set the color back to default
            SetColorDefault();
            // call this method recursivly with the right and left node, and on more level
            PrintTree(level + 1, node.RightChildNode);
            PrintTree(level + 1, node.LeftChildNode);
        }


        // this is a recursive method to print the information, but not as a tree like the last one
        public void PrintInformation(HuffmanNode node = null) {
            // if node is null use the root
            if (node == null) {
                node = Root;
            }
            // print the symbol, code and frequency
            Console.WriteLine("Symbol : " + node.Symbol + " -  Code : " + node.Code.Print() + " - Frequency : " + node.Frequency);
            // if it is not a leaf call the function recursivly for left and right
            if (!node.IsLeaf) {
                PrintInformation(node.LeftChildNode);
                PrintInformation(node.RightChildNode);
            }
        }


        // this is a recursive method to print the information, but only the leafs
        public void PrintfLeafAndCodes(HuffmanNode node = null) {
            // if node is null use the root
            if (node == null) {
                node = Root;
            }
            // if there is no left and right node, print the symbol, code and frequency. Then return
            if (node.LeftChildNode == null && node.RightChildNode == null) {
                Console.WriteLine("Symbol : " + node.Symbol + " -  Code : " + node.Code.Print() + " - Frequency : " + node.Frequency);
                return;
            }
            // recursivly call this method for the left and right node
            PrintfLeafAndCodes(node.LeftChildNode);
            PrintfLeafAndCodes(node.RightChildNode);
        }
    }
}