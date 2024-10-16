using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ProcesXoXApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ProcesXoX başlıyor!");

            // Debug amaçlı bilgi
            Console.WriteLine("Girdi: " + string.Join(", ", args));

            // Tüm mevcut işlemleri listele
            Console.WriteLine("Mevcut işlemler:");
            var processes = Process.GetProcesses().OrderBy(p => p.ProcessName).ToList();
            for (int i = 0; i < processes.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {processes[i].ProcessName} (ID: {processes[i].Id})");
            }

            // Kullanıcıdan işlem ID'sini ya da path'i sormak
            Console.WriteLine("\nMevcut işlemlerden birini seçmek için ID'sini girin, ya da bir uygulama path'i girin:");
            string userInput = Console.ReadLine();

            Process selectedProcess = null;

            if (int.TryParse(userInput, out int processIndex) && processIndex > 0 && processIndex <= processes.Count)
            {
                // Kullanıcı listeden bir işlem seçti
                selectedProcess = processes[processIndex - 1];
            }
            else if (File.Exists(userInput))
            {
                // Kullanıcı bir uygulama path'i girdi
                Console.WriteLine($"'{userInput}' yolunda uygulama izlenecek.");

                // Path üzerinden uygulama çalıştığında PID bulmak için döngü başlat
                selectedProcess = MonitorProcessStart(userInput);
            }
            else
            {
                Console.WriteLine("Geçersiz giriş, program sonlandırılıyor.");
                return;
            }

            if (selectedProcess != null)
            {
                // İşlem için mevcut CPU affinity'yi göster
                Console.WriteLine($"Bulunan işlem: {selectedProcess.ProcessName} (ID: {selectedProcess.Id})");
                Console.WriteLine($"Mevcut Affinity: {selectedProcess.ProcessorAffinity}");

                // Yeni affinity'yi ayarla
                Console.WriteLine("Yeni CPU Affinity (örneğin, 3 sadece CPU 0 ve 1 için):");
                string affinityInput = Console.ReadLine();

                // Geçerli bir bitmask kontrolü yap
                if (int.TryParse(affinityInput, out int affinityValue) && affinityValue >= 0)
                {
                    selectedProcess.ProcessorAffinity = (IntPtr)affinityValue;
                    Console.WriteLine($"Yeni CPU Affinity ayarlandı: {affinityValue}");
                }
                else
                {
                    Console.WriteLine("Geçersiz Affinity değeri girdiniz. Lütfen geçerli bir bitmask değeri girin.");
                }
            }
            else
            {
                Console.WriteLine("İşlem bulunamadı veya başlatılamadı.");
            }

            // Programın sona ermesini beklemek için kullanıcıdan girdi bekle
            Console.WriteLine("Program sona ermek üzere, devam etmek için bir tuşa basın...");
            Console.ReadLine(); // Kullanıcıdan girdi bekle
        }

        // Path'den bir uygulama başlarsa onu izleyip Process ID'sini bulacak metod
        static Process MonitorProcessStart(string applicationPath)
        {
            Process process = null;
            Console.WriteLine("Uygulama başlatılana kadar bekleniyor...");

            while (process == null)
            {
                // Path'e uygun process var mı diye sürekli kontrol
                var matchingProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(applicationPath));

                if (matchingProcesses.Length > 0)
                {
                    process = matchingProcesses[0]; // İlk bulduğu prosesi al
                    Console.WriteLine($"Uygulama bulundu: {process.ProcessName} (ID: {process.Id})");
                }
                else
                {
                    Thread.Sleep(2000); // 2 saniye bekle ve tekrar dene
                }
            }

            return process;
        }
    }
}
