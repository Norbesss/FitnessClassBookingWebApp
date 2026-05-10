## 1. Projekt letöltése

A projektet először le kell tölteni a verziókezelőből vagy át kell másolni a másik gépre.

Git használata esetén:

```bash
git clone https://github.com/Norbesss/FitnessClassBookingWebApp.git
```

Ezután be kell lépni a projekt főmappájába, ahol a "FitnessClassBookingWebApp.slnx" nevű található:

---

## 2. Szükséges programok

### Backend oldalhoz

- Visual Studio
- SQL Server Management Studio ha az adatbázisba be akarunk lépni

Entity Framework eszközök telepítése terminálban:

```bash
dotnet tool install --global dotnet-ef
```

Ha már telepítve van, akkor frissítés:

```bash
dotnet tool update --global dotnet-ef
```

### Frontend oldalhoz

- Node.js
- npm
- Angular CLI

Angular CLI telepítése:

```bash
npm install -g @angular/cli
```
---

## 3. Backend függőségek visszaállítása

A backend indítása előtt vissza kell állítani a NuGet csomagokat.

A solution főmappájában futtatható:

```bash
dotnet restore
```

Ez letölti a backend projektekhez szükséges csomagokat.

---

## 4. Frontend függőségek telepítése

Az Angular kliens külön függőségeket használ, ezért be kell lépni a frontend projekt mappájába:

```bash
cd fitnessclassbookingwebapp.client
```

Majd telepíteni kell az npm csomagokat:

```bash
npm install
```

---

## 5. Adatbázis kapcsolat beállítása

A kapcsolat az alábbi fájlban található:

```text
FitnessClassBookingWebApp.Server/appsettings.json
```

A "ConnectionStrings"-ben lévő "DefaultConnection" értéke fontos nekünk:

```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FitnessClassBookingDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
```

- Server Name: (localdb)\\mssqllocaldb
- Authentication: Windows Authentication
- Database Name: FitnessClassBookingDB
- Encrypt: Mandatory
- Trust Server Certificate: ki kell pipálni

Utána a "Connect" gomb megnyomása után be tudunk lépni az adatbázisba.

---

## 6. Adatbázis létrehozása migrációval

Ha a projekt Entity Framework Core migrációkat használ, akkor az adatbázis létrehozható az alábbi paranccsal.

A terminálban a projekt főmappájában, ahol a "FitnessClassBookingWebApp.slnx" található a következő parancsot kell lefuttatni:

```bash
dotnet ef database update --project FitnessClassBookingWeb.DataAccess --startup-project FitnessClassBookingWebApp.Server
```

Ez létrehozza az adatbázist és alkalmazza a migrációkat.

---

## 7. Backend indítása

A backend projekt indítása a főmappában:

```bash
dotnet run --project FitnessClassBookingWebApp.Server
```

Sikeres indítás után a szerver általában egy ilyen címen érhető el:

```text
https://localhost:xxxx
```

vagy

```text
http://localhost:xxxx
```

A pontos portszámot a konzol írja ki induláskor.

---

## 8. Frontend indítása

Egy külön terminálban be kell lépni az Angular projekt mappájába:

```bash
cd fitnessclassbookingwebapp.client
```

Majd el kell indítani az Angular fejlesztői szervert:

```bash
ng serve
```

vagy:

```bash
npm start
```

---

## 9. Gyakori hibák

### Nem indul az Angular projekt

Lehetséges ok:

```text
node_modules mappa hiányzik
```

Megoldás:

```bash
npm install
```

### Nem található az Angular CLI

Megoldás:

```bash
npm install -g @angular/cli
```

### Nem jön létre az adatbázis

Ellenőrizni kell:

- fut-e az SQL Server,
- helyes-e a connection string,
- telepítve van-e az Entity Framework CLI,
- léteznek-e migrációk.

Adatbázis frissítése:

```bash
dotnet ef database update --project FitnessClassBookingWeb.DataAccess --startup-project FitnessClassBookingWebApp.Server
```

### Bejelentkezés adminként

- Email: admin@fitness.com
- Jelszó: Admin123!
