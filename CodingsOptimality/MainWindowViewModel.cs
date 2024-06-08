using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CodingsOptimality.Backend.HuffmanOperator;
using CodingsOptimality.Backend.LzwOperator;
using CodingsOptimality.Backend.Shannon_FanoOperator;
using CodingsOptimality.Backend.ZStandardOperator;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace CodingsOptimality
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public MainWindowViewModel() { }

        #region Fields

        private string _extension = "";
        private byte[] _decodedHuffman = Array.Empty<byte>();
        private byte[] _decodedZStandard = Array.Empty<byte>();
        private byte[] _decodedLzw = Array.Empty<byte>();
        private byte[] _decodedShannonFano = Array.Empty<byte>();

        #endregion

        #region Properties

        private Window _parentWindow;
        public Window ParentWindow
        {
            get => _parentWindow;
            set
            {
                _parentWindow = value;
                OnPropertyChanged();
            }
        }



        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private string _fileNameHuffman;
        public string FileNameHuffman
        {
            get => _fileNameHuffman;
            set
            {
                _fileNameHuffman = value;
                OnPropertyChanged();
            }
        }
        private string _fileNameLzw;
        public string FileNameLzw
        {
            get => _fileNameLzw;
            set
            {
                _fileNameLzw = value;
                OnPropertyChanged();
            }
        }

        private string _fileNameShannonFano;
        public string FileNameShannonFano
        {
            get => _fileNameShannonFano;
            set
            {
                _fileNameShannonFano = value;
                OnPropertyChanged();
            }
        }

        private string _fileNameZStandard;
        public string FileNameZStandard
        {
            get => _fileNameZStandard;
            set
            {
                _fileNameZStandard = value;
                OnPropertyChanged();
            }
        }

        private double _huffmanTime;
        public double HuffmanTime
        {
            get => _huffmanTime;
            set
            {
                _huffmanTime = value;
                OnPropertyChanged();
            }
        }

        private double _shannonFanoTime;
        public double ShannonFanoTime
        {
            get => _shannonFanoTime;
            set
            {
                _shannonFanoTime = value;
                OnPropertyChanged();
            }
        }

        private double _zStandardTime;
        public double ZStandardTime
        {
            get => _zStandardTime;
            set
            {
                _zStandardTime = value;
                OnPropertyChanged();
            }
        }
        private double _lzwTime;
        public double LzwTime
        {
            get => _lzwTime;
            set
            {
                _lzwTime = value;
                OnPropertyChanged();
            }
        }

        private int _originalSize;
        public int OriginalSize
        {
            get => _originalSize;
            set
            {
                _originalSize = value;
                OnPropertyChanged();
            }
        }

        private int _huffmanSize;
        public int HuffmanSize
        {
            get => _huffmanSize;
            set
            {
                _huffmanSize = value;
                OnPropertyChanged();
            }
        }

        private int _shannonFanoSize;
        public int ShannonFanoSize
        {
            get => _shannonFanoSize;
            set
            {
                _shannonFanoSize = value;
                OnPropertyChanged();
            }
        }


        private int _zStandardSize;
        public int ZStandardSize
        {
            get => _zStandardSize;
            set
            {
                _zStandardSize = value;
                OnPropertyChanged();
            }
        }

        private int _lzwSize;
        public int LzwSize
        {
            get => _lzwSize;
            set
            {
                _lzwSize = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Methods

        private bool SelectFile()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.AddExtension = false;
            if (fileDialog.ShowDialog(ParentWindow) ?? false)
            {
                _fileName = fileDialog.FileName;
                _extension = Path.GetExtension(fileDialog.FileName);
                _originalSize = GetCompressedSize(File.ReadAllBytes(FileName).Length);
                return true;
            }

            return false;
        }

        private void EncodeHuffman(ref string errors)
        {
            var input = File.ReadAllBytes(Path.GetFullPath(FileName));
            var huffmanEncoder = new HuffmanOperator(input);
            if (!huffmanEncoder.Encode(ref errors)) return;

            _decodedHuffman = huffmanEncoder.DecodedData;
            _huffmanTime = huffmanEncoder.Time;
            _huffmanSize = GetCompressedSize(huffmanEncoder.Size);
        }

        private void EncodeLzw(ref string errors)
        {
            var input = File.ReadAllBytes(Path.GetFullPath(FileName));
            var lzwOperator = new LzwOperator(input);
            if (!lzwOperator.Encode(ref errors)) return;

            _decodedLzw = lzwOperator.DecodedData;
            _lzwTime = lzwOperator.Time;
            _lzwSize = GetCompressedSize(lzwOperator.Size);
        }

        private void EncodeShannonFano(ref string errors)
        {
            var input = File.ReadAllBytes(Path.GetFullPath(FileName));
            var shannonFanoOperator = new ShannonFanoOperator(input);
            if (!shannonFanoOperator.Encode(ref errors)) return;

            _decodedShannonFano = shannonFanoOperator.DecodedData;
            _shannonFanoTime = shannonFanoOperator.Time;
            _shannonFanoSize = GetCompressedSize(shannonFanoOperator.Size);
        }




        private void EncodeZStandard(ref string errors)
        {
            var input = File.ReadAllBytes(Path.GetFullPath(FileName));
            var zStandardOperator = new ZStandardOperator(input);
            if (!zStandardOperator.Encode(ref errors)) return;

            _decodedZStandard = zStandardOperator.DecodedData;
            _zStandardTime = zStandardOperator.Time;
            _zStandardSize = GetCompressedSize(zStandardOperator.Size);
        }


        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(FileName));
            OnPropertyChanged(nameof(FileNameHuffman));
            OnPropertyChanged(nameof(FileNameZStandard));
            OnPropertyChanged(nameof(HuffmanTime));
            OnPropertyChanged(nameof(ZStandardTime));
            OnPropertyChanged(nameof(HuffmanSize));
            OnPropertyChanged(nameof(ZStandardSize));
            OnPropertyChanged(nameof(OriginalSize));
            OnPropertyChanged(nameof(FileNameLzw));
            OnPropertyChanged(nameof(LzwTime));
            OnPropertyChanged(nameof(LzwSize));
            OnPropertyChanged(nameof(FileNameShannonFano));
            OnPropertyChanged(nameof(ShannonFanoTime));
            OnPropertyChanged(nameof(ShannonFanoSize));
        }

        private void SaveFileHuffman()
        {
            var directoryName = Path.GetDirectoryName(Path.GetFullPath(FileName));
            var huffmanFileName = $@"{directoryName}\{Path.GetFileNameWithoutExtension(FileName)}Huffman{_extension}";
            File.WriteAllBytes(huffmanFileName, _decodedHuffman);
            _fileNameHuffman = huffmanFileName;
        }

        private void SaveFileLzw()
        {
            var directoryName = Path.GetDirectoryName(Path.GetFullPath(FileName));
            var fileNameLzw = $@"{directoryName}\{Path.GetFileNameWithoutExtension(FileName)}Lzw{_extension}";
            File.WriteAllBytes(fileNameLzw, _decodedLzw);
            _fileNameLzw = fileNameLzw;
        }

        private void SaveFileZStandard()
        {
            var directoryName = Path.GetDirectoryName(Path.GetFullPath(FileName));
            var zStandardFileName = $@"{directoryName}\{Path.GetFileNameWithoutExtension(FileName)}ZStandard{_extension}";
            File.WriteAllBytes(zStandardFileName, _decodedZStandard);
            _fileNameZStandard = zStandardFileName;
        }

        private void SaveFileShannonFano()
        {
            var directoryName = Path.GetDirectoryName(Path.GetFullPath(FileName));
            var shannonFanoFileName = $@"{directoryName}\{Path.GetFileNameWithoutExtension(FileName)}ShannonFano{_extension}";
            File.WriteAllBytes(shannonFanoFileName, _decodedShannonFano);
            _fileNameShannonFano = shannonFanoFileName;
        }

        private void SaveFile()
        {
            try
            {
                SaveFileHuffman();
                SaveFileZStandard();
                SaveFileLzw();
                SaveFileShannonFano();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ParentWindow, "Can`t save file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int GetCompressedSize(int length)
        {
            var lenghtUpdated = (double)length;
            return (int)Math.Round(lenghtUpdated);
        }

        #endregion

        #region Commands

        
        public ICommand StartCommand => new RelayCommand(() =>
        {
            var oldTitle = ParentWindow.Title;
            ParentWindow.Title = "Work in progress...";
            var isFileSelected = SelectFile();
            if (!isFileSelected) return;

            var zStandardErrors = "";
            var huffmanErrors = "";
            var lzwErrors = "";
            var shannonFanoErrors = "";
            var actions = new[]
            {
                () =>
                {
                    EncodeZStandard(ref zStandardErrors);
                },
                () =>
                {
                    EncodeHuffman(ref huffmanErrors);
                },
                () =>
                {
                    EncodeLzw(ref lzwErrors);
                },
                () =>
                {
                    EncodeShannonFano(ref shannonFanoErrors);
                }
            };

            Parallel.Invoke(actions);

            var isError = false;
            if (!string.IsNullOrEmpty(huffmanErrors))
            {
                MessageBox.Show(ParentWindow, huffmanErrors, "Huffman Encoding Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                isError = true;
            }

            if (!string.IsNullOrEmpty(lzwErrors))
            {
                MessageBox.Show(ParentWindow, lzwErrors, "Lzw Encoding Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                isError = true;
            }

            if (!string.IsNullOrEmpty(zStandardErrors))
            {
                MessageBox.Show(ParentWindow, zStandardErrors, "ZStandard Encoding Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                isError = true;
            }

            if (!string.IsNullOrEmpty(shannonFanoErrors))
            {
                MessageBox.Show(ParentWindow, shannonFanoErrors, "Shannon-Fano Encoding Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                isError = true;
            }

            if (isError)
            {
                return;
            }

            SaveFile();
            UpdateProperties();
            ParentWindow.Title = oldTitle;
            var resultWindow = new ResultWindow(this);
            resultWindow.ShowDialog();

        });

        public ICommand CloseCommand => new RelayCommand(() =>
        {
            Environment.Exit(0);
        });

        #endregion

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
