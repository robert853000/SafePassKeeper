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
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Microsoft.Win32;
namespace SafePassKeeper
{


    public partial class MainWindow : Window
    {    /*private static readonly string FilePath = "encryptedText.txt";
    private static readonly string Password = "your-password"; // Změňte na vaše heslo*/
        private static string FilePath;
        private static string Password;
        private ObservableCollection<Account> accounts;

        public MainWindow()
        {
            InitializeComponent();

        }
        private void SetFilePathButton_Click(object sender, RoutedEventArgs e)
        {
            // Vytvoření dialogu pro výběr cesty k šifrovanému souboru
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Encrypted files (*.txt)|*.txt",
                Title = "Select Encrypted Password File"
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string newFilePath = openFileDialog.FileName;

                //if (FilePath != newFilePath)
              //  {
                    FilePath = newFilePath;

                    // Otevření nového okna pro zadání hesla
                    var passwordPrompt = new PasswordPromptWindow();
                    if (passwordPrompt.ShowDialog() == true)
                    {
                        Password = passwordPrompt.Password;

                        // Zavolání funkce pro načtení účtů
                        LoadAccounts();
                    }
                    else
                    {
                        MessageBox.Show("Password was not provided. Please try again.");
                    }
               // }
            }
        }
        private void CreateDefaultAccounts()
        {
            accounts = new ObservableCollection<Account>
    {
        new Account
        {
            AccountName = "Example Account 1",
            Username = "example1@example.com",
            Password = "password123",
            Domain = "example.com",
            Note = "Default account"
        },
        new Account
        {
            AccountName = "Example Account 2",
            Username = "example2@example.com",
            Password = "password456",
            Domain = "example.org",
            Note = "Another default account"
        }
    };

            // Uložení výchozích účtů do souboru
            SaveAccounts();
        }
        private void LoadAccounts()
        {
            if (string.IsNullOrEmpty(FilePath) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("File path or password is not set.");
                return;
            }

            try
            {
                byte[] encryptedBytes = File.ReadAllBytes(FilePath);

                // Dešifrování obsahu souboru
                string decryptedJson = Decrypt(encryptedBytes, Password);

                // Kontrola zda JSON není prázdný a platný
                if (string.IsNullOrWhiteSpace(decryptedJson) /*|| !IsValidJson(decryptedJson)*/)
                {
                    // Pokud není platný, vytvoříme výchozí účty
                    CreateDefaultAccounts();
                }
                else
                {
                    accounts = JsonSerializer.Deserialize<ObservableCollection<Account>>(decryptedJson);

                    // Pokud je seznam null, vytvoříme výchozí účty
                    if (accounts == null)
                    {
                        CreateDefaultAccounts();
                    }
                }

                // Naplnění ListBoxu
                AccountListBox.ItemsSource = accounts;
                AccountListBox.DisplayMemberPath = "AccountName";
                // Zobrazíme prvky UI po úspěšném načtení účtů
                AccountListBox.ItemsSource = accounts;
                AccountListBox.DisplayMemberPath = "AccountName";
                AccountListBox.Visibility = Visibility.Visible;
                DetailsTextBox.Visibility = Visibility.Visible;
                AddAccountButton.Visibility = Visibility.Visible;
                EditAccountButton.Visibility = Visibility.Visible;
                DeleteAccountButton.Visibility = Visibility.Visible;
                ExportBackupButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                AccountListBox.Visibility = Visibility.Hidden;
                DetailsTextBox.Visibility = Visibility.Hidden;
                AddAccountButton.Visibility = Visibility.Hidden;
                EditAccountButton.Visibility = Visibility.Hidden;
                DeleteAccountButton.Visibility = Visibility.Hidden;
                ExportBackupButton.Visibility = Visibility.Hidden;
                MessageBox.Show($"Error loading accounts: {ex.Message}");
               // CreateDefaultAccounts();
            }
            /*
            try
            {
                // Načtení a dešifrování
                byte[] encryptedBytes = File.ReadAllBytes(FilePath);
                string decryptedJson = Decrypt(encryptedBytes, Password);
                accounts = JsonSerializer.Deserialize<ObservableCollection<Account>>(decryptedJson);

                // Naplnění ListBoxu
                AccountListBox.ItemsSource = accounts;
                AccountListBox.DisplayMemberPath = "AccountName";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading accounts: {ex.Message}");
            }*/
        }

