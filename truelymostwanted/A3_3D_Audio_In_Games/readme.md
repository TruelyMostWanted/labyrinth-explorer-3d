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

## 2. Anwendungsbereiche

Spatial Audio ist ein Schlüsselbereich moderner Spielentwicklung:

| Bereich                     | Beschreibung |
|----------------------------|--------------|
| **Computerspiele**         | Präzises Richtungshören für Shooter, Horror-, Stealth- und Open-World-Spiele. |
| **Virtuelle Realität (VR)**| Immersion und realistische Kopfbewegungsanpassung. |
| **KI & NPCs**              | NPCs können Geräusche „hören“ und darauf reagieren. |
| **Barrierefreie Spiele**   | Präzise akustische Navigation für Spieler mit Sehbehinderung. |
| **Simulationen**           | Realistische Raumakustik für Training, Architektur, Akustikplanung. |

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

## 4. Verwendung für einen NPC

### Szenario

Ein NPC bewegt sich durch ein **Labyrinth** in Godot 4.4.  
- Der NPC besitzt einen `AudioListener3D`.  
- Geräusche werden durch `AudioStreamPlayer3D`-Instanzen im Labyrinth abgespielt.  
- Der NPC „hört“ diese Geräusche, kann die Richtung und Lautstärke interpretieren und reagiert:

| Geräuschtyp            | Verhalten des NPC |
|------------------------|-------------------|
| Leises, rhythmisches Geräusch | Neugierig nähern |
| Lautes, auffälliges Geräusch  | Schnell dorthin laufen |
| Bedrohliches Geräusch        | Fliehen / Rückzug |

---

### Technische Umsetzung

- **Basisfunktionen:**  
  - Godot liefert Positions- und Distanzinformationen bereits durch die Audio-Engine.  
  - Die Engine berechnet Pegel und Panning automatisch relativ zum Listener.

- **Erweiterung mit Spatial Audio:**  
  - *Steam Audio Plugin*: Realistischere Akustik durch Occlusion (Schall durch Wände), Reverb und HRTF.  
  - Script-Logik wertet Signalstärke und Filtereffekte aus, um zu entscheiden, ob der NPC Angst bekommt oder neugierig wird.  

- **KI-Integration:**  
  - Audioevents werden von der NPC-Logik als Signale interpretiert:  
    - Richtung = Zielorientierung.  
    - Pegel = Entfernungsschätzung.  
    - Frequenzprofil = Gefährlich vs. harmlos.  

---

### Vorteile dieser Technik

- **Immersion:** NPCs wirken „lebendiger“, da sie akustisch auf die Spielumgebung reagieren.  
- **Gameplay-Mechanik:** Spieler können Geräusche erzeugen, um NPCs abzulenken oder zu beeinflussen.  
- **Realismus:** Einfache Panning-Modelle reichen für Basisreaktionen; HRTF und Occlusion steigern Authentizität.  

---

## Fazit

Spatial Audio ist eine Schlüsseltechnologie für moderne Spiele und immersive Anwendungen.  
Engines wie Unreal bieten bereits umfassende Werkzeuge, während Godot über Community-Plugins wie Steam Audio ausgebaut werden kann.  

Für das NPC-Labyrinth-Szenario in Godot bedeutet das:
- Standardfunktionen genügen, um Basisverhalten (Annäherung, Rückzug) zu implementieren.
- Mit Steam Audio und HRTF wird das System realistischer und liefert feiner abgestufte Wahrnehmung.  
- Durch offene APIs und freie Lizenzierung eignet sich Godot ideal für Experimente mit akustikgetriebener KI.

