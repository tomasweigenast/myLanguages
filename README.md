# myLanguages - Cross-Platform Localization Engine

myLanguages is a cross-platform API that allows you to convert your applications into multi-language
applications and share them across the world!

> It's cross-platform so you can use it in Xamarin, .NET Core, MVC, Mono and .NET Framework projects

[![Build Status](https://img.shields.io/appveyor/ci/bitturesoftware/myLanguages.svg?style=for-the-badge)](https://ci.appveyor.com/project/bitturesoftware/mylanguages)
[![License](https://img.shields.io/badge/license-GNU%20GPLv3-blue.svg?style=for-the-badge)](https://github.com/Tomi-15/myLanguages/blob/master/LICENSE.txt)
[![OpenIssues](https://img.shields.io/github/issues-raw/Tomi-15/myLanguages.svg?style=for-the-badge)](https://github.com/Tomi-15/myLanguages/issues)
![Discord](https://img.shields.io/badge/Discord-Tomas%238453-orange.svg?style=for-the-badge&logo=discord)


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

There are already two built-in language decoders you can use:
- `EmbbededLanguageDecoder` *Will search for files with the .lang extension in the embedded files of your application.*
- `EmbbededLanguageDecoder(string)` *Will search for files with the .lang extension in the directory that you specified in the parameter*

**NOTE:** *You can create your own decoder by implementing the `ILanguageDecoder` interface in your class.*

This method will return you the unique instance of the Localization Engine. Or, you can use `LocalizationEngine.Instance` to get it from anywhere.

``` csharp
var instance = LocalizationEngine.MakeNew(new PhysicalFileDecoder("InstallationPath\\Langs"));
```
### Languages

#### **Defining a language**
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

#### **Loading languages**
---

To start searching and loading all the found languages, you will use the `DetectLanguages(string)` method, which takes an optional parameter:

 `defaultLanguage` **string**
>The default language to use in the application. This must be a valid loaded language. If the parameter is not provided, the API will get the system language and try to set that language.

``` csharp
int loadedLangs = LocalizationEngine.Instance.DetectLanguages();
```

#### **Adding new languages automatically**
---
You can use the `AddOnlineLanguage(Language, CultureInfo)` method to add languages based on the input language which take two parameters:

 `sourceLanguage` **Language**
 > The language to be translated

 `toLanguage` **CultureInfo**
 > The language to translate to

This method has two overloads:
- `AddOnlineLanguage(Language, CultureInfo, IOnlineTranslator)` *Which takes as parameters the language to be translated, the language to translate to, and the service to use.*
- `AddOnlineLanguages(Language, string[])` *Which takes as parameters the language to be translated, and an array of strings containing the languages to translate*

**NOTE:** *This method reads the input language file, get all the entries and translate them using an Online service*

> Translator services can be created by implementing the interface `IOnlineTranslator`

There is already one built-in online translator service:
- `GoogleTranslator` *Uses the Google Translation services to translate your entries*

#### **Changing the Language**
---
You can change the current language of your application **at runtime** using the `ChangeLanguage(string)` method, which takes one parameter:

 `languageCode` **string**
 > The new language to set the application

```csharp
LocalizationEngine.Instance.ChangeLanguage("en-US");
```
This method will try to change your application's language to the specified in the parameter. If you specify a Subculture (en-**US**), first of all, this language will be searched. If it was not found, the Engine will take the Culture (**en**-US) and search for a language that contains it. If everything fails, an exception will be thrown.

#### **Retrieving an entry**
---
To get an entry, you only need the key of the entry and use the indexer property `Localizator`

```csharp
string translatedValue = LocalizationEngine.Localizator["Titles.Principal"];
```

### WPF implementation
If you are working in a WPF project, you can install the WPF implementation package and work easily.

You don't need any **xmlns** reference. To locate an entry you will use the `Loc(Key)` markup extension.

```xml
<Button Content="{Loc Key=Titles.Welcome}" />

<TextBlock Text="{Loc Texts.TextBlock}" />

<TextBox Text="{Loc KeySource={Binding myKey}" />
```

If you want to show a default text if the key is not found or something fails, you can use the `FallbackValue` property
```xml
<TextBlock Text="{Loc NotFound.Key, FallbackValue=My default value}" />
```

### Useful Methods & Properties

To get all the installed languages:
```csharp
IEnumerable<Language> installedLangs = LocalizationEngine.Instance.GetInstalledLanguages();
```

Gets the current language of the application:
```csharp
Language currentLang = LocalizationEngine.Instance.CurrentLanguage;
```

To get a language:
```csharp
Language esLang = LocalizationEngine.Instance.GetLanguage("es");
```

### **Summary**
```csharp
// Console app's entry point
static void Main(string[] args)
{
    // Setup the Localization Engine
    LocalizationEngine.MakeNew("AppData\\MyApp\\langs");

    // Detect languages and set by default French
    LocalizationEngine.Instance.DetectLanguages("fr");

    // Print installed langs
    Console.WriteLine("Installed languages:");
            foreach (var lang in LocalizationEngine.Instance.GetInstalledLanguages())
                Console.WriteLine($"\t- {lang.ToString()}");

    // Print welcome message
    Console.WriteLine(LocalizationEngine.Localizator["Texts.Welcome"]);

    Console.ReadLine();
}
```

## License
Licensed under **GNU General Public License v3.0**

>For more information read LICENSE.txt

## Donate
[Buy me](https://www.paypal.me/tomasweg15?locale.x=es_XC) a Fernet ;)