        private void AccountListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountListBox.SelectedItem is Account selectedAccount)
            {
                // Zobrazení detailů v TextBoxu
                DetailsTextBox.Text = $"Account Name: {selectedAccount.AccountName}\n" +
                                      $"Username: {selectedAccount.Username}\n" +
                                      $"Password: {selectedAccount.Password}\n" +
                                      $"Domain: {selectedAccount.Domain}\n" +
                                      $"Note: {selectedAccount.Note}";
            }
        }
        private void EncryptAndSaveText(string text, string password)
        {
            byte[] encryptedBytes = Encrypt(text, password);
            File.WriteAllBytes(FilePath, encryptedBytes);
        }

        private string DecryptAndReadText(string password)
        {
            byte[] encryptedBytes = File.ReadAllBytes(FilePath);
            return Decrypt(encryptedBytes, password);
        }

        private byte[] Encrypt(string text, string password)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey(password, aes.KeySize / 8);
                aes.IV = GenerateKey(password, aes.BlockSize / 8);

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(text);
                            }
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        private string Decrypt(byte[] cipherText, string password)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey(password, aes.KeySize / 8);
                aes.IV = GenerateKey(password, aes.BlockSize / 8);

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
        private void SaveAccounts()
        {
            try
            {
                string json = JsonSerializer.Serialize(accounts);
                byte[] encryptedData = Encrypt(json, Password);
                File.WriteAllBytes(FilePath, encryptedData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving accounts: {ex.Message}");
            }
        }
        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountListBox.SelectedItem as Account;
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account to delete.");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete the account '{selectedAccount.AccountName}'?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Odstranění účtu ze seznamu
                accounts.Remove(selectedAccount);

                // Uložení do souboru
                SaveAccounts();

                // Vymazání detailů v TextBoxu
                DetailsTextBox.Clear();
            }
        }
        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var addAccountWindow = new AddAccountWindow(null);
            if (addAccountWindow.ShowDialog() == true)
            {
                // Přidání nového účtu do kolekce a aktualizace ListBoxu
                // accounts.Add(addAccountWindow.NewAccount);
                accounts.Add(addAccountWindow.EditedAccount);

                // Uložení do souboru
                SaveAccounts();
            }
        }
        private void ExportBackupButton_Click(object sender, RoutedEventArgs e)
        {
            // Vytvoření dialogu pro výběr cesty pro export
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = "backup_passwords.json"
            };

            bool? result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Serializace účtů do JSON
                    string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });

                    // Uložení JSON do souboru
                    File.WriteAllText(filePath, json);

                    MessageBox.Show("Backup exported successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting backup: {ex.Message}");
                }
            }
        }
        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAccount = AccountListBox.SelectedItem as Account;
            if (selectedAccount == null)
            {
                MessageBox.Show("Please select an account to edit.");
                return;
            }

            var editAccountWindow = new AddAccountWindow(selectedAccount);
            if (editAccountWindow.ShowDialog() == true)
            {
                // Aktualizování ListBoxu
                AccountListBox.ItemsSource = null;
                AccountListBox.ItemsSource = accounts;

                // Uložení do souboru
                SaveAccounts();
            }
        }
        private byte[] GenerateKey(string password, int length)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                byte[] keyBytes = new byte[length];
                Array.Copy(hashBytes, keyBytes, length);
                return keyBytes;
            }
        }
    }
}