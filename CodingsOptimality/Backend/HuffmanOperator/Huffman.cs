using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingsOptimality.Backend.HuffmanOperator
{
    internal class Huffman
    {
    }

    public class HuffmanNode
    {
        public byte Symbol { get; set; }
        public int Frequency { get; set; }
        public HuffmanNode Left { get; set; }
        public HuffmanNode Right { get; set; }

        public List<bool> Traverse(byte symbol, List<bool> data)
        {
            if (Right == null && Left == null)
            {
                return symbol.Equals(Symbol) ? data : null;
            }
            else
            {
                List<bool> leftPath = null;
                List<bool> rightPath = null;

                if (Left != null)
                {
                    List<bool> leftPrefix = new List<bool>();
                    leftPrefix.AddRange(data);
                    leftPrefix.Add(false);

                    leftPath = Left.Traverse(symbol, leftPrefix);
                }

                if (Right != null)
                {
                    List<bool> rightPrefix = new List<bool>();
                    rightPrefix.AddRange(data);
                    rightPrefix.Add(true);

                    rightPath = Right.Traverse(symbol, rightPrefix);
                }

                return leftPath ?? rightPath;
            }
        }
    }

    public class HuffmanTree
    {
        private List<HuffmanNode> nodes = new List<HuffmanNode>();
        public HuffmanNode Root { get; set; }
        public Dictionary<byte, int> Frequencies = new Dictionary<byte, int>();

        public void Build(byte[] source)
        {
            foreach (byte symbol in source)
            {
                if (!Frequencies.ContainsKey(symbol))
                {
                    Frequencies.Add(symbol, 0);
                }

                Frequencies[symbol]++;
            }

            foreach (KeyValuePair<byte, int> symbol in Frequencies)
            {
                nodes.Add(new HuffmanNode() { Symbol = symbol.Key, Frequency = symbol.Value });
            }

            while (nodes.Count > 1)
            {
                List<HuffmanNode> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList();

                if (orderedNodes.Count >= 2)
                {
                    // Take first two items
                    List<HuffmanNode> taken = orderedNodes.Take(2).ToList();

                    // Create a parent node by combining the frequencies
                    HuffmanNode parent = new HuffmanNode()
                    {
                        Symbol = (byte)'*',
                        Frequency = taken[0].Frequency + taken[1].Frequency,
                        Left = taken[0],
                        Right = taken[1]
                    };

                    nodes.Remove(taken[0]);
                    nodes.Remove(taken[1]);
                    nodes.Add(parent);
                }

                this.Root = nodes.FirstOrDefault();
            }
        }

        public BitArray Encode(byte[] source)
        {
            List<bool> encodedSource = new List<bool>();

            for (int i = 0; i < source.Length; i++)
            {
                List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
                encodedSource.AddRange(encodedSymbol);
            }

            BitArray bits = new BitArray(encodedSource.ToArray());

            return bits;
        }

        public byte[] Decode(BitArray bits)
        {
            HuffmanNode current = this.Root;
            List<byte> decoded = new List<byte>();

            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (current.Right != null)
                    {
                        current = current.Right;
                    }
                }
                else
                {
                    if (current.Left != null)
                    {
                        current = current.Left;
                    }
                }

                if (IsLeaf(current))
                {
                    decoded.Add(current.Symbol);
                    current = this.Root;
                }
            }

            return decoded.ToArray();
        }

        public bool IsLeaf(HuffmanNode node)
        {
            return (node.Left == null && node.Right == null);
        }
    }
}
