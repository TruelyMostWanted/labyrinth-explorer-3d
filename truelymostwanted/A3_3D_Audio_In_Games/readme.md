# README: Räumliches Echtzeit-Audio in Game Engines & NPC-Anwendungen

## 1. Definition und theoretische Anforderungen

**Räumliches Echtzeit-Audio (Spatial Audio)** ist eine Technologie, die in Computerspielen und VR/AR-Anwendungen verwendet wird, um Klänge so zu positionieren, dass sie aus einer bestimmten Richtung und Entfernung zu kommen scheinen.  
Das Ziel ist eine **realistische und immersive Klangwelt**, die das menschliche Richtungshören nachbildet.

Die wichtigsten theoretischen Grundlagen:
- **Psychoakustik**: Das menschliche Gehör bestimmt Richtung und Entfernung von Schallquellen anhand von:
  - *ITD* (Interaurale Zeitdifferenz): winzige Laufzeitunterschiede zwischen beiden Ohren (µs-Bereich).
  - *ILD* (Interaurale Pegeldifferenz): Lautstärkeunterschiede, v. a. bei hohen Frequenzen.
  - Spektrale Filterung durch Kopf, Schultern und Ohrmuscheln (HRTF).
- **Distanz- und Raumwahrnehmung**: Abnahme der Lautstärke, Frequenzdämpfung bei Entfernung, sowie Reflexionen und Nachhall.
- **Echtzeitfähigkeit**: Audiodaten müssen frame-synchron aktualisiert werden (<20 ms Latenz), damit Kopfbewegungen und dynamische Spielereignisse authentisch klingen.
- **Skalierbarkeit**: In modernen Spielen gibt es oft Dutzende simultane Quellen; Audio-Berechnungen müssen effizient sein.

---

## 2. Arten von räumlichem Audio

In Spielen gibt es verschiedene Verfahren zur Umsetzung von räumlichem Audio. Diese unterscheiden sich in Komplexität, Genauigkeit und Performance.

### 2.1 HRTF-basiertes binaurales Rendering

- **Prinzip:**  
  HRTF (Head-Related Transfer Function) modelliert, wie Kopf, Torso und Ohrmuscheln Schallwellen beeinflussen. Jeder Klang wird gefiltert, sodass er beim Hören über Kopfhörer wie aus einer bestimmten Richtung wahrgenommen wird.
  
- **Eigenschaften:**  
  - Sehr präzise Richtungswahrnehmung.
  - Besonders gut für Kopfhörer geeignet.
  - Benötigt für jede Quelle Filterung → hohe Rechenlast (O(m·N), m = Quellenzahl, N = Filterlänge).
  
- **Einsatz:**  
  VR, hochwertige Spiele, 3D-Audio für Kopfhörer.

---

### 2.2 Ambisonics (First & Higher Order)

- **Prinzip:**  
  Darstellung des gesamten Schallfelds um den Hörer durch **sphärische Harmonische**.  
  - FOA (First Order Ambisonics): 4 Kanäle (W, X, Y, Z).
  - HOA (Higher Order): Mehr Kanäle für höhere Richtungsgenauigkeit.
  
- **Eigenschaften:**  
  - Mischungen unabhängig von Anzahl der Quellen → effizient für viele Klänge.
  - Richtungsauflösung steigt quadratisch mit Ordnung.
  - Günstig in VR, da Soundfield einfach rotiert werden kann.
  
- **Einsatz:**  
  360°-Videos, VR, große Szenen mit vielen Quellen.

---

### 2.3 Geometriebasierte Modelle (Occlusion/Delay)

- **Prinzip:**  
  Einfache Modelle berechnen Dämpfung und Filterung anhand von Hindernissen oder Raumgeometrie:
  - Raycasts prüfen, ob ein Hindernis zwischen Quelle und Hörer liegt.
  - Low-Pass-Filter oder Pegelabsenkung simulieren Abschattung.
  - Verzögerungen können manuell gesetzt werden, um Echoeffekte zu imitieren.
  
- **Eigenschaften:**  
  - Sehr effizient (O(m·logG), G = Geometrieobjekte).
  - Gute Basis für viele Spiele ohne großen Rechenaufwand.
  - Weniger realistisch als vollständiges akustisches Raytracing.

