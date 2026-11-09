using GameVault.WPF.Models;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Net.Http.Json;

namespace GameVault.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient gamesClient = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            gamesClient.BaseAddress = new Uri("http://localhost:5288/api/Games");

            Loaded += async (s, e) => await LoadGames();

        }

        private async Task LoadGames()
        {

            var games = await gamesClient.GetAsync(gamesClient.BaseAddress);
            if (games.IsSuccessStatusCode)
            {
                var gameList = await games.Content.ReadFromJsonAsync<List<Game>>();
                GamesGrid.ItemsSource = gameList;
            }
            else
            {
                MessageBox.Show($"error: {games.StatusCode}");
            }
        }

        private async Task<Game?> GetGame(int id)
        {
            return await gamesClient.GetFromJsonAsync<Game>(
                    $"{gamesClient.BaseAddress}/{id}");
        }
        private async void AddGame_Click(
     object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateFields())
                    return;

                Game game = ReadGameFields();

                var response = await gamesClient.PostAsJsonAsync(
                    gamesClient.BaseAddress, game);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Game added successfully!");

                    ClearFields();
                    await LoadGames();
                }
                else
                {
                    MessageBox.Show(
                        "Error adding game: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // PUT: api/Games/{id}
        private async void UpdateGame_Click(
            object sender, RoutedEventArgs e)
        {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Please select a game to update.");
                    return;
                }

                if (!ValidateFields())
                    return;

                Game game = ReadGameFields();
                game.Id = id;

                var response = await gamesClient.PutAsJsonAsync(
                    $"{gamesClient.BaseAddress}/{id}", game);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Game updated successfully!");

                    ClearFields();
                    await LoadGames();
                }
                else
                {
                    MessageBox.Show(
                        "Error updating game: " + response.StatusCode);
                }
        }

        // DELETE: api/Games/{id}
        private async void DeleteGame_Click(
            object sender, RoutedEventArgs e)
        {
                if (!int.TryParse(txtId.Text, out int id))
                {
                    MessageBox.Show("Please select a game to delete.");
                    return;
                }

                var result = MessageBox.Show(
                    "Are you sure you want to delete this game?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                var response = await gamesClient.DeleteAsync(
                    $"{gamesClient.BaseAddress}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Game deleted successfully!");

                    ClearFields();
                    await LoadGames();
                }
                else
                {
                    MessageBox.Show(
                        "Error deleting game: " + response.StatusCode);
                }
        }
        private void GamesGrid_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GamesGrid.SelectedItem is Game game)
            {
                txtId.Text = game.Id.ToString();
                txtTitle.Text = game.Title;
                txtPlatform.Text = game.Platform;
                txtGenre.Text = game.Genre;
                txtYear.Text = game.ReleaseYear.ToString();
                txtDeveloper.Text = game.Developer;
            }
        }
        private Game ReadGameFields()
        {
            return new Game
            {
                Title = txtTitle.Text.Trim(),
                Platform = txtPlatform.Text.Trim(),
                Genre = txtGenre.Text.Trim(),
                ReleaseYear = int.Parse(txtYear.Text),
                Developer = txtDeveloper.Text.Trim()
            };
        }
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtPlatform.Text) ||
                string.IsNullOrWhiteSpace(txtGenre.Text) ||
                string.IsNullOrWhiteSpace(txtYear.Text) ||
                string.IsNullOrWhiteSpace(txtDeveloper.Text))
            {
                MessageBox.Show("Please complete all fields.");
                return false;
            }

            if (!int.TryParse(txtYear.Text, out int year) ||
                year < 0)
            {
                MessageBox.Show("Please enter a valid release year.");
                return false;
            }

            return true;
        }

        // Clear input fields
        private void ClearFields()
        {
            txtId.Clear();
            txtTitle.Clear();
            txtPlatform.Clear();
            txtGenre.Clear();
            txtYear.Clear();
            txtDeveloper.Clear();

            GamesGrid.SelectedItem = null;
        }

        private void Clear_Click(
            object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        // Refresh DataGrid
        private async void Refresh_Click(
            object sender, RoutedEventArgs e)
        {
            await LoadGames();
        }
    }
}
