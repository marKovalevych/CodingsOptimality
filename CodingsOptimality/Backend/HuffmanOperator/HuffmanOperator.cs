using System.Collections;
using System.Text;

namespace CodingsOptimality.Backend.HuffmanOperator
{
    internal class HuffmanOperator
    {
        #region fields

        private byte[]? _originalData;
        private BitArray _encodedData = new BitArray(0);
        private byte[] _decodedData = Array.Empty<byte>();
        private double _time;

        #endregion

        #region Properties

        public double Time => _time;
        public byte[] DecodedData => _decodedData;
        public int Size { get; private set; }

        #endregion

        #region Methods
        public HuffmanOperator(byte[] input)
        {
            _originalData = input.Clone() as byte[];
        }


        public bool Encode(ref string errorString)
        {
            if (_originalData == null)
            {
                errorString = "Input data is empty!";
                return false;
            }

            try
            {
                var timeStart = DateTime.Now;
                HuffmanTree huffmanTree = new HuffmanTree();

                huffmanTree.Build(_originalData);
                _encodedData = huffmanTree.Encode(_originalData);
                _decodedData = huffmanTree.Decode(_encodedData);
                Size = _encodedData.Length / 8;
                _time = (DateTime.Now - timeStart).Milliseconds;
                return true;
            }
            catch (Exception e)
            {
                errorString = e.Message;
                return false;
            }
        }

        #endregion




    }
}