- **Einsatz:**  
  Action-, Shooter- und Multiplayer-Spiele mit Performance-Fokus.

---

## 3. Technik in Game Engines

Moderne Engines bieten unterschiedliche Spatial-Audio-Funktionen. Viele setzen auf Plugins oder Middleware, um erweiterte Features zu ermöglichen.

### Übersichtstabelle

| Engine / Middleware | Eingebaute Verfahren          | Erweiterbar durch Plugins | Plattformen           | Lizenzmodell |
|---------------------|-----------------------------|-------------------------|----------------------|--------------|
| **Unity**           | 3D-Panning, Distanzattenuation | Steam Audio, Resonance, Oculus | Windows, macOS, Linux, Mobile, Konsolen, VR | Free/Pro, Plugins kostenlos |
| **Unreal Engine**   | HRTF, Ambisonics, Object Audio | Steam Audio, Oculus, Plattform-SDKs | PC, Konsolen, Mobile, VR | Kostenlos bis $1M Umsatz |
| **Godot Engine**    | 3D-Panning, einfache Filter | Steam Audio Plugin (Community) | Windows, macOS, Linux, Mobile, Web | MIT, komplett frei |
| **FMOD Studio**     | 3D-Panning, Geometry API    | Resonance, Steam Audio | Alle wichtigen Plattformen | Kostenlos bis $200k Umsatz |
| **Wwise**           | HRTF, Ambisonics, Spatial Audio SDK | Auro 3D, Resonance u. a. | PC, Konsolen, Mobile | Kommerzielle Lizenz |
| **OpenAL Soft**     | 3D-Panning, HRTF, Ambisonics | EFX (Hall, Filter) | PC, Mobile | LGPL, kostenlos |

---

### 3.1 Unity Engine

- **Stärken:** Breite Plugin-Unterstützung, einfache API, flexible Plattformabdeckung.  
- **Features:** 
  - Standardmäßig 3D-Panning.
  - HRTF über Microsoft Spatializer oder Resonance Audio.
  - Steam Audio für Raytracing-basierte Raumakustik.  
- **Einsatz:** Ideal für Indie- bis AA-Projekte, schnelle Integration ohne viel Custom-Code.  
- **Performance:** Plugins sind optimiert, HRTF erfordert aber selektive Nutzung bei vielen Quellen.  

---

### 3.2 Unreal Engine

- **Stärken:** Native High-End-Audioengine, umfangreiche DSP-Tools und Hardware-Offloading (Dolby Atmos, Tempest 3D).  
- **Features:**
  - HRTF, Ambisonics (bis 3. Ordnung), Audio Objects.
  - Submixes, Convolution Reverb, Raumportale.  
- **Einsatz:** AAA-Spiele mit komplexer Audioarchitektur.  
- **Performance:** Multi-Threaded, sehr optimiert; Plattform-Hardware kann Audio-Berechnungen übernehmen.  

---

### 3.3 Godot Engine

- **Stärken:** Open Source, schlanke API, hohe Anpassbarkeit.  
- **Features:**
  - Standardmäßig 3D-Panning, Distanzkurven, einfache Effekte.
  - Keine native HRTF-Unterstützung, aber experimentelles Steam Audio Plugin verfügbar.  
- **Einsatz:** Indie- und Hobby-Entwicklung; flexible Erweiterung durch GDExtensions.  
- **Performance:** Sehr effizient für einfaches Audio. Komplexe Modelle erfordern eigene Implementierungen oder Plugins.  

---

## 4. Erweiterte Verwendung für einen NPC (Godot Engine)

In einem Labyrinth-Setup in Godot 4.4 agiert ein NPC mit einem **AudioListener3D**, um Geräusche im Umfeld wahrzunehmen. Hier ein tiefgehenderer Überblick zur Integration und Logik:

---

### 4.1 Godot Nodes & Documentation

