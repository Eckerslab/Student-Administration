# Student Administration


Eine plattformübergreifende **.NET MAUI**-Anwendung zur Verwaltung von Studenten, Noten, Studiengängen und Universitäten. Die Anwendung ist in eine saubere, mehrschichtige Architektur (Datenzugriff, Services, UI) aufgeteilt und nutzt Entity Framework Core für die Persistenz.


## Inhaltsverzeichnis

- [Über das Projekt](#über-das-projekt)
- [Demo](#demo)
- [Features](#features)
- [Tech-Stack](#tech-stack)
- [Architektur](#architektur)
- [Voraussetzungen](#voraussetzungen)
- [Projektstruktur](#projektstruktur)


## Über das Projekt

Mit Student Administration lassen sich Studierende anlegen, bearbeiten, filtern und löschen, Noten zu Kursen erfassen sowie der Notendurchschnitt berechnen. Die UI unterstützt mehrere Sprachen über lokalisierte Ressourcen.

Diese Anwendung wurde zu Übungszwecken erstellt um das Arbeiten mit mehrschichtiger Architektur und Entity Framework zu verinnerlichen.

---


## Demo Play Video

<p align="center">
  <a href="https://youtu.be/iz5YZ8q-z4g">
    <img src="https://img.youtube.com/vi/iz5YZ8q-z4g/maxresdefault.jpg" alt="Demo-Video ansehen" width="600">
  </a>
</p>

## Features

- Studenten anlegen, bearbeiten und löschen (CRUD)
- Studenten nach ID, Vor-/Nachname und Studiengang filtern
- Noten zu Kursen erfassen und Notendurchschnitt berechnen
- Verwaltung von Studiengängen und Universitäten
- Mehrsprachige Benutzeroberfläche (Lokalisierung über `.resx`)
- Strukturiertes Logging mit Serilog (tagesrotierende Logdateien)
- MVVM-Architektur mit dem CommunityToolkit

---

## Tech-Stack

| Bereich | Technologie |
|---|---|
| Framework | .NET 9.0 / .NET MAUI |
| Datenzugriff | Entity Framework Core 9 (SQL Server / SQLite) |
| MVVM | CommunityToolkit.Mvvm |
| UI-Komponenten | CommunityToolkit.Maui, Syncfusion.Maui |
| Logging | Serilog |
| Tests | xUnit|

---

## Architektur

Das Projekt folgt einer mehrschichtigen Architektur mit Dependency Injection (registriert in `MauiProgram.cs`) und dem Repository-Pattern für den Datenzugriff.

```
StudentAdministration.UI         →  .NET-MAUI-App (Views, ViewModels, DI-Setup)
        │
        ▼
StudentAdministrationServices    →  Geschäftslogik, Binding-/List-Modelle
        │
        ▼
StudentAdministrationDatabase    →  EF-Core-DbContext, Models, Repositories
```

`StudentAdministration.UnitTests` testet die Service- und Repository-Schicht über Mock-Repositorys.

---

## Voraussetzungen

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- .NET MAUI Workload:
  ```bash
  dotnet workload install maui
  ```
- Visual Studio 2022 (mit Workload „.NET Multi-platform App UI development") oder VS Code mit dem .NET-MAUI-Setup
- Für SQL Server: eine erreichbare SQL-Server-Instanz (lokal oder LocalDB)
- Ein gültiger **Syncfusion-Lizenzschlüssel** (siehe [Konfiguration](#konfiguration))

---


## Projektstruktur

```
.
├── StudentAdministration.UI/            # MAUI-App: Views, ViewModels, MauiProgram.cs
├── StudentAdministrationServices/       # Services + Binding-/List-Modelle
├── StudentAdministrationDatabase/       # DbContext, Models, Repositories, SampleData
└── StudentAdministration.UnitTests/     # Unit-Tests mit Mock-Repositorys
```

---
