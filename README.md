# Aufgeschlagen! Entdecke Bücherschätze im Rokokosaal

## Inhaltsverzeichnis 
* [Kurzbeschreibung](#Kurzbeschreibung) 
* [Förderhinweis](#Förderhinweis) 
* [Dokumentation](#Dokumentation) 
* [Installation](#Installation)
* [Credits](#Credits) 
* [Lizenz](#Lizenz)


### Kurzbeschreibung
Digitale Räume können den analogen Raum während des Museumsbesuchs erweitern. So finden BesucherInnen beispielsweise einen interaktiven Zugang zu Objekten, die aus konservatorischen Gründen nicht zugänglich und einsehbar sind. Wie lassen sich solche historischen Bestände im analogen Raum digital vermitteln? Exemplarisch hat die Klassik Stiftung Weimar (KSW) dies für den historischen Rokokosaal in der Herzogin Anna Amalia Bibliothek erprobt: Dazu wurde die App Weimar+ um eine AR/3D-Anwendung erweitert, die den historischen Bibliotheksbestand einsehbar macht. Mit Smartphone oder Tablet können BesucherInnen ausgewählte historische Bücher digital in einer Augmented Reality aus den Regalen nehmen. Die 3D-Anwendung ermöglicht, den historischen Einband von allen Seiten in Augenschein zu nehmen. Ein Blättermodul erlaubt den Blick ins Buch. Die Besonderheiten des Exemplars werden in kurzen Audios vermittelt.

### Förderhinweis
Das Projekt museum4punkt0 wird gefördert durch die Beauftragte der Bundesregierung für Kultur und Medien aufgrund eines Beschlusses des Deutschen Bundestages. </br>
![BKM-Logo](https://github.com/museum4punkt0/images/blob/2c46af6cb625a2560f39b01ecb8c4c360733811c/BKM_Fz_2017_Web_de.gif)
![NeustartKultur](https://github.com/museum4punkt0/media_storage/blob/a35eedb36e5b502e90cd76d669a6b337002b230a/BKM_Neustart_Kultur_Wortmarke_pos_RGB_RZ_web.jpg)

### Dokumentation
Das Projekt 'Aufgeschlagen! Entdecke Bücherschätzeim Rokokosaal' ist für Mobilgeräte (iOS/Android) konzipiert und mit der Realtime Engine Unity umgesetzt. Unity bietet die notwendigen Werkzeuge für eine interaktive AR/3D-Anwendung und hat gegenüber einer nativen Implementierung den Vorteil, dass mittels einer gemeinsamen Codebasis (C#) für beide Plattformen gleichzeitig entwickelt werden kann. 

Das Projekt in seiner hier vorliegenden Version umfasst folgenden Funktionen:

#### Auswahl
- Auswahl der einzelnen Bücher über Listenmenü

#### AR-Modus
- AR-Marker einscannen, um Bücher zu aktivieren
- Bücher können frei rotiert werden
- Bücher kehren ins Sichtfeld zurück, sobald sie nicht mehr zu sehen sind
- Wechsel in den 3D-Modus über 'Mehr erfahren'

#### 3D-Modus
- Interaktives Tutorial in mehreren Schritten
- Bücher können rotiert (Drag mit einem Finger), bewegt (Drag mit zwei Fingern) und skaliert (Pinch mit zwei Fingern) werden
- Bücher können aufgeschlagen und wieder geschlossen werden (Doppelklick)
- Bücherseiten können vor- und zurückgeblättert werden (Drag mit einem Finger)

Im Original ist dieses Projekt in der Weimar+ App der Klassik Stiftung Weimar ([App Store](https://apps.apple.com/de/app/weimar/id1457546709?l=en) / [Play Store](https://play.google.com/store/apps/details?id=de.klassikStiftung.medienguide&hl=gsw&gl=US)) als Teil der Herzogin Anna Amalia Bibliothek eingebunden. Einige Funktionen, wie beispielsweise die Einbettung der Bücher in eine umfangreiche Audiotour oder weiterführende Informationen und Audiobeiträge zu den einzelnen Büchern sind ausschließlich in der Weimar+ App verfügbar.

### Installation
Um das Projekt zu öffnen, ist eine Installation des Unity Editors vorausgesetzt. Diese kann von der offiziellen [Unity Website](https://unity.com/de) bezogen werden. Grundsätzlich sollte das Projekt mit jeder aktuellen Version kompatibel sein, empfohlen ist jedoch die Version 2020.3.21f, die im [Unity Download Archive](https://unity3d.com/de/get-unity/download/archive) zu finden ist. Das Projekt (abgesehen von seinen AR-Funktionen) kann bereits im Unity Editor getestet werden: Hierzu einfach die Szene `AnnaAmalia_Books_Boot` im Ordner `#ANNA_AMALIA/Books/Scenes` öffnen und in den `Playmodus` wechseln. Um das Projekt in seiner vollen Funktionalität testen zu können, muss das Projekt wahlweise für iOS oder Android exportiert und auf ein entsprechendes Endgerät gespielt werden. Dabei ist zu beachten, dass beim Export in den Build Settings alle Szenen aus dem Ordner `#ANNA_AMALIA/Books/Scenes` aktiviert sind und die Szene `AnnaAmalia_Books_Boot` die erste in der Reihenfolge ist (Build-Index 0). Die AR-Marker zur Anwendung sind im Ordner `#ANNA_AMALIA/Books/Scenes/XR/` zu finden.

### Credits
 
entwickelt [Guidepilot](https://www.guidepilot.de/) (a MicroMovie Media GmbH Solution)
### Lizenz
Aufgeschlagen! Entdecke Bücherschätze im Rokokosaal Copyright © 2022 Klassik Stiftung Weimar / Guidepilot (?) </br>
Das Programm ist Freie Software: Sie können es unter den Bedingungen der GNU General Public License, wie von der Free Software Foundation, Version 3 der Lizenz jeder neueren veröffentlichten Version, weiterverbreiten und/oder modifizieren. </br>
Dieses Programm wird in der Hoffnung bereitgestellt, dass es nützlich sein wird, jedoch OHNE JEDE GEWÄHR,; sogar ohne die implizite Gewähr der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK. Siehe die [GNU General Public License](https://github.com/museum4punkt0/Aufgeschlagen/blob/7f74f908f2b2a873f6ee903144c8c2d7aae3b141/LICENSE) für weitere Einzelheiten.

Sie sollten eine Kopie der GNU General Public License zusammen mit diesem Programm erhalten haben. Wenn nicht, siehe <https://www.gnu.org/licenses/>.

