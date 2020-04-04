# deploymentHelper <!-- omit in toc -->

![logo](./logo.png)

## Table of contents <!-- omit in toc -->
- [Implemented Steps](#implemented-steps)
  - [QT-Deploy Step](#qt-deploy-step)
  - [Build documentation](#build-documentation)

## Implemented Steps
### QT-Deploy Step
Uses the 'windeployqt'-tool to deploy an QT-application. 'windeployqt' needs to be in the PATH of your OS.

This kind of step is added by using
```xml
<deploy-qt>
    ...
</deploy-qt>
```
and needs to be filled with arguments (optional) and the path to the executable to be deployed. 

Add an argument by using the `argument`-tag. Some arguments may need an given directory in order to work. To add an directory to an argument, you can use the `dir`-tag, which uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

Define the executable by using the `file`-tag. The `file`-tag uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

So one QT-Deploy Step could look as follows:
```xml
<deploy-qt>
    <argument>release</argument>
    <argument>no-translations</argument>
    <argument>no-svg</argument>            
    <argument>
        dir
        <dir path-type='rel'>../target/bin/x86_64</dir>
    </argument>
    <file path-type='rel'>../target/bin/x86_64/binary.exe</file> 
</deploy-qt>
```
and would result in
```
windeployqt --release --no-translations --no-svg --dir [...]\target\bin\x86_64 [...]\target\bin\x86_64\binary.exe
```
which will be executed.

### Build documentation
This step automatically builds doxygen docs. For the program to generate the documentation, 'doxygen' must be available in the PATH of your OS and the doxygen config-file needs to be already generated.

This kind of step is added by using
```xml
<build-doc>
    ...
</build-doc>
```
and needs to be filled with the path to the doxygen configuration file. Define this file by using the `file`-tag. The `file`-tag uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

So one build documentation step could look as follows:
```xml
<build-doc>
    <file path-type='rel'>../Doxyfile</file>
</build-doc>
```
and would result in
```
doxygen [...]/Doxyfile
```
which will be executed.