- **AudioStreamPlayer3D**: Spielt räumliche Klänge ab – Position, Volume, Attenuation und Panning inklusive.  
  → [Dokumentation AudioStreamPlayer3D](https://docs.godotengine.org/de/4.x/classes/class_audiostreamplayer3d.html)

- **AudioListener3D**: Repräsentiert die „Ohren“ des NPCs – verarbeitet räumliche Informationen aller Klänge im Umfeld.  
  → [Dokumentation AudioListener3D](https://docs.godotengine.org/en/4.4/classes/class_audiolistener3d.html)

- **Camera3D**: Visualisiert, was der NPC sieht; enthält auch Einstellungen zur Dopplerverschiebung, relevant bei schnellen Audioquellen.  
  → [Dokumentation Camera3D – allgemeine Informationen](https://docs.godotengine.org/en/4.4/classes/class_camera3d.html#class-camera3d)  
  → [Informationen zu Doppler-Tracking](https://docs.godotengine.org/en/4.4/classes/class_camera3d.html#enum-camera3d-dopplertracking)

---

### 4.2 Steam-Audio als GDExtension
Modernste Erweiterung: [godot-steam-audio auf GitHub](https://github.com/stechyo/godot-steam-audio) als GDExtension zur Integration von Steam Audio (Occlusion, Reverb, HRTF).

---

### 4.3 Szenario & Logik

- Der NPC verfügt über einen `AudioListener3D` sowie eine `Camera3D`, um Audio- und Sichtinformationen zu kombinieren.
- Mehrere `AudioStreamPlayer3D`-Instanzen im Labyrinth erzeugen Klänge (z. B. Schritte, Flüstern, Knarren).
- **Wahrnehmung durch den NPC:**
  - Die Lautstärke (Distance Attenuation) und Richtung (Panning) lassen Rückschlüsse auf Entfernung und Richtung zu.
  - Optional: Dopplereffekt (über Camera3D → DopplerTracking) bei sich bewegenden Quellen.

---

### 4.4 Entscheidungsmuster des NPC

Der NPC wertet akustische Eigenschaften wie Lautstärke und Klangfarbe aus, um sein Verhalten zu steuern:

| akustische Wahrnehmung    | NPC-Verhalten                        |
|---------------------------|--------------------------------------|
| Leises, gleichmäßiges Geräusch   | **Neugierig** – nähert sich vorsichtig |
| Lautes, klar geortetes Geräusch  | **Aggressiv/Schnell** – sprintet los    |
| Hörbar dumpfes, unheimliches Geräusch | **Flucht** – zieht sich zurück         |

---

### 4.5 Technische Umsetzung

- **Basisfunktionen:**  
  - Godot liefert Positions- und Distanzinformationen durch die Audio-Engine.  
  - Die Engine berechnet Pegel und Panning automatisch relativ zum Listener.
  - Alternative: Je größer die Distanz zum AudioStreamPlayer3D , desto leiser das Audio.

- **Erweiterung mit Spatial Audio:**  
  - *Steam Audio Plugin*: Realistischere Akustik durch Occlusion (Schall durch Wände), Reverb und HRTF.  
  - Script-Logik wertet Signalstärke und Filtereffekte aus, um zu entscheiden, ob der NPC Angst bekommt oder neugierig wird.  

- **KI-Integration:**  
  - Audioevents werden von der NPC-Logik als Signale interpretiert:  
    - Richtung = Zielorientierung.  
    - Pegel = Entfernungsschätzung.  
    - Frequenzprofil = Gefährlich vs. harmlos.  

---

### 4.6 Vorteile dieser Technik

- **Immersion:** NPCs wirken „lebendiger“, da sie akustisch auf die Spielumgebung reagieren.  
- **Gameplay-Mechanik:** Spieler können Geräusche erzeugen, um NPCs abzulenken oder zu beeinflussen.  
- **Realismus:** Einfache Panning-Modelle reichen für Basisreaktionen; HRTF und Occlusion steigern Authentizität.  

---

## 5. Fazit

Spatial Audio ist eine Schlüsseltechnologie für moderne Spiele und immersive Anwendungen.  
Engines wie Unreal bieten bereits umfassende Werkzeuge, während Godot über Community-Plugins wie Steam Audio ausgebaut werden kann.  

Für das NPC-Labyrinth-Szenario in Godot bedeutet das:
- Standardfunktionen genügen, um Basisverhalten (Annäherung, Rückzug) zu implementieren.
- Mit Steam Audio und HRTF wird das System realistischer und liefert feiner abgestufte Wahrnehmung.  
- Durch offene APIs und freie Lizenzierung eignet sich Godot ideal für Experimente mit akustikgetriebener KI.
