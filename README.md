# EntityFrameworkDbFirstProduct 📦

Bu proje, .NET Framework üzerinde C# ve Windows Forms kullanılarak geliştirilmiş, Entity Framework **Database First** yaklaşımıyla Ürün ve Kategori yönetimi sağlayan bir masaüstü uygulamasıdır.

## 🚀 Genel Bakış

Uygulama, var olan bir SQL Server veritabanı (`DbFirstProduct`) yapısından yola çıkarak Entity Framework modelini (`.edmx`) oluşturur. Kullanıcıların kategorileri ve bu kategorilere ait ürünleri yönetmesine olanak tanır. Temel CRUD (Create, Read, Update, Delete) işlemleri her iki varlık (Entity) için de mevcuttur.

## ✨ Özellikler

*   **Entity Framework Database First:** Var olan veritabanı şemasından otomatik model ve sınıflar (`TblCategory.cs`, `TblProduct.cs`, `Model1.Context.cs`) oluşturulmuştur.
*   **Kategori Yönetimi (`FrmCategory`):**
    *   Yeni kategori ekleme (`Create`).
    *   Mevcut kategorileri listeleme (`Read`).
    *   Kategori bilgilerini güncelleme (`Update`).
    *   Kategori silme (`Delete`).
    *   Kategori adına göre arama yapma (`Search`).
    *   Listeden seçilen kategorinin bilgilerini metin kutularına doldurma (`DataGridView_CellClick`).
*   **Ürün Yönetimi (`FrmProduct`):**
    *   Yeni ürün ekleme (`Create`) - Kategori seçimi `ComboBox` ile yapılır.
    *   Mevcut ürünleri listeleme (`Read`).
    *   Ürün bilgilerini güncelleme (`Update`).
    *   Ürün silme (`Delete`).
    *   Ürün adına göre arama yapma (`Search`).
    *   Kategorileri `ComboBox` içerisine yükleme (`FrmProduct_Load`).
    *   Ürünleri kategori adlarıyla birlikte listeleme (LINQ Join kullanarak - `btnProductListWithCategory_Click`).

## 🛠️ Kullanılan Teknolojiler

*   **Programlama Dili:** C#
*   **Framework:** .NET Framework 4.7.2
*   **Arayüz:** Windows Forms (WinForms)
*   **Veri Erişimi:** Entity Framework 6 (Database First)
*   **Veritabanı:** Microsoft SQL Server

## 💾 Veritabanı Kurulumu

Uygulamanın çalışması için `DbFirstProduct` adında bir SQL Server veritabanına ve ilgili tablolara ihtiyaç vardır.

1.  **Veritabanı ve Tabloları Oluşturma:** Aşağıdaki SQL scriptlerini bir SQL Server üzerinde çalıştırarak `DbFirstProduct` veritabanını ve tablolarını oluşturun:

    ```sql
    -- Veritabanını oluştur (eğer yoksa)
    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DbFirstProduct')
    BEGIN
        CREATE DATABASE DbFirstProduct;
    END
    GO

    USE DbFirstProduct;
    GO

    -- TblCategory Tablosu
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TblCategory]') AND type in (N'U'))
    BEGIN
        CREATE TABLE [dbo].[TblCategory](
            [CategoryId] [int] IDENTITY(1,1) NOT NULL,
            [CategoryName] [nvarchar](50) NULL,
         CONSTRAINT [PK_TblCategory] PRIMARY KEY CLUSTERED
        (
            [CategoryId] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY];
    END
    GO

    -- TblProduct Tablosu
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TblProduct]') AND type in (N'U'))
    BEGIN
        CREATE TABLE [dbo].[TblProduct](
            [ProductId] [int] IDENTITY(1,1) NOT NULL,
            [ProductName] [nvarchar](50) NULL,
            [ProductStock] [int] NULL,
            [ProductPrice] [decimal](18, 2) NULL,
            [ProductStatus] [bit] NULL, -- Bu alan modelde var, tabloda da olmalı.
            [CategoryId] [int] NULL,
         CONSTRAINT [PK_TblProduct] PRIMARY KEY CLUSTERED
        (
            [ProductId] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
        ) ON [PRIMARY];

        -- Foreign Key Constraint
        ALTER TABLE [dbo].[TblProduct] WITH CHECK ADD CONSTRAINT [FK_TblProduct_TblCategory] FOREIGN KEY([CategoryId])
        REFERENCES [dbo].[TblCategory] ([CategoryId]);

        ALTER TABLE [dbo].[TblProduct] CHECK CONSTRAINT [FK_TblProduct_TblCategory];
    END
    GO
    ```

