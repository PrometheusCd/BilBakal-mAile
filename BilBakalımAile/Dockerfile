# 1. AŞAMA: Kodları Hazırlama (Build)
# .NET 8.0 SDK'sını kullanıyoruz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Proje dosyasını kopyala ve gerekli paketleri indir
COPY *.csproj ./
RUN dotnet restore

# Kalan tüm dosyaları kopyala
COPY . ./

# Projeyi yayınlanabilir hale getir (Release modu)
RUN dotnet publish -c Release -o out

# 2. AŞAMA: Çalıştırma (Runtime)
# Sadece çalıştırıcıyı (ASP.NET Core Runtime) içeren daha hafif bir imaj kullanıyoruz
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Derlenmiş dosyaları önceki aşamadan al
COPY --from=build /app/out .

# Veritabanı dosyasını da kopyala (Soruların olduğu dosya)
COPY bilbakalim.db .

# Render.com gibi siteler için port ayarı
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Uygulamayı başlat
# DİKKAT: Proje ismin farklıysa burayı düzelt!
ENTRYPOINT ["dotnet", "BilBakalimAile.dll"]