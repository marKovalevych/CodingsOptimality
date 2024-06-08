using CodingsOptimality.Backend.HuffmanOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingsOptimality.Backend.Shannon_FanoOperator
{
    internal class ShannonFanoOperator
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
        public ShannonFanoOperator(byte[] input)
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

                var shannonFano = new ShannonFano(_originalData);
                _encodedData = shannonFano.Encode(_originalData);
                _decodedData = shannonFano.Decode(_encodedData);
                Size = _encodedData.Length / 8;

                //var huffmanTreeBuilder = new HuffmanTreeBuilder();
                //var huffmanTree = huffmanTreeBuilder.CreateTable(_originalData);
                //var encoder = new HuffmanCodec(huffmanTree);
                //_encodedData = encoder.Encode(_originalData);
                //_decodedData = encoder.Decode(_encodedData);
                _time = (DateTime.Now - timeStart).Milliseconds;
                //CheckErrorsBeforeRS();
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
