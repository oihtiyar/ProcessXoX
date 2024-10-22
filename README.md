# ProcessXoX

Uygulamayı geliştirmekteki motivaston Windows işletim sistemleri üzerinde path izleme yaparak CPU ataması sağlamak. Benim durumumda 7zip uygulamasına belirli bir sayıda CPU atamam gerekiyordu. Mevcutta 7zip uygulaması çalıştığında sunucu üzerindeki tüm kaynakları sömürerek CPU kullanım oranını %100 seviyesine çıkarıyordu. Her seferinde girip el ile işlem yapmaktansa uygulamayı monitör edip atama yapabilecek bir uygulama yazarak sorunuma bu şekilde çözüm buldum.

####################

The motivation for developing the application is to provide CPU assignment by path tracing on Windows operating systems. In my case I needed to assign a certain number of CPUs to the 7zip application. Currently, when the 7zip application runs, it consumes all resources on the server and increases the CPU usage rate to 100%. This is how I found a solution to my problem by writing an application that can monitor the application and make assignments, rather than logging in and performing manual operations each time.

#####################

Uygulama kullanım adımları;

İlgili appv1 altındaki dosyaları sunucunuza kopyalayın. 
komut satırı üzerinden kopyaladığınız dizine girerek uygulamayı çalıştırın.
ardından cpu atayacağınız uygulamanın path'ini girin.
daha sonra değeri seçin ve uygulamanın çalışmasnı bekleyin.
