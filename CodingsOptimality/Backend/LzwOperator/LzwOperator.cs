using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdNet;

namespace CodingsOptimality.Backend.LzwOperator
{
    internal class LzwOperator
    {
        #region fields

        private byte[]? _originalData;
        private List<int> _encodedData = new List<int>();
        private byte[] _decodedData = Array.Empty<byte>();
        private double _time;

        #endregion

        #region Properties

        public double Time => _time;
        
        public byte[] DecodedData => _decodedData;
        public int Size { private set; get; }

        #endregion

        #region Methods
        public LzwOperator(byte[] input)
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
                var inputString = new string(_originalData.Select(x => (char)x).ToArray());
                _encodedData = Lzw.Compress(inputString);
                Size = _encodedData.Count;

                var dec = Lzw.Decompress(_encodedData);

                var decList = new List<byte>();
                foreach (var c in dec)
                {
                    decList.Add((byte)c);
                }
                _decodedData = decList.ToArray();
                
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
