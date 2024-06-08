using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using ZstdNet;

namespace CodingsOptimality.Backend.ZStandardOperator
{
    internal class ZStandardOperator
    {
        #region fields

        private byte[]? _originalData;
        private byte[] _encodedData = Array.Empty<byte>();
        private byte[] _decodedData = Array.Empty<byte>();
        private double _time;

        #endregion

        #region Properties

        public double Time => _time;
        public byte[] DecodedData => _decodedData;
        public int Size { private set; get; }

        #endregion

        #region Methods
        public ZStandardOperator(byte[] input)
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
                
                var compressor = new Compressor();

               
                _encodedData = compressor.Wrap(_originalData);
                var decompressor = new Decompressor();
                _decodedData = decompressor.Unwrap(_encodedData);
                _time = (DateTime.Now - timeStart).Milliseconds;
                Size = _encodedData.Length;
                return true;
            }
            catch (ZstdException zstdException)
            {
                errorString = zstdException.Message;
                return false;
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
