using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ProcesXoXApp
{
    class Program
    {
        private static string configFilePath = "config.txt"; // Ayarların saklanacağı dosya

        static void Main(string[] args)
        {
            Console.WriteLine("ProcesXoX başlıyor!");

            string applicationPath;
            int affinityValue;

            // Eğer config dosyası varsa önceki path ve affinity değerini yükle
            if (File.Exists(configFilePath))
            {
                var config = File.ReadAllLines(configFilePath);
                applicationPath = config[0];
                affinityValue = int.Parse(config[1]);
                Console.WriteLine($"Önceki ayarlar yüklendi: Uygulama: {applicationPath}, Affinity: {affinityValue}");
            }
            else
            {
                // Kullanıcıdan path ve affinity ayarlarını al
                Console.WriteLine("Bir uygulama path'i girin:");
                applicationPath = Console.ReadLine();

                // Yeni CPU affinity değerini sorma
                Console.WriteLine("Uygulama için ayarlanacak CPU Affinity (örneğin, 3 sadece CPU 0 ve 1 için):");
                string affinityInput = Console.ReadLine();

                // Geçerli bir bitmask kontrolü yap
                if (!int.TryParse(affinityInput, out affinityValue) || affinityValue < 0)
                {
                    Console.WriteLine("Geçersiz Affinity değeri girdiniz. Lütfen geçerli bir bitmask değeri girin.");
                    return;
                }

                // Yeni ayarları dosyaya kaydet
                File.WriteAllLines(configFilePath, new string[] { applicationPath, affinityValue.ToString() });
            }

            // Path'teki uygulama başlatıldığında affinity ayarını yapmak için izlemeye başla
            MonitorProcessAndSetAffinity(applicationPath, affinityValue);

            // Programın sona ermesini beklemek için kullanıcıdan girdi bekle
            Console.WriteLine("Program sona ermek üzere, devam etmek için bir tuşa basın...");
            Console.ReadLine(); // Kullanıcıdan girdi bekle
        }

        // Uygulama başlatıldığında affinity ayarlayacak metod
        static void MonitorProcessAndSetAffinity(string applicationPath, int affinityValue)
        {
            Console.WriteLine($"'{applicationPath}' yolundaki uygulama izleniyor...");

            while (true)
            {
                Process process = null;

                // Path'e uygun process var mı diye sürekli kontrol
                var matchingProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(applicationPath));

                if (matchingProcesses.Length > 0)
                {
                    process = matchingProcesses[0]; // İlk bulduğu prosesi al
                    Console.WriteLine($"Uygulama bulundu: {process.ProcessName} (ID: {process.Id})");

                    // Affinity ayarla
                    try
                    {
                        process.ProcessorAffinity = (IntPtr)affinityValue;
                        Console.WriteLine($"Affinity başarıyla ayarlandı: {affinityValue}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Affinity ayarlanırken bir hata oluştu: {ex.Message}");
                    }
                }

                Thread.Sleep(2000); // 2 saniye bekle ve tekrar dene
            }
        }
    }
}
