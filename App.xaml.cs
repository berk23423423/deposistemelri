using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using DepoEnvanterApp.Data;

namespace DepoEnvanterApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Veritabanı başlatma işlemi
            try
            {
                using (var context = new AppDbContext())
                {
                    // 1. Veritabanını oluştur (yoksa)
                    context.Database.EnsureCreated();

                    // 2. Bekleyen migration'ları uygula
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                    // 3. Seed Data ekle (varsayılan veriler)
                    DbInitializer.Initialize(context);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Veritabanı başlatma hatası:\n{ex.Message}\n\nLütfen SQL Server Express'in çalıştığından emin olun.",
                    "Veritabanı Hatası",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                // Uygulama kapansın
                Current.Shutdown();
            }
        }
    }

}
