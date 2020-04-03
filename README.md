# deploymentHelper

### Implemented Steps
#### QT-Deploy Step
Uses the 'windeployqt'-tool to deploy an QT-application. 'windeployqt' needs to be in the PATH of your OS.

This kind of step is added by using
```xml
<deploy-qt>
    ...
</deploy-qt>
```
and needs to be filled with arguments (optional) and the path to the executable to be deployed. 

Add an argument by using the `argument`-tag. Define the executable by using the `file`-tag. The `file`-tag uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

So one QT-Deploy Step could look as follows:
```xml
<deploy-qt>
    <argument>release</argument>
    <argument>no-quick-import</argument>
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
windeployqt --release --no-quick-import --no-translations --no-svg --dir [...]\target\bin\x86_64 [...]\target\bin\x86_64\binary.exe
```
which will be executed.