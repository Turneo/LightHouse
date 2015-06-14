# LightHouse

The current document includes ways of solving common problems related to LightHouse.

## Testing

### Problems Discovering or Running xUnit.net Tests in Visual Studio 

- If you've previously installed the xUnit.net Visual Studio Runner VSIX (Extension), you must uninstall it first. The Visual Studio runner is only distributed via NuGet now. To remove it, to go Tools > Extensions and Updates. Scroll to the bottom of the list, and if xUnit.net is installed, uninstall it. This will force you to restart Visual Studio.
- You may be a victim of a corrupted runner cache inside Visual Studio. To clear this cache, shut down all instances of Visual Studio, then delete the folder %TEMP%\VisualStudioTestExplorerExtensions. Also make sure your project is only linked against a single version of the Visual Studio runner NuGet package (xunit.runner.visualstudio).

More information can found in the following xUnit.net documentation page:

http://xunit.github.io/docs/running-tests-in-vs.html

LightHouse is a registered trademark of Turneo AG (www.turneo.com).


