using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace GutenbergReader;

public partial class Form1 : Form
{
    private readonly HttpClient _client = new HttpClient();

    public Form1()
    {
        InitializeComponent();
        _client.DefaultRequestHeaders.Add("User-Agent", "C# Gutenberg Reader");
    }

    private async Task LoadHamlet()
    {
        txtDisplay.Text = "Загрузка...";
        try
        {
            txtDisplay.Text = await _client.GetStringAsync("https://www.gutenberg.org/cache/epub/1524/pg1524.txt");
            return;
        }
        catch
        {
        }

        try
        {
            txtDisplay.Text = await _client.GetStringAsync("https://www.gutenberg.org/cache/epub/1524/pg1524.txt.utf-8");
        }
        catch
        {
            txtDisplay.Text = "";
            MessageBox.Show("Ошибка загрузки текста.");
        }
    }

    private async Task LoadTopBooks()
    {
        lstBooks.Items.Clear();
        try
        {
            string json = await _client.GetStringAsync("https://gutendex.com/books/");
            var data = JsonSerializer.Deserialize<GutendexResponse>(json);

            if (data?.results != null)
            {
                foreach (var book in data.results)
                {
                    lstBooks.Items.Add(book);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке: {ex.Message}");
        }
    }

    private async Task ShowBookDetails()
    {
        if (lstBooks.SelectedItem is not GutenbergBook selectedBook) return;

        string coverUrl = selectedBook.formats.ContainsKey("image/jpeg") 
            ? selectedBook.formats["image/jpeg"] 
            : null;

        if (picCover.Image != null)
        {
            picCover.Image.Dispose();
            picCover.Image = null;
        }

        if (string.IsNullOrEmpty(coverUrl)) return;

        try
        {
            using (var stream = await _client.GetStreamAsync(coverUrl))
            {
                var newImage = Image.FromStream(stream);
                picCover.Image = newImage;
            }
        }
        catch
        {
        }
    }

    private async Task SearchBooks()
    {
        if (string.IsNullOrWhiteSpace(txtSearch.Text)) return;

        lstSearch.Items.Clear();
        lstSearch.Items.Add("Поиск...");
        
        try
        {
            string query = Uri.EscapeDataString(txtSearch.Text);
            string json = await _client.GetStringAsync($"https://gutendex.com/books/?search={query}");
            var data = JsonSerializer.Deserialize<GutendexResponse>(json);

            lstSearch.Items.Clear();
            if (data?.results != null && data.results.Count > 0)
            {
                foreach (var book in data.results)
                {
                    lstSearch.Items.Add(book);
                }
            }
            else
            {
                lstSearch.Items.Add("Ничего не найдено");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка поиска: {ex.Message}");
        }
    }

    private async Task DownloadAuthorBooks()
    {
        string fullName = $"{txtAuthorFirst.Text} {txtAuthorLast.Text}".Trim();

        if (string.IsNullOrEmpty(fullName))
        {
            MessageBox.Show("Введите имя автора!");
            return;
        }

        txtDisplay.Text = $"Ищем все книги автора: {fullName}...";
        tabControl.SelectedTab = tab1; 

        try
        {
            string query = Uri.EscapeDataString(fullName);
            string json = await _client.GetStringAsync($"https://gutendex.com/books/?search={query}");
            var data = JsonSerializer.Deserialize<GutendexResponse>(json);

            if (data?.results != null && data.results.Count > 0)
            {
                txtDisplay.Clear();
                txtDisplay.AppendText($"РЕЗУЛЬТАТЫ ПОИСКА ДЛЯ: {fullName.ToUpper()}\n");
                txtDisplay.AppendText("==========================================\n\n");

                foreach (var book in data.results)
                {
                    txtDisplay.AppendText($"ID: {book.id} | {book.title}\n");
                    txtDisplay.AppendText("------------------------------------------\n");
                }
            }
            else
            {
                txtDisplay.Text = $"Книги автора '{fullName}' не найдены в базе Gutenberg.";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
            tabControl.SelectedTab = tab5; 
        }
    }
}

public class GutendexResponse 
{
    public List<GutenbergBook> results { get; set; }
}

public class GutenbergBook 
{
    public int id { get; set; }
    public string title { get; set; }
    public List<Author> authors { get; set; }
    public Dictionary<string, string> formats { get; set; }
    
    public override string ToString() => $"{id}: {title}";
}

public class Author 
{
    public string name { get; set; }
}