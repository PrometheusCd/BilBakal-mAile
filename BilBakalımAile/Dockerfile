# 1. AŞAMA: Kodları Hazırlama (Build)
# .NET 10.0 SDK'sını kullanıyoruz (GÜNCELLENDİ)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Proje dosyasını kopyala ve gerekli paketleri indir
COPY *.csproj ./
RUN dotnet restore

# Kalan tüm dosyaları kopyala
COPY . ./

# Projeyi yayınlanabilir hale getir (Release modu)
RUN dotnet publish -c Release -o out

# 2. AŞAMA: Çalıştırma (Runtime)
# .NET 10.0 Runtime imajını kullanıyoruz (GÜNCELLENDİ)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# Derlenmiş dosyaları önceki aşamadan al
COPY --from=build /app/out .

# Veritabanı dosyasını da kopyala (Soruların olduğu dosya)
# DİKKAT: Eğer projenin içinde "bilbakalim.db" dosyası yoksa bu satır hata verir.
COPY bilbakalim.db .

# Render.com gibi siteler için port ayarı
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "BilBakalimAile.dll"]