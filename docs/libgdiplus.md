# GDI Plus on dotnet core for Mac

If you are running scryber, or any other application that requires the GDI+ library, from Visual Studio for Mac or Linux, then you might encounter the following error...

```
The type initializer for ‘System.Drawing.GDIPlus’ threw an exception. — -> 
System.DllNotFoundException: Unable to load DLL ‘gdiplus’: 
The specified module or one of its dependencies could not be found.
```

Unfortunately the libgdiplus libraries are not, by default, part of the DotNet Core app. (As of version 3.1.5, and 5.0 looks like it suffers the same).

## Getting the library

Luckily there is a [HomeBrew](https://brew.sh) package that contains the library.
(_If you don't have home brew, then the link will take you to the home page to install home brew._)

To install the binaries use

```console
brew install mono-libgdiplus
```

This will add the libraries to the Cellar and add a link to `/usr/local`

You will then need to add link to this library to your Microsoft.NetCore.App application version you are using.

```console
sudo ln -s /usr/local/lib/libgdiplus.dylib /usr/local/share/dotnet/shared/Microsoft.NETCore.App/3.1.5
```

_Where 3.1.5 is the latest version we were using at the time of writing. If the dotnet core app is upgraded, then it will need to be re-linked_

## Success

If you now try to rebuild and run your application or site, then this library should be found and everything work as expected.

*Any problems, then give us a shout*
