using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;


namespace huffman {
    class Program {
        public const string OUTPUT_DIR = "output";
        static void Main(string[] args) {
            HuffmanTree.EncodingType = Encoding.UTF8;
            List<Article> articles = new List<Article> {
                new Article() {
                    title = "den danske grundlov",
                    extract = SomeRandomText.DenDanskeGrundlov
                },
                new Article() {
                    title = "mini srp infohæfte 2018",
                    extract = File.ReadAllText(@"E:\Libraries\Documents\Visual Studio 2017\Projects\huffman\huffman\MiniSRPinfohæfte2018.txt")
                },
                new Article() {
                    title = "euklids elementer",
                    extract = File.ReadAllText(@"E:\Libraries\Documents\Visual Studio 2017\Projects\huffman\huffman\EuklidsElementer.txt")
                }
            };
            if (Directory.Exists(OUTPUT_DIR)) {
                Directory.Delete(OUTPUT_DIR, true);
            }
            var outputDir = Directory.CreateDirectory(OUTPUT_DIR);
            foreach (var article in articles) {
                var tree = HuffmanTree.CreateFromText(article.extract);
                var subDir = outputDir.CreateSubdirectory(article.title);
                var treeFilePath = Path.Combine(subDir.FullName, "tree.json");
                File.Create(treeFilePath).Close();
                File.WriteAllText(treeFilePath, tree.JSONEncode());

                var originalFilePath = Path.Combine(subDir.FullName, "original.txt");
                File.Create(originalFilePath).Close();
                File.WriteAllText(originalFilePath, article.extract);

                var encodedFilePath = Path.Combine(subDir.FullName, "encoded.txt");
                File.Create(encodedFilePath).Close();
                var encodedData = tree.Encode(article.extract);
                File.WriteAllBytes(encodedFilePath, encodedData);

                var encodedReadAbleFilePath = Path.Combine(subDir.FullName, "encoded_readable.txt");
                File.Create(encodedReadAbleFilePath).Close();
                File.WriteAllText(encodedReadAbleFilePath, (new BitArray(encodedData)).Print());

                var decodedFilePath = Path.Combine(subDir.FullName, "decoded.txt");
                File.Create(decodedFilePath).Close();
                var decodedData = tree.Decode(encodedData);
                File.WriteAllText(decodedFilePath, decodedData);

                var statsFilePath = Path.Combine(subDir.FullName, "stats.txt");
                File.Create(statsFilePath).Close();
                File.WriteAllLines(statsFilePath, new string[]{
                    "original size: " + (article.extract.Length * 8) + " b",
                    "compressed size: " + encodedData.Length + " b",
                    "compression percentage: " + (100*((double)encodedData.Length/((double)HuffmanTree.EncodingType.GetBytes(article.extract).Length))) + "%",
                    "tree size as json: " + (HuffmanTree.EncodingType.GetBytes(tree.JSONEncode()).Length) + " b",
                    "tree size as bytes: " + (new Func<string>(() => {
                        byte[] bytes;
                        IFormatter formatter = new BinaryFormatter();
                        using (MemoryStream stream = new MemoryStream()){
                            formatter.Serialize(stream, tree);
                            bytes = stream.ToArray();
                        }
                        return "" + bytes.Length;
                    }))() + " b",
                    "lossness: " + (article.extract == decodedData ? "yes" : (new Func<string>(()=>{
                        string re = "no" + Environment.NewLine;
                        if(article.extract.Length == decodedData.Length) {
                            List<string> errors = new List<string>();
                            for(int i = 0; i < article.extract.Length; i++) {
                                if(article.extract[i] != decodedData[i]) {
                                    errors.Add("position " + i + " does not match; original: " + article.extract[i].ToString() + ", decoded: " + decodedData[i].ToString());
                                }
                            }
                            re += String.Join(Environment.NewLine, errors.ToArray());
                        }
                        else {
                            re += "not the same length; original: " + article.extract.Length + ", decoded: " +decodedData.Length;
                        }
                        return re;
                    }))())
                });
            }
            Console.WriteLine("Done");
            Thread.Sleep(-1);
        }
    }
}