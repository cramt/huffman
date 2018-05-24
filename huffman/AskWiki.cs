using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace huffman {
    // this code is not relevant for the huffman encoding, this is an api for wikipedia, so that we can test the effectiveness of the huffman coding
    class AskWiki {
        public static async Task<List<WikiArticle>> Words(string[] words) {
            const string baseurl = "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro=&explaintext=&titles=";
            List<Task<List<WikiArticle>>> concurrency = new List<Task<List<WikiArticle>>>();
            for (int i = 0; i < words.Length; i++) {
                HttpClient client = new HttpClient();
                string url = baseurl + WebUtility.UrlEncode(words[i]);
                concurrency.Add(client.GetAsync(url).ContinueWith(x => {
                    List<WikiArticle> ret = new List<WikiArticle>();
                    try {
                        JObject obj = (JObject)JsonConvert.DeserializeObject(x.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        var pages = obj["query"]["pages"];
                        foreach (var jtoken in pages) {
                            foreach (var jtoken2 in jtoken) {
                                ret.Add(JsonConvert.DeserializeObject<WikiArticle>(jtoken2.ToString()));
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine(e);
                    }
                    return ret;
                }));
            }
            Task.WaitAll(concurrency.ToArray());
            List<WikiArticle> re = new List<WikiArticle>();
            foreach(var con in concurrency) {
                if(con == null) {
                    continue;
                }
                foreach(var c in con.Result) {
                    if(c.extract == "") {
                        continue;
                    }
                    re.Add(c);
                }
            }
            return re;
        }
    }
    class WikiArticle {
        public int pageid;
        public int ns;
        public string title;
        public string extract;
    }
}