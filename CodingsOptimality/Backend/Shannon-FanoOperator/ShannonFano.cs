using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingsOptimality.Backend.Shannon_FanoOperator
{
    internal class ShannonFanoNode
    {
        public byte Symbol { get; set; }
        public double Probability { get; set; }
        public string Code { get; set; }
    }

    internal class ShannonFano
    {
        public List<ShannonFanoNode> Nodes { get; private set; }

        public ShannonFano(byte[] input)
        {
            Nodes = BuildFrequencyTable(input);
            BuildShannonFanoTree(Nodes);
        }

        private List<ShannonFanoNode> BuildFrequencyTable(byte[] input)
        {
            var frequencyTable = new Dictionary<byte, int>();

            foreach (byte symbol in input)
            {
                if (frequencyTable.ContainsKey(symbol))
                    frequencyTable[symbol]++;
                else
                    frequencyTable[symbol] = 1;
            }

            int totalSymbols = input.Length;
            var nodes = new List<ShannonFanoNode>();

            foreach (var kvp in frequencyTable)
            {
                nodes.Add(new ShannonFanoNode
                {
                    Symbol = kvp.Key,
                    Probability = (double)kvp.Value / totalSymbols,
                    Code = ""
                });
            }

            return nodes.OrderByDescending(node => node.Probability).ToList();
        }

        private void BuildShannonFanoTree(List<ShannonFanoNode> nodes)
        {
            if (nodes.Count == 1)
                return;

            int splitIndex = FindSplitIndex(nodes);

            for (int i = 0; i < nodes.Count; i++)
            {
                if (i < splitIndex)
                    nodes[i].Code += "0";
                else
                    nodes[i].Code += "1";
            }

            BuildShannonFanoTree(nodes.Take(splitIndex).ToList());
            BuildShannonFanoTree(nodes.Skip(splitIndex).ToList());
        }

        private int FindSplitIndex(List<ShannonFanoNode> nodes)
        {
            double totalProbability = nodes.Sum(node => node.Probability);
            double halfTotal = totalProbability / 2.0;

            double accumulatedProbability = 0.0;
            int splitIndex = 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                accumulatedProbability += nodes[i].Probability;
                if (accumulatedProbability >= halfTotal)
                {
                    splitIndex = i + 1;
                    break;
                }
            }

            return splitIndex;
        }

        public Dictionary<byte, string> GetCodeTable()
        {
            var codeTable = new Dictionary<byte, string>();
            foreach (var node in Nodes)
            {
                codeTable[node.Symbol] = node.Code;
            }
            return codeTable;
        }

        public BitArray Encode(byte[] input)
        {
            var codeTable = GetCodeTable();
            var bits = new List<bool>();

            foreach (byte symbol in input)
            {
                string code = codeTable[symbol];
                bits.AddRange(code.Select(c => c == '1'));
            }

            return new BitArray(bits.ToArray());
        }

        public byte[] Decode(BitArray encoded)
        {
            var reverseCodeTable = GetCodeTable().ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            var decodedBytes = new List<byte>();
            var currentCode = "";

            foreach (bool bit in encoded)
            {
                currentCode += bit ? '1' : '0';
                if (reverseCodeTable.ContainsKey(currentCode))
                {
                    decodedBytes.Add(reverseCodeTable[currentCode]);
                    currentCode = "";
                }
            }

            return decodedBytes.ToArray();
        }
    }
}
