namespace GutenbergReader;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;
    private TabControl tabControl;
    private TabPage tab1, tab2, tab3, tab5;
    private RichTextBox txtDisplay;
    private ListBox lstBooks, lstSearch;
    private PictureBox picCover;
    private TextBox txtSearch, txtAuthorFirst, txtAuthorLast;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.tabControl = new TabControl { Dock = DockStyle.Fill };
        this.tab1 = new TabPage("Задание 1");
        this.tab2 = new TabPage("Задание 2 & 4");
        this.tab3 = new TabPage("Задание 3");
        this.tab5 = new TabPage("Задание 5");

        // Задание 1
        Button btnHamlet = new Button { Text = "Загрузить Гамлета", Top = 10, Left = 10, Width = 150 };
        btnHamlet.Click += async (s, e) => await LoadHamlet();
        txtDisplay = new RichTextBox { Top = 40, Left = 10, Width = 750, Height = 350, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom };
        tab1.Controls.Add(btnHamlet);
        tab1.Controls.Add(txtDisplay);

        // Задание 2 & 4
        Button btnTop = new Button { Text = "Загрузить Топ-100", Top = 10, Left = 10, Width = 150 };
        btnTop.Click += async (s, e) => await LoadTopBooks();
        lstBooks = new ListBox { Top = 40, Left = 10, Width = 300, Height = 350 };
        lstBooks.SelectedIndexChanged += async (s, e) => await ShowBookDetails();
        picCover = new PictureBox { Top = 40, Left = 320, Width = 200, Height = 300, SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
        tab2.Controls.Add(btnTop);
        tab2.Controls.Add(lstBooks);
        tab2.Controls.Add(picCover);

        // Задание 3
        txtSearch = new TextBox { Top = 10, Left = 10, Width = 200 };
        Button btnSearch = new Button { Text = "Поиск", Top = 10, Left = 220 };
        btnSearch.Click += async (s, e) => await SearchBooks();
        lstSearch = new ListBox { Top = 40, Left = 10, Width = 400, Height = 300 };
        tab3.Controls.Add(txtSearch);
        tab3.Controls.Add(btnSearch);
        tab3.Controls.Add(lstSearch);

        // Задание 5
        txtAuthorFirst = new TextBox { Top = 10, Left = 10, PlaceholderText = "Имя" };
        txtAuthorLast = new TextBox { Top = 10, Left = 120, PlaceholderText = "Фамилия" };
        Button btnAuthor = new Button { Text = "Скачать книги", Top = 10, Left = 230, Width = 120 };
        btnAuthor.Click += async (s, e) => await DownloadAuthorBooks();
        tab5.Controls.Add(txtAuthorFirst);
        tab5.Controls.Add(txtAuthorLast);
        tab5.Controls.Add(btnAuthor);

        this.tabControl.TabPages.AddRange(new[] { tab1, tab2, tab3, tab5 });
        this.Controls.Add(tabControl);
        this.Size = new Size(800, 500);
    }
}
