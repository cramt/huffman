namespace huffman {
    class HuffmanTree {
        private HuffmanNode root = null;
        public HuffmanNode Root { get => root; }
        public HuffmanTree(HuffmanNode node) {
            root = node;
        }
        public string JSONEncode() {
            return JSONAbleHuffmanNode.Parse(Root).Serialize();
        }
    }
}