2.  **Bağlantı Dizesi (Connection String):** Projenin `App.config` dosyasındaki `connectionStrings` bölümünü kontrol edin. `DbFirstProductEntities` adlı bağlantı dizesindeki `data source` kısmını kendi SQL Server sunucu adınızla değiştirmeniz gerekebilir. Mevcut ayar: `data source=UMUT\SQLEXPRESS`.

    ```xml
    <connectionStrings>
      <add name="DbFirstProductEntities" connectionString="metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlClient;provider connection string="data source=YOUR_SERVER_NAME;initial catalog=DbFirstProduct;integrated security=True;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework"" providerName="System.Data.EntityClient" />
    </connectionStrings>
    ```
    `YOUR_SERVER_NAME` kısmını kendi sunucu bilgilerinizle güncelleyin (örneğin, `.` , `(localdb)\mssqllocaldb`, `YOUR_PC_NAME\SQLEXPRESS` vb.).

## 🏃 Nasıl Çalıştırılır?

1.  Projeyi bilgisayarınıza klonlayın:
    ```bash
    git clone https://github.com/kullanici-adiniz/EntityFrameworkDbFirstProduct.git
    ```
    *(`kullanici-adiniz` kısmını kendi GitHub kullanıcı adınızla değiştirin)*
2.  Yukarıdaki "Veritabanı Kurulumu" adımlarını takip ederek veritabanını hazırlayın.
3.  Gerekirse `App.config` dosyasındaki bağlantı dizesini güncelleyin.
4.  `EntityFrameworkDbFirstProduct.sln` dosyasını Visual Studio (2019 veya üzeri önerilir) ile açın.
5.  NuGet Paket Yöneticisi Konsolu'nu açın (`Tools -> NuGet Package Manager -> Package Manager Console`) ve şu komutu çalıştırarak Entity Framework paketinin yüklendiğinden emin olun (genellikle `.csproj` dosyasında `PackageReference` olduğu için otomatik yüklenir, ancak kontrol etmek iyidir):
    ```powershell
    Update-Package -reinstall EntityFramework
    ```
    *(Eğer proje ilk kez açılıyorsa ve paketler eksikse, Visual Studio genellikle otomatik olarak geri yükler.)*
6.  Projeyi derleyin (Build -> Build Solution).
7.  Uygulamayı başlatın (Debug -> Start Debugging veya F5). Ana form olarak `FrmProduct` açılacaktır.

## 🔄 Modeli Güncelleme (Database First)

Eğer veritabanı şemasında (tablo ekleme/çıkarma, sütun değiştirme vb.) değişiklik yaparsanız, Entity Framework modelini güncellemeniz gerekir:

1.  Visual Studio'da Solution Explorer'da `Model1.edmx` dosyasına çift tıklayarak EDMX Tasarımcısını açın.
2.  Tasarımcı yüzeyinde boş bir alana sağ tıklayın.
3.  `Update Model from Database...` seçeneğini seçin.
4.  Açılan sihirbazda `Refresh` sekmesinden güncellenecek tabloları seçin veya `Add` sekmesinden yeni tabloları ekleyin. `Finish` butonuna tıklayın.
5.  Değişiklikleri kaydedin ve projeyi yeniden derleyin.

Bu README, projenizi anlamak ve çalıştırmak isteyenler için iyi bir başlangıç noktası olacaktır. Başarılar!
