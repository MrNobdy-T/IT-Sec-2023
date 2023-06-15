# Bug Report
Betreff: [OWASP Top 10 - Software and Data Integrity Failures]

## Zusammenfassung
Die vorgestellte Applikation von meinen Brüdern Clemens HAFENSCHMER
ähh HAFENSCHER und Onuralp METE konzentriert sich auf eine simple
Loginfunktionalität mit Datenbankanbindung. Nach erfolgreichem Login
wird eine Mockup-Seite mit Beispieldiagrammen dargestellt. Bei
genauerer Untersuchung stellt sich heraus, dass das verwendete
NuGet-Paket die Vertraulichkeit des Systems kompromittiert.

## Umgebung
- Betriebssystem: [Betriebssystemunabhängig
  (Getestet auf Windows 11 Education Version 22H2 OS build 22621.1778)]
- Browser: [Browserunabhängig
  (Getestet mit Microsoft Edge Version 114.0.1823.43 (Official build) (64-bit))]
- Weitere relevante Informationen: Keine

## Schritte zur Reproduktion
  1. Starten von Backend und Frontend.
  2. Aufrufen des Frontends im Browser.
  3. Einloggen mit Benutzername "admin123" und Passwort "admin".
  4. Beim vorherigen Schritt wurde unbemerkt die gesamte Datenbank ausgelesen.

## Erwartetes Verhalten
Es wird ein Loginvorgang ohne jegliche negative Seiteneffekte erwartet.

## Aktuelles Verhalten
Beim Login wird als Seiteneffekt die gesamte Datenbank ausgelesen und
an den Herausgeber des hypothetischen NuGet Pakets übermittelt.
(Letzteres wurde nur exemplarisch mittels Code-Kommentaren dargestellt)

## Zusätzliche Informationen
Es zeigten sich bei der Ausführung keinerlei Fehlermeldungen, der schadhafte
Vorgang wird versucht still und heimlich durchzuführen.

## Mögliche Lösung(en)
  1. Die Funktionalität des NuGet-Pakets mit einer eigenständigen
	 Implementierung ersetzen.
  2. (Das unerwünschte Verhalten des NuGet-Pakets könnte gemeldet
	 werden mit der Hoffnung auf Korrektur.)

## Gemachte Änderungen
1. Ersetzung des NuGet-Pakets mit einer internen Implementierung.
2. Umstellung auf Global Usings von C#
3. Anwenden von Coding-Richtlinien		
   (https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md)
4. Zahlreiche Objektinitialisierungen vereinfacht.

## Kontext
Es könnte sein, dass für die Übertragung der sensiblen Daten aus der Datenbank
zum Angreifer eine Internetverbindung nötig ist. Die exemplarische Implementierung
mit Code-Kommentaren hierbei lässt ein solches Verhalten ausschließen.

## Kontaktdaten
- Name: [Benjamin BOGNER]
- Matrikelnummer: [52109171]
- E-Mail: [122638@fhwn.ac.at]
- GitHub-Benutzername: [Benson-sama]
