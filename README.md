# Scene Director
A ScriptableObject based approach to scene management to provide stricter access, more features, and customizability without MonoBehaviours or Singletons.  Uses a ScriptableObject based `SceneDirector` to implement scene management functionality, as well as additional systems like transitions and loading screens.

Part of the [Oni](https://github.com/nmacadam/Oni) Unity toolkit.

## Installation
### Git
This package can be installed with the Unity Package Manager by selecting the add package dropdown, clicking "Add package from git url...", and entering `https://github.com/nmacadam/Scene-Director.git`.

Alternatively the package can be added directly to the Unity project's manifest.json by adding the following line:
```
{
  "dependencies": {
      ...
      "com.daruma-works.oni":"https://github.com/nmacadam/Scene-Director.git"
      ...
  }
}
```
For either option, by appending `#<release>` to the Oni.git url you can specify a specific release (e.g. Oni.git#1.0.0-preview)

### Manual
Download this repository as a .zip file and extract it, open the Unity Package Manager window, and select "Add package from disk...".  Then select the package.json in the extracted folder.
