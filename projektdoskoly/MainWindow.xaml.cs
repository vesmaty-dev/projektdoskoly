using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MathPracticeApp
{
    public partial class MainWindow : Window
    {
        private MathQuestion aktualniUloha;
        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            GenerujUlohu();
        }

        private void CategoryChanged(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) GenerujUlohu();
        }

        private void GenerujUlohu()
        {
            TxtOdpoved.Text = "";
            TxtVysledek.Text = "";
            TxtOdpoved.Focus();

            if (RbAlgebra.IsChecked == true)
            {
                aktualniUloha = GenerujAlgebru();
            }
            else
            {
                aktualniUloha = GenerujGeometrii();
            }

            TxtZadani.Text = aktualniUloha.TextUlohy;
        }

        private MathQuestion GenerujAlgebru()
        {
            
            int a = rnd.Next(2, 10);
            int x = rnd.Next(-10, 11);
            int b = rnd.Next(-20, 20);
            int c = (a * x) + b;

            string znamenko = b >= 0 ? $"+ {b}" : $"- {Math.Abs(b)}";

            return new MathQuestion
            {
                TextUlohy = $"Vyřešte rovnici pro x:\n\n{a}x {znamenko} = {c}",
                SpravnaOdpoved = x,
                Kategorie = "Algebra"
            };
        }

        private MathQuestion GenerujGeometrii()
        {
            int typ = rnd.Next(0, 3);
            MathQuestion otazka = new MathQuestion { Kategorie = "Geometrie" };

            switch (typ)
            {
                case 0: // Pythagorova věta
                    int odvesnaA = rnd.Next(3, 10);
                    int odvesnaB = rnd.Next(3, 10);
                    double prepona = Math.Round(Math.Sqrt(odvesnaA * odvesnaA + odvesnaB * odvesnaB), 2);
                    otazka.TextUlohy = $"Pravoúhlý trojúhelník má odvěsny a = {odvesnaA} cm, b = {odvesnaB} cm.\nVypočítejte délku přepony v cm (zaokrouhlete na 2 des. místa).";
                    otazka.SpravnaOdpoved = prepona;
                    break;

                case 1: // Obsah kruhu
                    int r = rnd.Next(2, 12);
                    double obsah = Math.Round(Math.PI * r * r, 2);
                    otazka.TextUlohy = $"Kruh má poloměr r = {r} cm.\nVypočítejte jeho obsah v cm² (zaokrouhlete na 2 des. místa, π = 3.14).";
                    otazka.SpravnaOdpoved = obsah;
                    break;

                case 2: // Objem krychle
                    int hrana = rnd.Next(2, 15);
                    otazka.TextUlohy = $"Vypočítejte objem krychle o hraně a = {hrana} cm.\nVýsledek uveďte v cm³.";
                    otazka.SpravnaOdpoved = Math.Pow(hrana, 3);
                    break;
            }
            return otazka;
        }

        private void Vyhodnot()
        {
            if (string.IsNullOrWhiteSpace(TxtOdpoved.Text)) return;

            // Náhrada desetinné čárky/tečky pro konzistenci
            string vstup = TxtOdpoved.Text.Replace('.', ',');

            if (double.TryParse(vstup, out double vysledek))
            {
                // Tolerance pro zaokrouhlení
                if (Math.Abs(vysledek - aktualniUloha.SpravnaOdpoved) < 0.05)
                {
                    TxtVysledek.Text = "Správně! Výborně.";
                    TxtVysledek.Foreground = Brushes.Green;
                }
                else
                {
                    TxtVysledek.Text = $"Chyba. Správná odpověď je {aktualniUloha.SpravnaOdpoved}.";
                    TxtVysledek.Foreground = Brushes.Red;
                }
            }
            else
            {
                TxtVysledek.Text = "Zadejte platné číslo!";
                TxtVysledek.Foreground = Brushes.Orange;
            }
        }

        private void BtnZkontrolovat_Click(object sender, RoutedEventArgs e)
        {
            Vyhodnot();
        }

        private void TxtOdpoved_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Vyhodnot();
            }
        }

        private void BtnDalsi_Click(object sender, RoutedEventArgs e)
        {
            GenerujUlohu();
        }
    }
}