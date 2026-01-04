using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DepoEnvanterApp.Models
{
    public class Urun : INotifyPropertyChanged
    {
        private int _id;
        private string _urunAdi = string.Empty;
        private int _stokAdedi;
        private double _fiyat;
        private string _barkod = string.Empty;
        private string _kategori = string.Empty;
        private string _resimYolu = "pack://application:,,,/Resources/no-image.png";

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string UrunAdi
        {
            get => _urunAdi;
            set { _urunAdi = value; OnPropertyChanged(); }
        }

        public int StokAdedi
        {
            get => _stokAdedi;
            set { _stokAdedi = value; OnPropertyChanged(); }
        }

        public double Fiyat
        {
            get => _fiyat;
            set { _fiyat = value; OnPropertyChanged(); }
        }

        public string Barkod
        {
            get => _barkod;
            set { _barkod = value; OnPropertyChanged(); }
        }

        public string Kategori
        {
            get => _kategori;
            set { _kategori = value; OnPropertyChanged(); }
        }

        public string ResimYolu
        {
            get => _resimYolu;
            set { _resimYolu = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}