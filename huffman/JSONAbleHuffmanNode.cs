using System.Collections;
using System;
using Newtonsoft.Json;

namespace huffman {
    // This class is a json friendly version of the HuffmanNode
    // it is essentially the same, except for the code variable, which is a string of 1's and 0's now, not a bit array
    // this is because, when a BitArray gets json serialized, it gets converted to an array of boolean values
    // 10101101111 as a BitArray, would look like this when json serialized [true, false, true, false, true, true, false, true, true, true, true]
    // this takes up a lot of space, and we therefore use a string instead
    // denne klasse er en json venlig version af HuffmanNode
    // det er næsten det samme, denne bruger bare en tekststreng som 1'ere og 0'ere til at beskrive code
    // dette er fordi at når en BitArray bliver json serialiserede, så bliver den konveterede til en array af boolean'er
    // 10101101111 som en BitArray, ville se sådan her ud når det er json serialiserede [true, false, true, false, true, true, false, true, true, true, true]
    // dette bruger meget plads, og derfor bruger vi en tekststreng i stedet for en BitArray til json serialisering
    [Serializable]
    class JSONAbleHuffmanNode{
        //the same variables as in HuffmanNode
        // samme variabler som i HuffmanNode
        public string symbol;
        // code is a string
        // code er en tekststreng
        public string code;
        public JSONAbleHuffmanNode leftTree;
        public JSONAbleHuffmanNode rightTree;
        public bool isLeaf;
        //method to convert a HuffmanNode to the json friendly type
        //funktion til at konvertere en HuffmanNode til den json venlige type
        public static JSONAbleHuffmanNode Parse(HuffmanNode node) {
            //if not null, call the contructor
            //hvis den ikke er null, brug contructoren 
            if(node != null) {
                return new JSONAbleHuffmanNode(node);
            }
            // return null if the node is null
            // returner null hvis at noden er null
            return null;
        }
        // the private contructor for the class
        // den private contructor for denne klasse
        private JSONAbleHuffmanNode(HuffmanNode node) {
            // symbol is symbol, since a string is normal for json
            // symbol er symbol, eftersom at en tekststreng er normalt for json
            symbol = node.Symbol;
            // code is a string instead of an bit array, this is more json friendly
            // code bliver konvertede til en tekststren for at være json venlig
            code = node.Code.Print();
            // right and left tree, is converted recursively
            // højere og venstre node er konverteret rekursivt
            leftTree = Parse(node.LeftChildNode);
            rightTree = Parse(node.RightChildNode);
            //leaf is just leaf
            //et blad er et blad
            isLeaf = node.IsLeaf;
        }
        // simple method that uses the Newtonsoft.Json package, to json convert this class
        // en simple funktion der bruger Newtonsoft.Json pakken til at konvertere til json
        public string Serialize() {
            return JsonConvert.SerializeObject(this);
        }
    }
}