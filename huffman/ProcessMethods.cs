using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Threading;

namespace huffman {
    static partial class ProcessMethods {
        public static void setColor() {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public static void setColorDefault() {
            Console.ForegroundColor = ConsoleColor.White;
        }
        //  Creates a Node List that reading the characters on the file.
        public static List<HuffmanNode> GetListFromFile(string filename) {
            List<HuffmanNode> nodeList = new List<HuffmanNode>();  // Node List.
            try {
                // Creating a new unique node that reading from the file.
                // If it is the same character, increase the frequency of the value. It is possiple with "frequencyIncreas()" method.
                FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                for (int i = 0; i < stream.Length; i++) {
                    string read = Convert.ToChar(stream.ReadByte()).ToString();
                    if (nodeList.Exists(x => x.Symbol == read)) // Checking the value that have you created before?
                        nodeList[nodeList.FindIndex(y => y.Symbol == read)].FrequencyIncrease(); // If is yes, find the index of the Node and increase the frequency of the Node.
                    else
                        nodeList.Add(new HuffmanNode(read));   // If is no, create a new node and add to the List of Nodes
                }
                nodeList.Sort();   // Sort the Nodes on the List according to their frequency value.
                return nodeList;
            }
            catch (Exception e) {
                Console.WriteLine(e);
                Thread.Sleep(-1);
                return null;
            }

        }


        //  Creates a Tree according to Nodes(frequency, symbol)
        public static HuffmanTree GetTreeFromList(List<HuffmanNode> nodeList) {
            while (nodeList.Count > 1) {  // 1 because a tree need 2 leaf to make a new parent.
                HuffmanNode node1 = nodeList[0];    // Get the node of the first index of List,
                nodeList.RemoveAt(0);               // and delete it.
                HuffmanNode node2 = nodeList[0];    // Get the node of the first index of List,
                nodeList.RemoveAt(0);               // and delete it.
                nodeList.Add(new HuffmanNode(node1, node2));    // Sending the constructor to make a new Node from this nodes.
                nodeList.Sort();        // and sort it again according to frequency.
            }
            HuffmanTree tree = new HuffmanTree(nodeList[0]);
            SetCodeToTheTree(tree.Root);
            return tree;
        }

        // Setting the codes of the nodes of tree. Recursive method.
        public static void SetCodeToTheTree(HuffmanNode Nodes, BitArray code = null) {
            if (code == null) {
                code = new BitArray(new bool[] { });
            }
            if (Nodes == null)
                return;
            if (Nodes.LeftTree == null && Nodes.RightTree == null) {
                Nodes.Code = code;
                return;
            }
            BitList left = BitList.Parse(code);
            left.Add(false);
            SetCodeToTheTree(Nodes.LeftTree, left.ToBitArray());
            BitList right = BitList.Parse(code);
            right.Add(true);
            SetCodeToTheTree(Nodes.RightTree, right.ToBitArray());
        }


        // Printing all Tree! Recursive method.
        public static void PrintTree(int level, HuffmanNode node) {
            if (node == null)
                return;
            for (int i = 0; i < level; i++) {
                Console.Write("\t");
            }
            Console.Write("[" + node.Symbol + "]");
            setColor();
            Console.WriteLine("(" + node.Code + ")");
            setColorDefault();
            PrintTree(level + 1, node.RightTree);
            PrintTree(level + 1, node.LeftTree);
        }


        //  Printing the Node's information on the nodeList
        public static void PrintInformation(List<HuffmanNode> nodeList) {
            foreach (var item in nodeList)
                Console.WriteLine("Symbol : {0} - Frequency : {1}", item.Symbol, item.Frequency);
        }


        // Printing the symbols and codes of the Nodes on the tree.
        public static void PrintfLeafAndCodes(HuffmanNode nodeList) {
            if (nodeList == null)
                return;
            if (nodeList.LeftTree == null && nodeList.RightTree == null) {
                Console.WriteLine("Symbol : " + nodeList.Symbol + " -  Code : " + nodeList.Code.Print() + " - Frequency : " + nodeList.Frequency);
                return;
            }
            PrintfLeafAndCodes(nodeList.LeftTree);
            PrintfLeafAndCodes(nodeList.RightTree);
        }
    }
}