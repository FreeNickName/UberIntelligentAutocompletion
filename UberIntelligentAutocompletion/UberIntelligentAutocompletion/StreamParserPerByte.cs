using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberIntelligentAutocompletion
{
    class StreamParserPerByte
    {
        public static void Parse(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.ASCII))
            {
                var timer = new Stopwatch();
                timer.Start();
                if (reader.EndOfStream)
                {
                    return;
                }

                var dictionaryCount = int.Parse(reader.ReadLine());

                var hashSetRoot = new Dictionary<char, HashSetTreeNode>();
                
                var newLine = Encoding.ASCII.GetChars(Encoding.ASCII.GetBytes(Environment.NewLine));
                while (--dictionaryCount > 0)
                {
                    var line = reader.ReadLine();
                    var hashSet = hashSetRoot;
                    HashSetTreeNode node = null;

                    bool isNew = false;
                    var tokens = line.Split(' ');
                    var word = tokens[0];
                    var count = int.Parse(tokens[1]);

                    for (int i = 0; i < word.Length - 1; ++i)
                    {
                        var symbol = word[i];
                        if (/*newLine.Contains(symbol) ||*/ symbol == ' ')
                        {
                            node.IsEnd = true;
                            break;
                        }

                        if (isNew || !hashSet.TryGetValue(symbol, out node))
                        {
                            hashSet.Add(symbol, node = new HashSetTreeNode(symbol, count));
                            isNew = true;
                        }

                        hashSet = node.Next;
                    }

                    //var symbol = (char)reader.Read();
                    //if (newLine.Contains(symbol) || char.IsWhiteSpace(symbol))
                    //{
                    //    continue;
                    //}

                    //var newSet = new Dictionary<char, object>();
                    //hashSet.Add(symbol, newSet);
                    //hashSet = newSet;
                }

                timer.Stop();

                Console.WriteLine(timer.ElapsedMilliseconds);
                timer.Restart();

                using (var outputStream = new StreamWriter(@"C:\Users\DJiN\Downloads\Тестовое задание C# разработчик\test.out"))
                {

                    var wordsCount = int.Parse(reader.ReadLine());

                    while (--wordsCount > 0)
                    {
                        var line = reader.ReadLine();
                        var hashSet = hashSetRoot;
                        HashSetTreeNode node = null;

                        for (int i = 0; i < line.Length - 1; ++i)
                        {
                            var symbol = line[i];

                            if (!hashSet.TryGetValue(symbol, out node))
                            {
                                break;
                            }
                            hashSet = node.Next;
                        }

                        //node.Count;

                        var bla = node.Next.OrderByDescending(o => o.Value.Count).Take(10).ToArray();
                        if (!node.IsEnd || bla.Min(o => o.Value.Count) > node.Count)
                        {
                            foreach(var l in bla)
                            {
                                outputStream.WriteLine(l);
                            }
                            outputStream.WriteLine();
                            continue;
                        }

                        var result = new string[10];
                        var max = 10;
                        for (int i = 0; i < max; ++i)
                        {
                            if (bla[i].Value.Count > node.Count)
                            {
                                result[i] = "";
                            }
                            else
                            if (bla[i].Value.Count == node.Count)
                            {
                                result[i] = "";
                            }
                            else
                            {
                                result[i] = node.ToString();
                                ++i;
                                --max;
                            }
                        }
                    }
                }


                //using (var outputStream = new StreamWriter(@"C:\Users\DJiN\Downloads\Тестовое задание C# разработчик\test.out"))
                //{
                //    var result = "";
                //    GetLine(hashSetRoot, result, outputStream);
                //}



            }
            Console.ReadKey();
        }

        static void GetLine(IDictionary<char, HashSetTreeNode> value, string result, StreamWriter output)
        {
            foreach (var node in value)
            {
                if (node.Value.IsEnd)
                {
                    output.WriteLine(result + node.Key);
                    //continue;
                }
                if (node.Value.Next.Count == 0)
                {
                    //    output.WriteLine(result);
                    //    return;
                    continue;
                }
                var newResult = (string)result.Clone() + node.Key;

                GetLine(node.Value.Next, newResult, output);
            }
        }
    }
}
