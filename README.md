# myLanguages - Cross-Platform Localization Engine

myLanguages is a cross-platform API that allows you to convert your applications into multi-language
applications and share them across the world!

> It's cross-platform so you can use it in Xamarin, .NET Core, MVC, Mono and .NET Framework projects

[![Build Status](https://img.shields.io/travis/Tomi-15/cfgConfig.svg?style=for-the-badge)](https://travis-ci.com/Tomi-15/cfgConfig)
[![License](https://img.shields.io/badge/license-GNU%20GPLv3-blue.svg?style=for-the-badge)](https://github.com/Tomi-15/myLanguages/blob/master/LICENSE.txt)
[![OpenIssues](https://img.shields.io/github/issues-raw/Tomi-15/cfgConfig.svg?style=for-the-badge)](https://github.com/Tomi-15/myLanguages/issues)
[![Discord](https://img.shields.io/badge/Discord-Tomas%238453-orange.svg?style=for-the-badge&logo=discord)]


## Features
- Easy to use
- Fast implementation on any platform
- Language changing at runtime
- No .resx files
- Posibility to automatically add new languages
- Ready to use WPF implementation

## Installation
You can install the latest version of myLanguages via **Nuget Package Manager**

``` Shell
PM> Install-Package myLanguages.Core
```
And the **WPF** implementation
``` Shell
PM> Install-Package myLanguages.Wpf
```

## Usage
You can only have one instance of the Localization Engine in your entire application. Before start using it, you need to create that instance.

### **The Engine**

#### Instantiation 
---
To instantiate it, you will use the ```LocalizationEngine``` class and the ```MakeNew(ILanguageDecoder)``` method, which take one parameter:

 `decoder` **ILanguageDecoder**
>The decoder used to load languages

There is already two built-in language decoders you can use:
- `EmbbededLanguageDecoder` *Will search for files with the .lang extension in the embedded files of your application.*
- `EmbbededLanguageDecoder(string)` *Will search for files with the .lang extension in the directory that you specified in the parameter*

>You can create your own decoder by implementing the `ILanguageDecoder` interface in your class.

This method will return you the unique instance of the Localization Engine. Or, you can use `LocalizationEngine.Instance` to get it from anywhere.

``` csharp
var instance = LocalizationEngine.MakeNew(new PhysicalFileDecoder("InstallationPath\\Langs"));
```
### Languages

#### Defining a language
---
A language is simply a file which its name is the language name and the extension MUST be **.lang**.
Entries are defined one per line, the first value is the key, the second is the text between quotes that the key represents.

**File name:** en-US.lang
> This will create a language with a subculture **(US)**, but, it can be only the native culture **(en)**

**File content:**
```text
Texts.Welcome="Welcome to my application!"
Texts.Title1="Great Application"
Texts.Title2="Made by Tomas Wegenast"
Pages.AboutUs.MadeBy="This library was made by:"
Pages.AboutUs.SupportUs="You can help us by donating in our Patreon page:"
```

#### Loading languages
---

To start searching and loading all the found languages, you will use the `DetectLanguages(string)` method, which takes an optional parameter:

 `defaultLanguage` **string**
>The default language to use in the application. This must be a valid loaded language. If the parameter is not provided, the API will get the system language and try to set that language.

``` csharp
int loadedLangs = LocalizationEngine.Instance.DetectLanguages();
```

#### Adding new languages automatically
---
You can use the `AddOnlineLanguage(Language, CultureInfo)` method to add languages based on the input language which take two parameters:

 `sourceLanguage` **Language**
 > The language to be translated

 `toLanguage` **CultureInfo**
 > The language to translate to

<img style="vertical-align: middle;" src="https://raw.githubusercontent.com/Tomi-15/my-repo-icons/master/information-icon-48px.png">
This will read the input language file, get all the entries and translate it using an Online service

This method has two overloads:
- `AddOnlineLanguage(Language, CultureInfo, IOnlineTranslator)` *Which takes as parameters the language to be translated, the language to translate to, and the service to use.*
- `AddOnlineLanguages(Language, string[])` *Which takes as parameters the language to be translated, and an array of strings containing the languages to translate*

> Translator services can be created by implementing the interface `IOnlineTranslator`

### **Creating and implementing configurations**

#### Configure a class
---
First or all, you need to mark a class to act as a Configuration, and you do that by adding the `Config` attribute to the class:
```csharp
[Config]
public class Settings1
{
    public bool DarkModeEnabled { get; set; }

    public string Username { get; set; }

    public int MaxLoginAttemps { get; set; } = 5;
}
```
Optionally, you can define a custom name for the class by adding the `Name` property to the attribute:
```[Config(Name = "UserSettings")]```
> If it's not specified, the name of the class will be used as the name of the Configuration

#### Implementing the configuration
---
Once you have configured the class that will act as Configuration, you are ready to implement it by calling the `Implement<T>()` method, from the `Implementations` property found in the `ConfigurationManager` class:
```csharp
myManager.Implementations.Implement<Settings1>();
```
> You can use the non-generic method if you want: ```Implement(Type)``` which takes the type of your Configuration class

### **Using the configurations**

#### Get a configuration
---
To get a Configuration, you have to call the method `GetConfig<T>` from the `ConfigurationManager` class:
```csharp
var settings1 = myManager.GetConfig<Settings1>();
```
It will throw an `ConfigNotFoundException` if the Configuration is invalid or is not implemented.
> You can use the non-generic method if you want: `GetConfig(Type)` which takes the type of your Configuration class

#### Saving configurations
---
When you make a change in your Configuration class, you can wait for the auto save (if its configured), wait until the application gets closed, or you can save it by calling the extension method `SaveConfig<TConfig>(TConfig)` which takes one parameter:

 `config` **TConfig**
>The Configuration class that is going to be saved

```csharp
config.SaveConfig();
```
It will throw an `ConfigNotFoundException` if the Configuration is invalid or is not implemented.

### **Summary**
```csharp
// Console app's entry point
static void Main(string[] args)
{
    ConfigurationManager.UseConsole();

    // Create the manager
    var manager = ConfigurableManager.Make("AppData\\Roaming\\MyApp\\Settings", "myManager")
                                        .Configure(settings => // Configure manager
                                        {
                                            settings
                                                .WithAutoSaveEach(TimeSpan.FromMinutes(30))
                                                .WithSaveMode(SaveModes.Json);

                                        }).Build(); // Build it

    // Implement a configuration of type MySettings
    manager.Implementations.Implement<MySettings>();

    // Get the configuration
    var config = manager.GetConfig<MySettings>();

    // Print some values
    Console.WriteLine($"{config.DarkModeEnabled} : {config.Username}");

    // Wait until user input
    Console.ReadLine();

    // Terminate the session
    ConfigurationManager.Terminate();
}
```

### Using the Backup system
---
If you didn't enabled while configuring the manager, enable it by calling the `ConfigureBackups()` method.
> More information above

When you enable backups, at the time the application gets closed, and all configurations are saved, backups of them are made, encrypted and saved in a directory called Backups, located at the working path of the Configuration Manager specified.

If something happens to your configuration files, you can have to call **ONE TIME** the method `RestoreLastBackup()` from the `ConfigurationManager` instance after implementing your configurations.

```csharp
myManager.RestoreLastBackup();
```

After restoring, you have to eliminate that line from your program's code.

## License
Licensed under **GNU General Public License v3.0**

>For more information read LICENSE.txt

## Donate
[Buy me](https://www.paypal.me/tomasweg15?locale.x=es_XC) a Fernet ;)