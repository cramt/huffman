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
        // Dette er en statisk egenskab for den type enkoding teksten skal benytte
        // Standarden er UTF-8, fordi at vi er danske og 'æøå' findes ikke i ASCII
        public static Encoding EncodingType = Encoding.UTF8;

        // The "root" of the Huffman tree, which is the upmost node
        // It is read only for everything outside the class
        // "Roden" på Huffman-træet, hvilket er den øverste knudepunkt
        // Den er kun læst for alt uden for klassen
        public HuffmanNode Root { get; } = null;
        // The constructor sets the Root property, based on its argument
        // "Constructoren" sætter Roden, eller "Root", baseret på dets argument
        public HuffmanTree(HuffmanNode node) {
            Root = node;
        }
        // This method json serializes the tree
        // json is choosen since it is the easiest for other programs to deserialize
        // and for humans to read
        // I funktionen serialiserer json træet
        // json er valgt siden at det er det er nemt for andre programmer at læse eller "deserialisere"
        // Samtidigt er det også nemt for mennesker at læse
        public string JSONEncode() {
            return JSONAbleHuffmanNode.Parse(Root).Serialize();
        }
        // This method is to encode text based on the this tree
        // Denne funktion enkoder tekst baseret på det foregående træ
        public byte[] Encode(string text) {
            // a local recursive function to convert a character into an array of bits based on the root huffman node
            // En lokal rekursiv funktion for at konvertere et symbol om til et array af bits baseret på Hoffman-nodens rod
            BitArray convertToBitarray(HuffmanNode n, char c) {
                // if we found a leaf, return the code
                // Hvis der findes en 'gren', returnerer koden
                if (n.IsLeaf) {
                    return n.Code;
                }
                else {
                    // Call this function again, but with either the left or the right
                    // Kald denne funktion igen, men med enten højre eller venstre
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
            // Ny liste for bitsene
            BitList bits = new BitList();
            // make the text into a character array, and loop trough it
            // Lav teksten om til et symbol-array, og gentag igennem det
            text.ToCharArray().ToList().ForEach(x => {
                // Use the recursive function from before to calculate the bits
                // Add the bits to the list of bits
                // Der bruges den rekursive funktion fra før til at udregne bitsene
                // Tilføj bitsene til listen over bits
                bits.AddBitArray(convertToBitarray(Root, x));
            });
            // We add a 1 to the list
            // Der tilføjes et 1 til listen
            bits.Add(true);
            // We fill the list up with 0's, until it is divisible by 8
            // Listen fyldes op med 0'er, indtil at den kan divideres med 8
            while (bits.Count % 8 != 0) {
                bits.Add(false);
            }
            // Create the array of bytes
            // Der laves et array over bytes
            byte[] bytes = new byte[bits.Count / 8];
            // Copy the array of bits to the array of bytes
            // Listen over bits indsættes ind i listen over bytes
            bits.ToBitArray().CopyTo(bytes, 0);
            // Return the bytes
            // Returner bytes
            return bytes;
        }
        // This method decodes bytes into a string, based on the current huffman tree
        // Essentially, the opposite of the one above
        // Denne funktion afkoder bytes til en string, baseret på det nuværende Huffman-træ
        // Stort set bare det foregående baglæns
        public string Decode(byte[] encoded) {
            // Turn the bytes into bits, and make a list of them
            // Bytes omdannes til bits, og der laves en liste af dem
            BitList bits = BitList.Parse(new BitArray(encoded));
            // a recursive function to remove the ending of the bits
            // the ending is there, only to make sure that the length of the list is divisible by 8
            // this function therefore removes 0 from the end, until it hits a 1, which is then removed, and the function stops
            // En rekursiv funktion for at fjerne enderne på bits
            // Enderne er der kun for at sikre at længden på listen kan divideres med 8
            // Denne funktion fjerner derfor 0'er fra enden, indtil at den rammer et 1, hvilket derefter fjernes og funktionen stopper
            void clean(BitList list) {
                //if it is a 1
                // Hvis det er et 1
                if (list[list.Count - 1]) {
                    // remove the 1
                    // Fjern 1
                    list.RemoveAt(list.Count - 1);
                }
                // if it is a 0
                // Hvis det er et 0
                else {
                    // remove the 0
                    // Fjern 0
                    list.RemoveAt(list.Count - 1);
                    // repeat
                    // Gentag
                    clean(list);
                }
            }
            // call the function just described
            // Kald funktionen fra før
            clean(bits);
            // init the string that is going to hold the decoded string
            // strengen der skal indehold den afkodede streng laves
            string str = "";
            // define a variable to hold the current node that we are on
            // Der defineres en variabel til at holde det nuværende knudepunkt der kigges på
            HuffmanNode currentNode = Root;
            // loop thought all the bits
            // Kør igennem alle bits
            for (int i = 0; i < bits.Count; i++) {
                // if it is a 1, take the right node
                // Hvis det er et 1, tag højre knude
                if (bits[i]) {
                    currentNode = currentNode.RightChildNode;
                }
                // if it is a 0, take the left node
                // Hvis det er et 0, tag venstre knude
                else {
                    currentNode = currentNode.LeftChildNode;
                }
                // if the current node is a leaf
                // Hvis den nuværende knude er et blad
                if (currentNode.IsLeaf) {
                    // add the symbol to the str variable
                    // Tilføj symbolet til streng-variablen "str"
                    str += currentNode.Symbol;
                    // go back to the root of the tree
                    // Gå tilbage til træets rod
                    currentNode = Root;
                }
            };
            // return the decoded symbols
            // Returner de afkodede symboler
            return str;
        }
        // this method creates a tree based on text
        // Denne funktion laver et træ baseret på tekst
        public static HuffmanTree CreateFromText(string text) {
            // the list that we wanna fill up with nodes
            // Listen der skal fyldes med noder
            List<HuffmanNode> nodeList = new List<HuffmanNode>();
            // all the characters from the text
            // Alle symbolerne fra teksten
            char[] characters = text.ToCharArray();
            // loop thought the characters
            // Gentag igennem symbolerne
            for (int i = 0; i < characters.Length; i++) {
                // the character as a string
                // Symbolerne som strenge
                string read = characters[i].ToString();
                // has the node already been created?
                // Er knuden allerede blevet skabt?
                if (nodeList.Exists(x => x.Symbol == read)) {
                    // If is yes, find the index of the Node and increase the frequency of the Node.
                    // Hvis ja, find indekset for knuden og øg frekvensen for knuden
                    nodeList[nodeList.FindIndex(y => y.Symbol == read)].Frequency++;
                }
                else {
                    // If is no, create a new node and add to the List of Nodes
                    // Hvis nej, skab en ny knude og tilføj den til listen over knuder
                    nodeList.Add(new HuffmanNode(read));
                }
            }
            // sort them, this is done based on frequency because of IComparable<HuffmanNode>.CompareTo
            // Sorter dem, dette gøres baseret på frekvens fordi at IComparable<HuffmanNode>.CompareTo
            nodeList.Sort();
            // loop thought them, until only one is left
            // Kør igennem dem alle sammen indtil der kun er en tilbage
            while (nodeList.Count > 1) {
                // Get the node of the first index of List, this is the one with the lowest frequency
                // Få knuden for det første indeks af Listen, dette er den med den laveste frekvens
                HuffmanNode node1 = nodeList[0];
                // and delete it.
                // Fjern den
                nodeList.RemoveAt(0);
                // do the same thing again
                // Gør det samme igen
                HuffmanNode node2 = nodeList[0];
                nodeList.RemoveAt(0);
                // make a parant node with node1 and node2 and the left and right child nodes
                // Lav en parent-knude med node1 og node1 og de venstre og højre child-knuder
                nodeList.Add(new HuffmanNode(node1, node2));
                // and sort it again according to frequency.
                // Og sorter det igen efter frekvens
                nodeList.Sort();
            }
            // create a tree based on the remaining root node
            // Lav et træ baseret på den tilbageværende rod-knude
            HuffmanTree tree = new HuffmanTree(nodeList[0]);
            // this is a recursive function to set the binary code of every leaf node
            // Dette er en rekursiv funktion for at sætte den binære værdi for hvert blad
            void setCodeToTheTree(HuffmanNode Nodes, BitArray code = null) {
                // if the current code is not set, set it to an empty BitArray
                // Hvis den nuværende kode ikke er sat, sættes den til et tomt BitArray
                if (code == null) {
                    code = new BitArray(new bool[] { });
                }
                // if the code is empty do nothing
                // Hvis koden er tom, gør intet
                if (Nodes == null) {
                    return;
                }
                // if there is no left node and right node, then set the code based on the current code
                // Hvis der ikke er nogen venstre knude, sæt koden baseret på den nuværende kode
                if (Nodes.LeftChildNode == null && Nodes.RightChildNode == null) {
                    Nodes.Code = code;
                    return;
                }
                // create a bitlist for the left node
                // lav en bitliste for den venstre knude
                BitList left = BitList.Parse(code);
                // add false for the left side
                // tilføj false for den venstre side
                left.Add(false);
                // call this function recursively, with the left bitlist and the left side node
                // Kald denne funktion rekursivt, med den venstre bitliste og den venstre knude
                setCodeToTheTree(Nodes.LeftChildNode, left.ToBitArray());
                // create a bitlist for the right node
                // Lav en bitliste for den højre knude
                BitList right = BitList.Parse(code);
                // add true for the right side
                // tilføj true for den højre side
                right.Add(true);
                // call the function recursively, with the right bitlist and the right side node
                // Kald denne funktion rekursivt, med den højre bitliste og den højre knude
                setCodeToTheTree(Nodes.RightChildNode, right.ToBitArray());
            }
            // call the recursive function
            // Kald den rekursive funktion
            setCodeToTheTree(tree.Root);
            // the tree
            // Træet returneres
            return tree;
        }
        //this sets the color of the console to cyan
        // Dette sætter farven på konsolen til turkis
        private static void SetColor() {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
        //this sets the color of the console to the default white
        // Dette sætter farven på konsolen til den standarde hvid
        private static void SetColorDefault() {
            Console.ForegroundColor = ConsoleColor.White;
        }


        // this is a recursive method to print the tree
        // Dette er en rekursiv funktion for at udskrive træet
        public void PrintTree(int level, HuffmanNode node = null) {
            // if node is null use the root
            // Hvis knuden er null benyttes roden
            if (node == null) {
                node = Root;
            }
            // print a tab for every level
            // Print en række for hvert niveau
            for (int i = 0; i < level; i++) {
                Console.Write("\t");
            }
            // print the symbol
            // Udskriv symbolet
            Console.Write("[" + node.Symbol + "]");
            // set color
            // Sæt farve
            SetColor();
            // print the code
            // Udskriv koden
            Console.WriteLine("(" + node.Code.Print() + ")");
            // set the color back to default
            // Sæt farven tilbage til normal
            SetColorDefault();
            // call this method recursivly with the right and left node, and one more level
            // Kald denne funktion rekursivt med den højre og venstre knude, og endnu et niveau
            PrintTree(level + 1, node.RightChildNode);
            PrintTree(level + 1, node.LeftChildNode);
        }


        // this is a recursive method to print the information, but not as a tree like the last one
        // Dette er en rekursiv funktion for at udskrive den information, men ikke som et træ ligesom før
        public void PrintInformation(HuffmanNode node = null) {
            // if node is null use the root
            // Hvis knuden er null benyttes roden
            if (node == null) {
                node = Root;
            }
            // print the symbol, code and frequency
            // Udskriv symbolet, koden og frekvensen
            Console.WriteLine("Symbol : " + node.Symbol + " -  Code : " + node.Code.Print() + " - Frequency : " + node.Frequency);
            // if it is not a leaf call the function recursivly for left and right
            // Hvis det ikker er et blad kaldes den funktion rekursivt for venstre og højre side
            if (!node.IsLeaf) {
                PrintInformation(node.LeftChildNode);
                PrintInformation(node.RightChildNode);
            }
        }


        // this is a recursive method to print the information, but only the leafs
        // Dette er en rekursiv funktion for at udskrive den information, men kun de venstre blade
        public void PrintfLeafAndCodes(HuffmanNode node = null) {
            // if node is null use the root
            // Hvis knuden er null benyttes roden
            if (node == null) {
                node = Root;
            }
            // if there is no left and right node, print the symbol, code and frequency. Then return
            // Hvis der ikke er nogen venstre knude, udskriv symbolet, koden og frekvensen. Derefter returner
            if (node.LeftChildNode == null && node.RightChildNode == null) {
                Console.WriteLine("Symbol : " + node.Symbol + " -  Code : " + node.Code.Print() + " - Frequency : " + node.Frequency);
                return;
            }
            // recursivly call this method for the left and right node
            // Rekursivt kald denne funktion for den venstre og højre knude
            PrintfLeafAndCodes(node.LeftChildNode);
            PrintfLeafAndCodes(node.RightChildNode);
        }
    }
}